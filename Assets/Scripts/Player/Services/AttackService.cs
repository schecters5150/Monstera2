using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackService : MonoBehaviour
{
    public int damage;
    public float swordSwingTime;
    public float attackDelayTime;
    public int swordStaminaReduction;
    public bool attackLeftFlag;
    public bool attackRightFlag;
    public bool attackDownFlag;

    public GameObject hitboxLeft;
    public GameObject hitboxRight;
    public GameObject hitboxUp;
    public GameObject hitboxDown;
    public GameObject _playerSprite;
    public AudioClip hitSound;
    

    private InputManager inputManager;
    private Animator animator;
    private StatusModel statusModel;
    private PlayerTimerManager timerManager;
    private PlayerHealth healthService;
    private AudioSource audioSource;


    public void Start()
    {
        statusModel = GetComponent<StatusModel>();
        inputManager = GetComponent<InputManager>();
        animator = GetComponent<Animator>();
        timerManager = GetComponent<PlayerTimerManager>();
        healthService = GetComponent<PlayerHealth>();
        audioSource = GetComponent<AudioSource>();
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
        ClearAttackFlags();
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
        AudioSource.PlayClipAtPoint(hitSound, transform.position);
        timerManager.attackTimer.Trigger(swordSwingTime);
        healthService.ReduceStamina(swordStaminaReduction);
        ResetSpriteRotation();
        statusModel.isAttacking = true;
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

    public void ClearAttackFlags()
    {
        if (!statusModel.isAttacking) { 
            attackLeftFlag = false;
            attackRightFlag = false;
            attackDownFlag = false;
        }
    }

    public void SetStatusModel(StatusModel statusModel)
    {
        this.statusModel = statusModel;
    }
}
