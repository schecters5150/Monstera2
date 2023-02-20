using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public StatusModel statusModel;
    public AudioSource audioSource;
    public AudioClip potteryBreak;
    public AudioClip attackWhoosh;

    private void Start()
    {
        statusModel = GetComponent<StatusModel>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!statusModel.isInvincible)
        {
            if(collision.tag == "Enemy")
            {
                AudioSource.PlayClipAtPoint(potteryBreak, transform.position);
            }
        }
    }
}
