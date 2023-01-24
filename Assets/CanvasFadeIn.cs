using System.Collections;
using System.Collections.Generic;
using TheraBytes.BetterUi;
using UnityEngine;
using UnityEngine.UI;

public class CanvasFadeIn : MonoBehaviour
{
    private Image image;
    public float maxFadeTime;
    Timer fadeTimer;
    // Start is called before the first frame update
    void Start()
    {
        fadeTimer = new Timer();
        image = GetComponent<BetterImage>();
        fadeTimer.Trigger(maxFadeTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!fadeTimer.IsUp())
        {
            fadeTimer.CalculateTime();
            image.color = new Color(0,0,0, fadeTimer.GetTime() / maxFadeTime);
        }
    }
}
