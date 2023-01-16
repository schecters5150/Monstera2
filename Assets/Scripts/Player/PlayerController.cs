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

    public float dodgeGateRefresh;
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
        if (collider.tag == "Enemy" && !_statusModel.isInvincible && !_statusModel.isDodging)
        {
            if (_statusModel.isDodging) return;
            _moveService.SetBumpDirection(GetBumpDirection(collider));
            _timerManager.disableMoveTimer.Trigger(_timerManager.maxHitstunTime * .25f);
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
            _moveService.MoveToLastCheckpoint();
            _timerManager.disableMoveTimer.Trigger(.5f);
        }
        if (collider.tag == "LoadTrigger")
        {
            ES3.Save("health", _playerHealth.health);
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
            _onHitFeedback.GetComponent<MMFeedbacks>().PlayFeedbacks();
            _playerHealth.ReduceHealth(collider.gameObject.GetComponent<EnemyController>().damageDealt);
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
    }

    public void AnimationTriggers()
    {
        _animator.SetBool("AnimHitstun", _statusModel.isHitstun);

        //Walking direction
        if (_controller.isGrounded && _controller.velocity.x != 0) _animator.SetBool("AnimWalk", true);
        else _animator.SetBool("AnimWalk", false);


        if (_controller.velocity.x < 0 && (!AnimatorIsPlaying("AttackLeft") || !AnimatorIsPlaying("AttackRight")) && !_statusModel.isCling)
        {
            _playerSprite.flipX = true;
        }
        if (_controller.velocity.x > 0 && (!AnimatorIsPlaying("AttackLeft") || !AnimatorIsPlaying("AttackRight")) && !_statusModel.isCling)
        {
            _playerSprite.flipX = false;
        }

        if (_controller.isGrounded)
        {
            _playerSprite.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }


            //Zooms
            if (_inputManager.ZoomIsPressed())
        {
            _zoomInFeedback.GetComponent<MMFeedbacks>().PlayFeedbacks();
        }
        else _zoomOutFeedback.GetComponent<MMFeedbacks>().PlayFeedbacks();

        //Hover

        if (!_controller.isGrounded && _moveService.isHovering)
        {
            _animator.SetBool("AnimHover", true);
            _playerSprite.transform.rotation = Quaternion.Euler(0f, 0f, _moveService.GetAngle() * 57.2958f);
            this.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, _moveService.GetAngle() * 57.2958f); ;

            if (_controller.velocity.x > 0)
            {
                _playerSprite.flipX = false;
                _playerSprite.transform.rotation = Quaternion.Euler(0f, 0f, _moveService.GetAngle() * 57.2958f); ;
                this.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, _moveService.GetAngle() * 57.2958f); ;

            }
            if (_controller.velocity.x < 0)
            {
                _playerSprite.flipX = true;
                _playerSprite.transform.rotation = Quaternion.Euler(0f, 0f, 360 - _moveService.GetAngle() * 57.2958f); ;
                this.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, _moveService.GetAngle() * 57.2958f); ;
            }

        }
        else if (!_controller.isGrounded && !_moveService.isHovering)
        {
            _playerSprite.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            this.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            _animator.SetBool("AnimFall", true);
            _animator.SetBool("AnimHover", false);
        }
        else
        {
            _animator.SetBool("AnimHover", false);
            _animator.SetBool("AnimFall", false);
        }


        if (_statusModel.isDodging)
        {
            var color = _spriteRenderer.color;
            var timeLeft = (_timerManager.maxDodgeTime - _timerManager.dodgeTimer.GetTime()) / _timerManager.maxDodgeTime;
            _spriteRenderer.material.color = new Color(color.r, color.g, timeLeft, timeLeft);
        }
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