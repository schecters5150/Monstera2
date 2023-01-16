using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float timer;
    private bool isUp;
    public void Trigger(float time)
    {
        timer = time;
    }

    public void CalculateTime()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            isUp = false;
        }
        else isUp = true;
    }

    public bool IsUp()
    {
        return isUp;
    }

    public float GetTime()
    {
        return timer;
    }
}
