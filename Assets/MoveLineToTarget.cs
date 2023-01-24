using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLineToTarget : MonoBehaviour
{
    private GameObject playerObject;
    private Rigidbody2D rb;
    public float speed;
    private float angle;
    private float velocityX;
    private float velocityY;
    private List<string> allowedTypes = new List<string>()
    {
        "Enemy",
        "LineOfSight",
        "Untagged"
    };
    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        var playerPosition = playerObject.transform.position;
        var distanceX = playerPosition.x - transform.position.x;
        var distanceY = playerPosition.y - transform.position.y;

        angle = Mathf.Atan(distanceY / distanceX);

        if (playerObject.transform.position.x > transform.position.x)
        {
            velocityX = Mathf.Cos(angle) * speed * Time.fixedDeltaTime;
        }
        else
        {
            velocityX = -Mathf.Cos(angle) * speed * Time.fixedDeltaTime;
        }
        if (playerObject.transform.position.y > transform.position.y)
        {
            velocityY = Mathf.Cos(angle) * speed * Time.fixedDeltaTime;
        }
        else
        {
            velocityY = -Mathf.Cos(angle) * speed * Time.fixedDeltaTime;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();      
    }

    private void Move()
    {
        
        
        Vector3 newPosition = new Vector3(rb.position.x + velocityX, rb.position.y + velocityY, 0);
        rb.position = newPosition;

    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!allowedTypes.Contains(collision.collider.tag)) Destroy(this.transform.parent.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!allowedTypes.Contains(collider.tag)) Destroy(this.transform.parent.gameObject);
    }
}
