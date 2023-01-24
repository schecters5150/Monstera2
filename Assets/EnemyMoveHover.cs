using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMoveHover : MonoBehaviour
{
    // Start is called before the first frame update
    public float hitstunSpeed;
    private Rigidbody2D rb;
    public GameObject returnPosition;
    private AIDestinationSetter aiDestiniationSetter;
    private Transform playerTransform;
    private EnemyStatusModel enemyStatusModel;
    private AIPath aiPath;

    void Start()
    {
        aiDestiniationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        enemyStatusModel = GetComponent<EnemyStatusModel>();
        playerTransform = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyStatusModel.isHitstun)
        {
            aiPath.enabled = false;
            HitstunMove();
        }
        else if (enemyStatusModel.isInLOS)
        {
            aiPath.enabled = true;
            aiDestiniationSetter.target = playerTransform;
        }
        else {
            aiPath.enabled = true; 
            aiDestiniationSetter.target = returnPosition.transform; 
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            Debug.Log("beep boop");
        }
    }
}
