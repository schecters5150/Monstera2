using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProjectileMove : MonoBehaviour
{
    public SpellType spellType;
    public SpriteRenderer spriteRenderer;

    public int directionX;
    public int directionY;
    public int speedX;
    public int speedY;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {   
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        if (spellType == SpellType.horizontalLob) HorizontalLob();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HorizontalLob()
    {
        directionX = GameObject.Find("Player").transform.GetComponent<MoveService>().GetDirectionX();
        if (directionX == 1) spriteRenderer.flipX = false;
        if (directionX == -1) spriteRenderer.flipX = true;
        rb.velocity = new Vector3 (directionX * speedX, 0, 0);
    }
}

public enum SpellType
{
    horizontalLob
}
