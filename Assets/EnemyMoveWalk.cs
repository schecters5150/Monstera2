using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMoveWalk : MonoBehaviour
{
    // Start is called before the first frame update
    public float hitstunSpeed;
    public float walkSpeed;
    public float stopDistance;
    private Rigidbody2D rb;
    public GameObject returnPosition;
    private Transform playerTransform;
    private EnemyStatusModel enemyStatusModel;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyStatusModel = GetComponent<EnemyStatusModel>();
        playerTransform = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemyStatusModel.isHitstun)
        {
            HitstunMove();
        }
        else if (enemyStatusModel.isInLOS)
        {
            Walk(playerTransform);
        }
        else
        {
            Walk(returnPosition.transform);
        }
    }

    private void HitstunMove()
    {
        if (GameObject.Find("Player").GetComponent<PlayerController>()._statusModel.isDodging) return;

        var playerPosition = GameObject.Find("Player").transform.position;
        bool horizontal = Mathf.Abs(playerPosition.x - rb.position.x) >= Mathf.Abs(playerPosition.y - rb.position.y);

        if (horizontal)
        {
            var direction = rb.position.x < playerPosition.x ? -1 : 1;
            Vector3 newPosition = new Vector3(rb.position.x + hitstunSpeed * direction * Time.deltaTime, rb.position.y, 0);
            rb.position = newPosition;
        }
        else
        {
            var direction = rb.position.y < playerPosition.y ? -1 : 1;
            Vector3 newPosition = new Vector3(rb.position.x, rb.position.y + hitstunSpeed * direction * Time.deltaTime, 0);
            rb.position = newPosition;
        }
    }

    private void Walk(Transform target)
    {
        var direction = rb.position.x > target.position.x ? -1 : 1;
        Vector3 newPosition = new Vector3(rb.position.x + walkSpeed * direction * Time.deltaTime, rb.position.y, 0);
        if(Mathf.Abs(rb.position.x - target.position.x) > stopDistance) rb.position = newPosition;
    }
}
