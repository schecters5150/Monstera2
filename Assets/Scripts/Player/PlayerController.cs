using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Prime31.CharacterController2D _controller;
    public MoveService _moveService;
    public Animator _animator;
    public StatusModel _statusModel;
    public SpriteRenderer _spriteRenderer;
    public InputManager _inputManager;
    public PlayerTimerManager _timerManager;
    public IntersceneStatusModel _interstateModel;
    public PlayerHealth _playerHealth;
    public GameObject _onHitFeedback;
    public GameObject _zoomInFeedback;
    public GameObject _zoomOutFeedback;
    public GameObject _playerSpriteObject;

    private SoundController _soundController;
    public float dodgeGateRefresh;
    private bool inZoomOutZone;
    private bool dodgeGateSemiphor = false;
    private SpriteRenderer _playerSprite;

    private bool enemySemiphor = false;


    #region Startup functions
    void Start()
    {
        _moveService = GetComponent<MoveService>();
        _inputManager = GetComponent<InputManager>();
        _timerManager = GetComponent<PlayerTimerManager>();
        _controller = GetComponent<Prime31.CharacterController2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _statusModel = GetComponent<StatusModel>();
        _interstateModel = GetComponent<IntersceneStatusModel>();
        _playerHealth = GetComponent<PlayerHealth>();
        _playerSprite = _playerSpriteObject.GetComponent<SpriteRenderer>();
        _soundController = GetComponent<SoundController>();
        SetInterstateParams();
    }

    public void SetInterstateParams()
    {
        var selected = _interstateModel.GetLoadPosition();
        _moveService.SetCheckPoint(selected.transform.position.x, selected.transform.position.y);
        _moveService.MoveToLastCheckpoint();
    }
    #endregion


    #region loop procedures
    void Update()
    {
        _statusModel.isGrounded = _controller.isGrounded;

        AnimationTriggers();
        DebugReload();
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Enemy" && !_statusModel.isInvincible && !_statusModel.isDodging && !_statusModel.isParrying)
        {
            if (_statusModel.isDodging) return;
            _moveService.SetBumpDirection(GetBumpDirection(collider));
            _timerManager.invincibilityTimer.Trigger(_timerManager.maxInvincibilityTime);
            _timerManager.hitstunTimer.Trigger(_timerManager.maxHitstunTime);
        }
        if (collider.tag == "DodgeGate")
        {
            if (!_statusModel.isDodging)
            {
                _moveService.DisableHover();
                _moveService.SetBumpDirection(GetBumpDirection(collider));
                _timerManager.bumpTimer.Trigger(_timerManager.maxBumpTime);
            }
        }

    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "CheckPoint")
        {
            _moveService.SetCheckPoint(collider.gameObject.transform.position.x, collider.gameObject.transform.position.y);
        }
        if (collider.tag == "Hazard")
        {
            _playerHealth.ReduceHealth(_playerHealth.maxHealth / 10);
            _playerHealth.RefreshAllStamina();
            _moveService.MoveToLastCheckpoint();
            _timerManager.disableMoveTimer.Trigger(.5f);
        }
        if (collider.tag == "LoadTrigger")
        {
            ES3.Save("health", _playerHealth.health);
            ES3.Save("heals", _playerHealth.heals);
            ES3.Save("checkpointId", collider.GetComponent<CheckpointId>().id);
            SceneManager.LoadScene(collider.GetComponent<SceneId>().id);
        }

        if (collider.tag == "DodgeGate" && _statusModel.isDodging && !dodgeGateSemiphor)
        {
            dodgeGateSemiphor = true;
            _playerHealth.AddStamina(dodgeGateRefresh);
        }
        if (collider.tag == "Enemy" && !enemySemiphor && !_statusModel.isDodging)
        {
            enemySemiphor = true;

            if (_statusModel.isParrying)
            {
                _soundController.PlayParrySuccess();
            }
            else
            {
                if (!_statusModel.isInvincible)
                {
                    _onHitFeedback.GetComponent<MMFeedbacks>().PlayFeedbacks();
                    _playerHealth.ReduceHealth(collider.gameObject.GetComponent<EnemyHitbox>().damage);
                    _timerManager.invincibilityTimer.Trigger(_timerManager.maxInvincibilityTime);
                }
                _moveService.SetBumpDirection(GetBumpDirection(collider));
                _timerManager.hitstunTimer.Trigger(_timerManager.maxHitstunTime);
            }
        }
        if (collider.tag == "ZoomOutZone")
        {
            inZoomOutZone = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "DodgeGate")
        {

            dodgeGateSemiphor = false;
        }
        if (collider.tag == "Enemy")
        {
            enemySemiphor = false;
        }
        if (collider.tag == "ZoomOutZone")
        {
            inZoomOutZone = false;
        }
    }

    public void AnimationTriggers()
    {
        _animator.SetBool("AnimHitstun", _statusModel.isHitstun);

        //Walking 
        if (_controller.isGrounded && _controller.velocity.x != 0)
        {
            _animator.SetBool("AnimWalk", true);
            _statusModel.isWalking = true;
        }
        else
        {
            _animator.SetBool("AnimWalk", false);
            _statusModel.isWalking = false;
        }

        //Directionality
        if (_controller.velocity.x < 0 && !_statusModel.isAttackAnimation && !_statusModel.isCling)
        {
            _playerSprite.flipX = true;
        }
        if (_controller.velocity.x > 0 && !_statusModel.isAttackAnimation && !_statusModel.isCling)
        {
            _playerSprite.flipX = false;
        }
        if (_statusModel.isAttacking)
        {
            if (GetComponent<AttackService>().attackLeftFlag)
            {
                _playerSprite.flipX = true;
            }
            if (GetComponent<AttackService>().attackRightFlag)
            {
                _playerSprite.flipX = false;
            }
        }
        if (_statusModel.isCling)
        {
            if (_moveService.clingJumpDirection > 0)
            {
                _playerSprite.flipX = false;
            }
            if (_moveService.clingJumpDirection < 0)
            {
                _playerSprite.flipX = true;
            }
        }

        //Rotation
        if (_controller.isGrounded || _statusModel.isAttacking || !_moveService.isHovering)
        {
            _playerSprite.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (_moveService.isHovering)
        {
            if (_controller.velocity.x > 0)
            {
                _playerSprite.transform.rotation = Quaternion.Euler(0f, 0f, _moveService.GetAngle() * 57.2958f);
            }
            if (_controller.velocity.x < 0)
            {
                _playerSprite.transform.rotation = Quaternion.Euler(0f, 0f, 360 - _moveService.GetAngle() * 57.2958f);
            }
        }

        //Zooms
        if (_inputManager.ZoomIsPressed() || inZoomOutZone)
        {
            _zoomInFeedback.GetComponent<MMFeedbacks>().PlayFeedbacks();
        }
        else _zoomOutFeedback.GetComponent<MMFeedbacks>().PlayFeedbacks();

        //Hover
        if (_moveService.isHovering)
        {
            _statusModel.isHovering = true;
            _animator.SetBool("AnimHover", true);
        }
        else if (!_controller.isGrounded)
        {
            _statusModel.isHovering = false;
            _animator.SetBool("AnimFall", true);
            _animator.SetBool("AnimHover", false);
        }
        else
        {
            _statusModel.isHovering = false;
            _animator.SetBool("AnimHover", false);
            _animator.SetBool("AnimFall", false);
        }

        //Dodge
        if (_statusModel.isDodging && !_moveService.isHovering)
        {
            _animator.SetBool("AnimDodge", true);
        }
        else
        {
            _animator.SetBool("AnimDodge", false);
        }

        //Wall cling
        if (_statusModel.isCling)
        {
            _animator.SetBool("AnimWallCling", true);
        }
        else { _animator.SetBool("AnimWallCling", false); }
    }


    public bool IsInAttackAnimation()
    {
        if (AnimatorIsPlaying("AttackLeft")) return true;
        if (AnimatorIsPlaying("AttackRight")) return true;
        if (AnimatorIsPlaying("AttackUp")) return true;
        if (AnimatorIsPlaying("AttackDown")) return true;

        return false;
    }

    bool AnimatorIsPlaying(string stateName)
    {
        return AnimatorIsPlaying() && _animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    bool AnimatorIsPlaying()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).length >
               _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    #endregion

    public int GetBumpDirection(Collider2D collider)
    {
        return (GetComponent<Transform>().position.x < collider.gameObject.transform.position.x) ? -1 : 1;

    }

    private void DebugReload()
    {
        if (_inputManager.DodgeIsPressed() && _inputManager.HoverIsPressed() && _inputManager.JumpIsPressed() && _inputManager.StartIsPressed())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


}