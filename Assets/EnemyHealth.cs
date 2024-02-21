using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxHealth;
    public float health;
    public EnemyHealth attachedObjectHealth;
    public bool triggerAttachedObjectHitstun;
    public bool reduceAttachedObjectHealth;

    public float hitstunTime;
    Timer hitstunTimer;
    public float invincibilityTime;
    Timer invincibilityTimer;
    public bool hasPoise;
    public float maxPoise;
    public float poise;
    public float poiseHealRate;
    public float poiseBreakTime;
    Timer poiseTimer;
    public GameObject deathObject;

    private EnemyStatusModel enemyStatusModel;
    private EnemySoundController enemySoundController;

    public void Start()
    {
        health = maxHealth;
        poise = maxPoise;
        hitstunTimer = new Timer();
        invincibilityTimer = new Timer();
        poiseTimer = new Timer();
        enemyStatusModel = GetComponentInParent<EnemyStatusModel>();
        enemySoundController = GetComponentInParent<EnemySoundController>();
    }

    public void Update()
    {
        if (invincibilityTimer.IsUp()) enemyStatusModel.isInvincible = false;
        if (hitstunTimer.IsUp())
        {
            enemyStatusModel.isHitstun = false;
        }

        if (health <= 0) Death();
    }

    public void Death()
    {
        if (deathObject != null) { Instantiate(deathObject, transform.position, transform.rotation); }
        enemySoundController.PlayDeathNoise();
        Destroy(this.transform.parent.gameObject);
    }

    public void FixedUpdate()
    {
        invincibilityTimer.CalculateTime();
        hitstunTimer.CalculateTime();
        poiseTimer.CalculateTime();

        if (poise < maxPoise && !enemyStatusModel.isPoiseBroken)
        {
            poise += .016f * poiseHealRate;
        }
        if (poiseTimer.IsUp() && enemyStatusModel.isPoiseBroken)
        {
            poise = maxPoise;
            enemyStatusModel.isPoiseBroken = false;
        }
    }

    public void ReduceHealth(float health)
    {
        this.health -= health;

        if (attachedObjectHealth != null && reduceAttachedObjectHealth)
        {
            attachedObjectHealth.ReduceHealth(health);
        }
    }

    public void ReducePoise(float poiseDamage)
    {
        if (!hasPoise || enemyStatusModel.isPoiseBroken) return;

        this.poise -= poiseDamage;

        if (this.poise <= 0)
        {
            poise = 0;
            enemyStatusModel.isPoiseBroken = true;
            poiseTimer.Trigger(poiseBreakTime);
        }
    }

    public void TriggerHitstun()
    {
        if (enemyStatusModel.isStunnable)
        {
            hitstunTimer.Trigger(hitstunTime);
            enemyStatusModel.isHitstun = true;
        }
        if (attachedObjectHealth != null && triggerAttachedObjectHitstun)
        {
            attachedObjectHealth.TriggerHitstun();
        }
    }

    public void TriggerInvincibility()
    {
        invincibilityTimer.Trigger(invincibilityTime);
        enemyStatusModel.isInvincible = true;
    }

    public void HitDetection(Collider2D collision)
    {
        if (collision.tag == "Hitbox")
        {
            ReduceHealth(collision.gameObject.GetComponent<PlayerDamage>().healthDamage);
            ReducePoise(collision.gameObject.GetComponent<PlayerDamage>().poiseDamage);
            TriggerHitstun();
            TriggerInvincibility();
        }
        if (collision.tag == "Player")
        {
            TriggerHitstun();
        }
    }
}
