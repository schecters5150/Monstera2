using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimerManager : MonoBehaviour
{
    private StatusModel _statusModel;
    private InputManager _inputManager;
    private PlayerHealth _playerHealth;

    public float maxDodgeTime;
    public Timer dodgeTimer;
    public float maxHitstunTime;
    public Timer hitstunTimer;
    public float maxBumpTime;
    public Timer bumpTimer;
    public float maxDisableAttackTime;
    public Timer disableAttackTimer;
    public float maxDisableDodgeTime;
    public Timer disableDodgeTimer;
    public float maxClingJumpTime;
    public Timer clingJumpTimer;
    public float maxInvincibilityTime;
    public Timer invincibilityTimer;
    public Timer disableMoveTimer;
    public Timer jumpTimer;
    public Timer attackTimer;
    public Timer attackAnimationTimer;
    public Timer parryTimer;


    void Start()
    {
        _inputManager = GetComponent<InputManager>();
        _statusModel = GetComponent<StatusModel>();
        InitiateTimers();
    }

    // Update is called once per frame
    void Update()
    {
        SetInputStatus();  
        SetTimerInputs();
    }

    private void FixedUpdate()
    {
        RunTimers();
    }

    void InitiateTimers()
    {
        dodgeTimer = new Timer();
        hitstunTimer = new Timer();
        bumpTimer = new Timer();
        disableAttackTimer = new Timer();
        disableDodgeTimer = new Timer();
        clingJumpTimer = new Timer();
        invincibilityTimer = new Timer();
        disableMoveTimer = new Timer();
        jumpTimer = new Timer();
        attackTimer = new Timer();
        attackAnimationTimer = new Timer();
        parryTimer = new Timer();   
    }

    private void RunTimers()
    {
        dodgeTimer.CalculateTime();
        hitstunTimer.CalculateTime();
        bumpTimer.CalculateTime();
        disableAttackTimer.CalculateTime();
        disableDodgeTimer.CalculateTime();
        clingJumpTimer.CalculateTime();
        invincibilityTimer.CalculateTime();
        disableMoveTimer.CalculateTime();
        jumpTimer.CalculateTime();
        attackTimer.CalculateTime();
        attackAnimationTimer.CalculateTime();
        parryTimer.CalculateTime();
    }

    void SetTimerInputs()
    {
        if (dodgeTimer.IsUp() && _statusModel.isDodging)
        {
            if (_statusModel.isDodging) disableDodgeTimer.Trigger(maxDisableDodgeTime);
            _statusModel.isDodging = false;
        }
        if (hitstunTimer.IsUp()) _statusModel.isHitstun = false;
        if (bumpTimer.IsUp()) _statusModel.isBump = false;
        if (disableAttackTimer.IsUp()) _statusModel.disableAttack = false;
        if (disableDodgeTimer.IsUp()) _statusModel.disableDodge = false;
        if (clingJumpTimer.IsUp()) _statusModel.isClingJump = false;
        if (invincibilityTimer.IsUp()) _statusModel.isInvincible = false;
        if (disableMoveTimer.IsUp()) _statusModel.disableMove = false;
    }

    private void SetInputStatus()
    {
        if (_inputManager.DodgeTriggered() && !_statusModel.disableDodge)
        {
            dodgeTimer.Trigger(maxDodgeTime);
            _statusModel.isDodging = true;
        }
        if (_inputManager.JumpTriggered() && _statusModel.isCling && !_statusModel.isGrounded)
        {
            clingJumpTimer.Trigger(maxClingJumpTime);
        }
        if (!hitstunTimer.IsUp()) _statusModel.isHitstun = true;
        if (!bumpTimer.IsUp()) _statusModel.isBump = true;
        if (!disableAttackTimer.IsUp()) _statusModel.disableAttack = true;
        if (!disableDodgeTimer.IsUp()) _statusModel.disableDodge = true;
        if (!clingJumpTimer.IsUp()) _statusModel.isClingJump = true;
        if (!invincibilityTimer.IsUp()) _statusModel.isInvincible = true;
        if (!disableMoveTimer.IsUp()) _statusModel.disableMove = true;
        if (!attackTimer.IsUp()) _statusModel.isAttacking = true;
        if (!attackAnimationTimer.IsUp()) _statusModel.isAttackAnimation = true;

        if (!parryTimer.IsUp()) _statusModel.isParrying = true;
        else { _statusModel.isParrying = false; }
    }
}
