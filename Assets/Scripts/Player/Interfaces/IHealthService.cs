using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthService 
{
    public int GetHealth();
    public void SetHealth(int health);
    public void ReduceHealth(int reduction);
    public void TriggerGradualReduceHealth(int reduction, float time);
    public float GradualReduceHealth();
    public void TriggerInvincibility(float time);
    public bool CalculateInvincibility();
    public void Health();
}
