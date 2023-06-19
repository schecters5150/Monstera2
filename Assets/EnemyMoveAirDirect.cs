using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyMoveAirDirect : MonoBehaviour
{
    public float speed;
    public Transform target;
    private Vector3 moveDirection;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (target == null)
        {
            target = GameObject.Find("Player").transform;
        }
        moveDirection = Direction();
        Rotation();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }
    private void Rotation()
    {
        Vector3 targ = target.transform.position;
        targ.z = 0f;

        Vector3 objectPos = transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;

        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    private Vector2 Direction()
    {
        var direction = new Vector3(0, 0, 0);
        var x = target.position.x - rb.position.x;
        var y = target.position.y - rb.position.y;
        var scalar = math.sqrt(x * x + y * y);
        direction.x = x / scalar;
        direction.y = y / scalar;
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
