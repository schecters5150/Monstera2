using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMoveHover : MonoBehaviour
{
    // Start is called before the first frame update
    public float hitstunSpeed;
    public Rigidbody2D rb;
    public GameObject hitstunPosition;
    public GameObject LineOfSite;
    private AIDestinationSetter aiDestiniationSetter;
    private Transform playerTransform;
    private EnemyStatusModel enemyStatusModel;

    void Start()
    {
        aiDestiniationSetter = GetComponent<AIDestinationSetter>();
        enemyStatusModel = GetComponent<EnemyStatusModel>();
        playerTransform = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyStatusModel.isHitstun)
        {
            HitstunMove();
        }
        else if (enemyStatusModel.isInLOS) {
            aiDestiniationSetter.target = playerTransform;
        }
        else aiDestiniationSetter.target = LineOfSite.transform;
    }

    private Transform SetHitstunTransform()
    {
        if (GameObject.Find("Player").GetComponent<PlayerController>()._statusModel.isDodging) return transform;

        var playerPosition = GameObject.Find("Player").transform.position;
        bool horizontal = Mathf.Abs(playerPosition.x - transform.position.x) >= Mathf.Abs(playerPosition.y - transform.position.y);

        if (horizontal)
        {
            var direction = transform.position.x < playerPosition.x ? -1 : 1;
            Vector3 newPosition = new Vector3(transform.position.x + 10 * direction * Time.deltaTime, transform.position.y, 0);
            hitstunPosition.transform.position = newPosition;
        }
        else
        {
            var direction = transform.position.y < playerPosition.y ? -1 : 1;
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + 10 * direction * Time.deltaTime, 0);
            hitstunPosition.transform.position = newPosition;
        }

        return hitstunPosition.transform;
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Hitbox" || collider.tag == "Player")
        {
                SetHitstunTransform();          
        }
    }
}
