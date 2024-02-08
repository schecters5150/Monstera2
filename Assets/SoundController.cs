using Pathfinding.Ionic.Zip;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public StatusModel statusModel;
    public AudioClip potteryBreak;
    public AudioClip attackWhoosh;
    public AudioClip walk;
    public AudioClip hover;
    public AudioClip hit;
    public AudioClip parry;
    public AudioClip parrySuccess;
    private AudioSource audioSource;

    public float walkTimerDelay;
    private Timer walkDelayTimer;
    public float hoverTimerDelay;
    private Timer hoverDelayTimer;

    private void Start()
    {
        statusModel = GetComponent<StatusModel>();
        audioSource = transform.parent.Find("UiAndCamera").Find("Camera").Find("SFX").GetComponent<AudioSource>();
        walkDelayTimer = new Timer();
        hoverDelayTimer = new Timer();
    }

    public void PlayWhoosh()
    {
        audioSource.PlayOneShot(attackWhoosh);
    }

    public void PlayHit()
    {
        audioSource.PlayOneShot(hit);
    }

    public void PlayParry()
    {
        audioSource.PlayOneShot(parry);
    }

    public void PlayParrySuccess()
    {
        audioSource.PlayOneShot(parrySuccess);
    }

    public void PlayShatter()
    {
        audioSource.PlayOneShot(potteryBreak);
    }

    public void Update()
    {
        var test = walkDelayTimer.IsUp();
        if (statusModel.isWalking && walkDelayTimer.IsUp()) {
            audioSource.PlayOneShot(walk);
            walkDelayTimer.Trigger(walkTimerDelay);
        }
        /*if (statusModel.isHovering && hoverDelayTimer.IsUp())
        {
            audioSource.PlayOneShot(hover);
            hoverDelayTimer.Trigger(hoverTimerDelay);
        }*/
        walkDelayTimer.CalculateTime();
        //hoverDelayTimer.CalculateTime();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
