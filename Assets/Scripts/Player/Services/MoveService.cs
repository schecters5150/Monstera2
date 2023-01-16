using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveService : MonoBehaviour
{

    public float bounceSpeed;
    public float dodgeSpeed;
    public float maxHoverSpeed;
    public float hoverMod;
    public float gravity;
    public float hitstunSpeed;
    public float jumpForce;
    public float maxJumpTime;
    public int maxJumps;
    public float walkSpeed;
    public float bumpSpeed;
    public float clingSpeed;
    public float clingJumpSpeed;

    public float jumpStaminaDeplete;
    public float hoverStaminaDeplete;
    public float dodgeStaminaDeplete;

    public int previousDirection;
    private int direction;
    private int jumpsLeft;

    private bool jumpFlag;
    private float angle;
    private bool disableHover;
    private int bumpDirection;
    private int dodgeDirection;
    private int clingJumpDirection;


    private StatusModel _statusModel;
    private Vector2 lastCheckPoint;

    private Prime31.CharacterController2D _controller;
    private InputManager _inputManager;
    private PlayerTimerManager _timerManager;
    private InventoryModel _inventoryModel;
    private PlayerHealth _playerHealth;

    public bool isHovering;

    public void Start()
    {
        _statusModel = GetComponent<StatusModel>();
        _controller = GetComponent<Prime31.CharacterController2D>();
        _inputManager = GetComponent<InputManager>();
        _timerManager = GetComponent<PlayerTimerManager>();
        _inventoryModel = GetComponent<InventoryModel>();
        _playerHealth = GetComponent<PlayerHealth>();

        if (!_inventoryModel.debugDoubleJump) maxJumps--;
    }

    public void Update()
    {
        ControllerMove();
    }

    public void ControllerMove()
    {
        var velocity = _controller.velocity;
        if (_controller.isGrounded) ResetJumps();
        if (!_statusModel.disableMove)
        {
            Jump(ref velocity);
            AirMovement(ref velocity);
            Dodge(ref velocity);
            Walk(ref velocity);
            HitStun();
            Bump();
        }
        else
        {
            velocity.x = 0;
            velocity.y = 0;
        }

        if (_controller.velocity.x > 0) previousDirection = 1;
        else if (_controller.velocity.x < 0) previousDirection = -1;

        _controller.move(velocity * Time.deltaTime);
    }

    #region Movements
    public void Walk(ref Vector3 velocity)
    {
        if (!isHovering && !_statusModel.isDodging && !_statusModel.isClingJump)
        {
            var inputX = _inputManager.MovementX();
            if (inputX < 0) velocity.x = -walkSpeed;
            else if (inputX > 0) velocity.x = walkSpeed;
            else velocity.x = 0;
        }
        if (_statusModel.isAttacking && _statusModel.isGrounded) velocity.x = 0;
    }

    public void Jump(ref Vector3 velocity)
    {
        if (_statusModel.isClingJump)
        {
            velocity.x = clingJumpDirection * clingJumpSpeed;
        }
        if (_inputManager.JumpTriggered() && jumpsLeft > 0 && !_statusModel.isDodging && !_statusModel.staminaDepleted)
        {
            _timerManager.jumpTimer.Trigger(maxJumpTime);
            jumpFlag = true;
        }
        if (!_timerManager.jumpTimer.IsUp() && jumpFlag && !isHovering)
        {
            velocity.y = (maxJumpTime - _timerManager.jumpTimer.GetTime()) * jumpForce;
        }
        if (jumpFlag && !_inputManager.JumpIsPressed() && _timerManager.jumpTimer.GetTime() < maxJumpTime * .3)
        {
            disableHover = false;
            jumpFlag = false;
            jumpsLeft--;
        }
    }

    public void AirMovement(ref Vector3 velocity)
    {
        HoverHitBoxRotation();
        if (_inputManager.HoverIsPressed() && !_controller.isGrounded && !disableHover && !_statusModel.staminaDepleted)
        {
            CalculateHover(ref velocity, gravity);
            _playerHealth.ReduceStamina(hoverStaminaDeplete * Time.deltaTime);
            isHovering = true;
        }
        else if (_statusModel.isCling && velocity.y < 0 && _inventoryModel.debugWallCling)
        {
            isHovering = false;
            velocity.y = -clingSpeed;
            jumpsLeft = jumpsLeft++;
            disableHover = false;
        }
        else
        {
            isHovering = false;
            angle = -.1f;
            velocity.y += -gravity * Time.deltaTime;
        }

        if (_controller.isGrounded && !_statusModel.isCling) DisableHover();
    }

    public void Dodge(ref Vector3 velocity)
    {
        if (!_statusModel.isDodging || _statusModel.disableDodge || _statusModel.staminaDepleted) return;

        if (isHovering)
        {
            velocity.x += dodgeSpeed * Time.deltaTime * GetDirectionX();
            velocity.y += dodgeSpeed * Time.deltaTime * GetDirectionY();
        }
        else
        {
            velocity.x = _inputManager.MovementX() * dodgeSpeed;
            velocity.y = 0;
        }

        _playerHealth.ReduceStamina(dodgeStaminaDeplete * Time.deltaTime);
    }

    public void HitStun()
    {
        if (!_statusModel.isHitstun && !_statusModel.disableMove) return;

        var velocity = _controller.velocity;
        velocity.x = hitstunSpeed * bumpDirection;
        _controller.move(velocity * Time.deltaTime);
    }

    public void Bump()
    {
        if (!_statusModel.isBump) return;

        var velocity = _controller.velocity;
        velocity.x = bumpSpeed * bumpDirection;
        _controller.move(velocity * Time.deltaTime);
    }

    public void Bounce()
    {
        _controller.velocity.y = bounceSpeed;
    }

    #endregion

    #region Hover functions
    public void DisableHover()
    {
        disableHover = true;
    }

    public void CalculateHover(ref Vector3 velocity, float gravity)
    {
        var dragCoef = 2f;
        var thrustCoef = 1.5f;
        var speed = Mathf.Sqrt(Mathf.Pow(velocity.x, 2) + Mathf.Pow(velocity.y, 2));
        if (speed > maxHoverSpeed) speed = maxHoverSpeed;
        angle = GetNextAngle(angle);

        if (angle >= 0)
        {
            speed *= 1 - dragCoef * Mathf.Sin(angle) * Time.deltaTime;
            velocity.x = speed * GetDirectionX() * Mathf.Cos(angle);
            velocity.y = speed * Mathf.Sin(angle);
        }
        else
        {
            speed *= 1 + thrustCoef * -Mathf.Sin(angle) * Time.deltaTime;
            velocity.x = speed * GetDirectionX() * Mathf.Cos(angle);
            velocity.y = speed * Mathf.Sin(angle);
        }
        if (speed < .15) disableHover = true;
    }
    public float GetNextAngle(float angle)
    {
        var stickInput = _inputManager.MovementY();

        if (angle < 1 && stickInput > 0) return angle + stickInput * hoverMod * Time.deltaTime;
        if (angle > -.6 && stickInput < 0) return angle + stickInput * hoverMod * Time.deltaTime;
        else return angle;
    }

    public void HoverHitBoxRotation()
    {
        if (_controller.isGrounded || GetComponent<AttackService>().IsAttackingAnimation() || !isHovering)
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (isHovering)
        {
            if (_controller.velocity.x > 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(0f, 0f, GetAngle() * 57.2958f);
            }
            if (_controller.velocity.x < 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 360 - GetAngle() * 57.2958f);
            }
        }
    }

    #endregion

    #region Math and resets
    public void ResetJumps()
    {
        jumpsLeft = maxJumps;
        isHovering = false;
        if(!_inputManager.HoverIsPressed()) disableHover = false;
    }
    public void MoveToLastCheckpoint()
    {
        _controller.velocity.x = 0;
        _controller.velocity.y = 0;
        _controller.transform.position = new Vector3(lastCheckPoint.x, lastCheckPoint.y, 0);
    }
    public void SetCheckPoint(float posX, float posY)
    {
        lastCheckPoint.x = posX;
        lastCheckPoint.y = posY;
    }
    public int GetDirectionX()
    {
        if (_controller.velocity.x > 0) direction = 1;
        else if (_controller.velocity.x < 0) direction = -1;
        else direction = previousDirection;

        return direction;
    }

    public int GetDirectionY()
    {
        if (_controller.velocity.y > 0) direction = 1;
        if (_controller.velocity.y < 0) direction = -1;

        return direction;
    }

    public void SetBumpDirection(int bumpDirection)
    {
        this.bumpDirection = bumpDirection;
    }

    public void SetClingJumpDirection(int clingJumpDirection)
    {
        this.clingJumpDirection = clingJumpDirection;
    }

    public float GetAngle()
    {
        return this.angle;
    }

    #endregion
}
