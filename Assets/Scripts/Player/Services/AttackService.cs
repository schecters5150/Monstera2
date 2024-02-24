using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackService : MonoBehaviour
{
    public int damage;
    public float swordSwingTime;
    public float attackDelayTime;
    public float parryTime;
    public int swordStaminaReduction;
    public bool attackLeftFlag;
    public bool attackRightFlag;
    public bool attackDownFlag;

    public GameObject hitboxLeft;
    public GameObject hitboxRight;
    public GameObject hitboxUp;
    public GameObject hitboxDown;
    public GameObject _playerSprite;

    private InputManager inputManager;
    private InventoryModel inventoryModel;
    private Animator animator;
    private StatusModel statusModel;
    private PlayerTimerManager timerManager;
    private PlayerHealth healthService;
    private SoundController soundController;
    private SpellManager spellManager;

    public void Start()
    {    
        inventoryModel = GetComponent<InventoryModel>();
        statusModel = GetComponent<StatusModel>();
        inputManager = GetComponent<InputManager>();
        animator = GetComponent<Animator>();
        timerManager = GetComponent<PlayerTimerManager>();
        healthService = GetComponent<PlayerHealth>();
        soundController = GetComponent<SoundController>();
        spellManager = GetComponent<SpellManager>();
    }
    public void Update()
    {
        ClearHitboxes();
        CheckAttack();
        CheckParry();
        CheckSpell();
    }
    public void CheckParry()
    {
        try
        {
            if (inputManager.ParryTriggered() && !statusModel.isAttacking)
            {
                soundController.PlayParry();
                timerManager.parryTimer.Trigger(parryTime);
            }
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }
    }
    public void CheckSpell()
    {
        try
        {
            if (inputManager.SpellTriggered())
            {
                var pos = new Vector3(transform.position.x, transform.position.y + 1f, 0);
                Instantiate(spellManager.activeSpellPrefab, pos, Quaternion.Euler(0, 0, 0));
                timerManager.attackAnimationTimer.Trigger(swordSwingTime + attackDelayTime);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void CheckAttack()
    {
        if (!statusModel.disableAttack && !statusModel.isCling)
        {
            AttackRight();
            AttackLeft();
            AttackUp();
            AttackDown();
        }
        ClearAttackFlags();
        ClearAttackTimer();
    }
    private void AttackRight()
    {
        if (inputManager.AttackRightTriggered() && !statusModel.isAttacking)
        {
            SwordFunctions();
            attackRightFlag = true;
            hitboxRight.SetActive(true);
            animator.SetTrigger("TrAttackRight");
        }
    }
    private void AttackLeft()
    {
        if (inputManager.AttackLeftTriggered() && !statusModel.isAttacking)
        {
            SwordFunctions();
            attackLeftFlag = true;
            hitboxLeft.SetActive(true);
            animator.SetTrigger("TrAttackLeft");
        }
    }
    private void AttackUp()
    {
        if (inputManager.AttackUpTriggered() && !statusModel.isAttacking)
        {
            SwordFunctions();
            hitboxUp.SetActive(true);
            animator.SetTrigger("TrAttackUp");
        }
    }
    private void AttackDown()
    {
        if (inputManager.AttackDownTriggered() && !statusModel.isAttacking && !statusModel.isGrounded)
        {
            SwordFunctions();
            hitboxDown.SetActive(true);
            animator.SetTrigger("TrAttackDown");
            attackDownFlag = true;
        }
    }

    private void SwordFunctions()
    {
        soundController.PlayWhoosh();
        timerManager.attackTimer.Trigger(swordSwingTime);
        timerManager.attackAnimationTimer.Trigger(swordSwingTime + attackDelayTime);
        healthService.ReduceStamina(swordStaminaReduction);
        ResetSpriteRotation();
        statusModel.isAttacking = true;
        statusModel.isAttackAnimation = true;
    }

    private void ResetSpriteRotation()
    {
        _playerSprite.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void ClearHitboxes()
    {
        if (statusModel.isAttacking && timerManager.attackTimer.IsUp())
        {
            hitboxUp.SetActive(false);
            hitboxDown.SetActive(false);
            hitboxLeft.SetActive(false);
            hitboxRight.SetActive(false);
            statusModel.isAttacking = false;
            timerManager.disableAttackTimer.Trigger(attackDelayTime);
        }
        if (timerManager.attackAnimationTimer.IsUp())
        {
            statusModel.isAttackAnimation = false;
        }
    }

    public void ClearAttackFlags()
    {
        if (!statusModel.isAttacking)
        {
            attackLeftFlag = false;
            attackRightFlag = false;
            attackDownFlag = false;
        }
    }

    public void ClearAttackTimer()
    {
        if (statusModel.isCling)
        {
            timerManager.attackTimer.Trigger(-1);
        }
    }

    public void SetStatusModel(StatusModel statusModel)
    {
        this.statusModel = statusModel;
    }
}
