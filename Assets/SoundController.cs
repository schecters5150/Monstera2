using Pathfinding.Ionic.Zip;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public StatusModel statusModel;
    public AudioClip potteryBreak;
    public AudioClip attackWhoosh;
    public AudioClip hit;
    private AudioSource audioSource;

    private void Start()
    {
        statusModel = GetComponent<StatusModel>();
        audioSource = transform.parent.Find("UiAndCamera").Find("Camera").Find("SFX").GetComponent<AudioSource>();
    }

    public void PlayWhoosh()
    {
        audioSource.PlayOneShot(attackWhoosh);
    }

    public void PlayHit()
    {
        audioSource.PlayOneShot(hit);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!statusModel.isInvincible)
        {
            if(collision.tag == "Enemy")
            {
                audioSource.PlayOneShot(potteryBreak);
            }
        }
    }
}
