using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float timer;
    public void Trigger(float time)
    {
        timer = time;
    }

    public void CalculateTime()
    {
        if (timer > 0) timer -= Time.fixedDeltaTime;
    }

    public bool IsUp()
    {
        return (timer <= 0);
    }

    public float GetTime()
    {
        return timer;
    }
}
