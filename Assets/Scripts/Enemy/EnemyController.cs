using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Enemy;


public class EnemyController : MonoBehaviour
{

    private HealthService _healthService;

    public EnemyMove enemyMove;
    public EnemyDetect enemyDetect;
    public MoveType moveType;
    public float moveSpeed;
    public Rigidbody2D rb;

    public int maxHealth;
    public int health;
    public int damageDealt;
    public bool isInvincible;
    public bool isHitstun;
    public bool isStunnable;
    public float maxStunTimer;
    public float stunTimer;
    public float stunSpeed;
    public bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyMove = new EnemyMove(this);
        enemyMove.moveType = moveType;
    }

    private void Awake()
    {
        _healthService = new HealthService(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        Health();
        HitstunTimer();
        Move();
    }

    public void Move()
    {
        if (enemyDetect == null) return;
        if (enemyDetect.triggered && !isHitstun) enemyMove.TriggerMove();
    }

    public void HitstunTimer()
    {
        if (isHitstun)
        {
            stunTimer -= Time.deltaTime;
            enemyMove.TriggerHitstun(stunSpeed);
            if (stunTimer < 0) isHitstun = false;
        }
    }

    public void Health()
    {
        health = _healthService.GetHealth();
        _healthService.GradualReduceHealth();
        _healthService.CalculateInvincibility();
        if (health <= 0) Destroy(this.gameObject);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            isHitstun = true;
            stunTimer = maxStunTimer;
        }
        if (collider.tag == "Ground") isGrounded = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Hitbox")
        {
            if (!_healthService.IsInvincible())
            {
                _healthService.ReduceHealth(5); // TODO 
                _healthService.TriggerInvincibility(maxStunTimer);
                if (isStunnable && !isHitstun)
                {
                    isHitstun = true;
                    stunTimer = maxStunTimer;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Ground") isGrounded = false;
    }




}
