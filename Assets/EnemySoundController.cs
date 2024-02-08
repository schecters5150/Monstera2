using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundController : MonoBehaviour
{
    public AudioClip deathSound;
    public AudioClip attackSound;
    private AudioSource audioSource;

    // Start is called before the first frame update
    private void Start()
    {
        audioSource = GameObject.Find("Template").transform.Find("UiAndCamera").Find("Camera").Find("SFX").GetComponent<AudioSource>();
    }

    public void PlayDeathNoise()
    {
        audioSource.PlayOneShot(deathSound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
