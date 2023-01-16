using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackService : MonoBehaviour
{
    public int damage;
    public float swordSwingTime;
    public float attackDelayTime;
    public bool downFlag;

    public GameObject hitboxLeft;
    public GameObject hitboxRight;
    public GameObject hitboxUp;
    public GameObject hitboxDown;
    public GameObject _playerSprite;

    private InputManager inputManager;
    private Animator animator;
    private StatusModel statusModel;
    private PlayerTimerManager timerManager;


    public void Start()
    {
        statusModel = GetComponent<StatusModel>();
        inputManager = GetComponent<InputManager>();
        animator = GetComponent<Animator>();
        timerManager = GetComponent<PlayerTimerManager>();
    }
    public void Update()
    {
        ClearHitboxes();
        CheckAttack();    
    }

    public void CheckAttack()
    {
        if (!statusModel.disableAttack) {
            AttackRight();
            AttackLeft();
            AttackUp();
            AttackDown();
        }
    }
    private void AttackRight()
    {
        if (inputManager.AttackRightTriggered() && !statusModel.isAttacking)
        {
            ResetSpriteRotation();
            _playerSprite.GetComponent<SpriteRenderer>().flipX = false;
            hitboxRight.SetActive(true);
            timerManager.attackTimer.Trigger(swordSwingTime);
            statusModel.isAttacking = true;
            animator.SetTrigger("TrAttackRight");
        }           
    }
    private void AttackLeft()
    {
        if (inputManager.AttackLeftTriggered() && !statusModel.isAttacking)
        {
            ResetSpriteRotation();
            _playerSprite.GetComponent<SpriteRenderer>().flipX = true;
            hitboxLeft.SetActive(true);
            timerManager.attackTimer.Trigger(swordSwingTime);
            statusModel.isAttacking = true;
            animator.SetTrigger("TrAttackLeft");
        }
    }
    private void AttackUp()
    {
        if (inputManager.AttackUpTriggered() && !statusModel.isAttacking)
        {
            ResetSpriteRotation();
            hitboxUp.SetActive(true);
            timerManager.attackTimer.Trigger(swordSwingTime);
            statusModel.isAttacking = true;
            animator.SetTrigger("TrAttackUp");
        }
    }
    private void AttackDown()
    {
        if (inputManager.AttackDownTriggered() && !statusModel.isAttacking)
        {
            ResetSpriteRotation();
            hitboxDown.SetActive(true);
            timerManager.attackTimer.Trigger(swordSwingTime);
            statusModel.isAttacking = true;
            animator.SetTrigger("TrAttackDown");
            downFlag = true;
        }
    }

    private void ResetSpriteRotation()
    {
        _playerSprite.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void ClearHitboxes()
    {
        if(statusModel.isAttacking && timerManager.attackTimer.IsUp())
        {
            hitboxUp.SetActive(false);
            hitboxDown.SetActive(false);
            hitboxLeft.SetActive(false);
            hitboxRight.SetActive(false);
            statusModel.isAttacking = false;
            timerManager.disableAttackTimer.Trigger(attackDelayTime);
        }
    }

    public void SetStatusModel(StatusModel statusModel)
    {
        this.statusModel = statusModel;
    }

    public void ResetDownFlag()
    {
        downFlag = false;
    }
}
