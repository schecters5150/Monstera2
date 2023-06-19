using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxHealth;
    public int health;

    public float hitstunTime;
    Timer hitstunTimer;
    public float invincibilityTime;
    Timer invincibilityTimer;

    private EnemyStatusModel enemyStatusModel;

    public void Start()
    {
        health = maxHealth;
        hitstunTimer = new Timer();
        invincibilityTimer = new Timer();
        enemyStatusModel = GetComponentInParent<EnemyStatusModel>();
    }

    public void Update()
    {
        if (invincibilityTimer.IsUp()) enemyStatusModel.isInvincible = false;
        if (hitstunTimer.IsUp())
        {
            enemyStatusModel.isHitstun = false;
        }

        if (health <= 0) Destroy(this.transform.parent.gameObject);

        
    }

    public void FixedUpdate()
    {
        invincibilityTimer.CalculateTime();
        hitstunTimer.CalculateTime();
    }

    public void ReduceHealth(int health)
    {
        this.health -= health;
    }

    public void TriggerHitstun()
    {
        if (enemyStatusModel.isStunnable)
        {
            hitstunTimer.Trigger(hitstunTime);
            enemyStatusModel.isHitstun = true;
        }
    }

    public void TriggerInvincibility()
    {
        invincibilityTimer.Trigger(invincibilityTime);
        enemyStatusModel.isInvincible = true;
    }

    public void HitDetection(Collider2D collision)
    {
        if (collision.tag == "Hitbox" && collision.transform.parent.gameObject.name == "Player")
        {
            ReduceHealth(1);
            TriggerHitstun();
            TriggerInvincibility();
        }
        if (collision.tag == "Player")
        {
            TriggerHitstun();
        }
    }


}
