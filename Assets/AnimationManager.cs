using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform playerTransform;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.Find("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationFlip();
    }

    private void AnimationFlip()
    {
        if (rb.position.x > playerTransform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        if (rb.position.x < playerTransform.position.x)
        {
            spriteRenderer.flipX = false;
        }
    }
}
