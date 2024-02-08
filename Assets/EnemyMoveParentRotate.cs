using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveParentRotate : MonoBehaviour
{
    public float speed;
    private Vector3 moveDirection;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveDirection = Direction();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private Vector2 Direction()
    {
        var direction = new Vector3(0, 0, 0);
        var x = Math.Cos(transform.rotation.eulerAngles.z * .0174);
        var y = Math.Sin(transform.rotation.eulerAngles.z * .0174);
        var scalar = Math.Sqrt(x * x + y * y);
        direction.x = (float)(x / scalar);
        direction.y = (float)(y / scalar);
        return direction;
    }
    private void Move()
    {
        Vector3 newPosition = new Vector3(rb.position.x + speed * moveDirection.x * Time.deltaTime,
            rb.position.y + speed * moveDirection.y * Time.deltaTime,
            0);
        rb.position = newPosition;
    }
}
