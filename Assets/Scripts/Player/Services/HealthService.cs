using System;
using UnityEngine;

public class HealthService
{

    private int health;
    private float gradualHealthTimer;
    private int gradualHealthReduction;
    private float invincibilityTimer = 0;


    public HealthService(int health)
    {
        this.health = health;
    }

    public void Health()
    {
        health = GetHealth();
        GradualReduceHealth();
        CalculateInvincibility();
    }

    public int GetHealth()
    {
        return health;
    }
    public void SetHealth(int health)
    {
        this.health = health;
    }
    public void ReduceHealth(int reduction)
    {
        if (!(invincibilityTimer > 0)) health -= reduction;
    }
    public void TriggerGradualReduceHealth(int reduction, float time)
    {
        gradualHealthTimer = time;
        gradualHealthReduction = reduction;
    }
    public float GradualReduceHealth()
    {
        if(gradualHealthTimer > 0)
        {
            health -= gradualHealthReduction;
            gradualHealthTimer -= Time.deltaTime;
        }

        return gradualHealthTimer;
    }

    public void TriggerInvincibility(float time)
    {
        invincibilityTimer = time;
    }

    public void CalculateInvincibility()
    {
        if (invincibilityTimer > 0) invincibilityTimer -= Time.deltaTime;
    }

    public bool IsInvincible()
    {
        if (invincibilityTimer > 0) return true;
        else return false;
    }
}
