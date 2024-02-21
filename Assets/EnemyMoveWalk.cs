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
    public Animator _animator;
    public SpriteRenderer _spriteRenderer;
    private Rigidbody2D rb;
    public GameObject returnPosition;
    private Transform playerTransform;
    private EnemyStatusModel enemyStatusModel;

    void Start()
    {
        enemyStatusModel = GetComponentInParent<EnemyStatusModel>();
        playerTransform = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemyStatusModel.isPoiseBroken) ;//do nothing
        else if (enemyStatusModel.isHitstun)
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
        

        if(rb.position.x != target.position.x) _animator.SetBool("AnimWalk", true);
        else _animator.SetBool("AnimWalk", false);

        if(direction > 0) _spriteRenderer.flipX = false;
        else _spriteRenderer.flipX = true;

        if (Mathf.Abs(rb.position.x - target.position.x) > stopDistance) rb.position = newPosition;


    }
}
