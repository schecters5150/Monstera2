using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool isPlayerProjectile;
    public bool isThruGround;
    public bool isThruHitbox;
    public bool isThruEnemy;
    public Timer existTimer;
    public float duration;
    // Start is called before the first frame update
    void Start()
    {
        existTimer = new Timer();
        existTimer.Trigger(duration);
    }

    // Update is called once per frame
    void Update()
    {
        if(duration > 0)
        {
            if (existTimer.IsUp()) Destroy(this.gameObject);
            existTimer.CalculateTime();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Ground" && !isThruGround)
        {
            Destroy(this.gameObject);
        }

        if (collider.tag == "Hitbox" && !isThruHitbox)
        {
            Destroy(this.gameObject);
        }

        if (collider.tag == "Enemy" && !isThruEnemy)
        {
            Destroy(this.gameObject);
        }

        if (collider.tag == "Player" && !isPlayerProjectile)
        {
            Destroy(this.gameObject);
        }
    }
}
