using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMoveHoverTarget : MonoBehaviour
{
    // Start is called before the first frame update
    public float hitstunSpeed;
    private Rigidbody2D rb;
    public GameObject returnPosition;
    private AIDestinationSetter aiDestiniationSetter;
    public GameObject targetTransform;
    public GameObject playerObject;
    private EnemyStatusModel enemyStatusModel;
    private AIPath aiPath;
    private bool edgeDetect;
    Timer edgeDetectTimer;
    public float edgeDetectMoveTime;
    private ContactPoint2D contact;
    float edgeDetectAngle;
    void Start()
    {
        playerObject = GameObject.Find("Player");
        edgeDetectTimer = new Timer();
        aiDestiniationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        enemyStatusModel = GetComponent<EnemyStatusModel>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (enemyStatusModel.isHitstun)
        {
            aiPath.enabled = false;
            HitstunMove();
        }
        else if (edgeDetect)
        {
            aiPath.enabled = false;
            EdgeDetectMove();
        }
        else if (enemyStatusModel.isInLOS)
        {
            aiPath.enabled = true;
            aiDestiniationSetter.target = targetTransform.transform;
        }
        else
        {
            aiPath.enabled = true;
            aiDestiniationSetter.target = returnPosition.transform;
        }
        edgeDetectTimer.CalculateTime();
        if (edgeDetect && edgeDetectTimer.IsUp()) edgeDetect = false;
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

    private void EdgeDetectMove()
    {
        float velocityX;
        float velocityY;
        if (playerObject.transform.position.x > transform.position.x) {
            velocityX = Mathf.Cos(edgeDetectAngle) * hitstunSpeed * Time.fixedDeltaTime;
        }
        else
        {
            velocityX = -Mathf.Cos(edgeDetectAngle) * hitstunSpeed * Time.fixedDeltaTime;
        }
        if (playerObject.transform.position.y > transform.position.y)
        {
            velocityY = Mathf.Cos(edgeDetectAngle) * hitstunSpeed * Time.fixedDeltaTime;
        }
        else
        {
            velocityY = -Mathf.Cos(edgeDetectAngle) * hitstunSpeed * Time.fixedDeltaTime;
        }
        Vector3 newPosition = new Vector3(rb.position.x + velocityX, rb.position.y + velocityY, 0);
        rb.position = newPosition;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && !edgeDetect)
        {
            if (playerObject.GetComponent<PlayerController>()._statusModel.isDodging) return;

            var playerPosition = playerObject.transform.position;
            var distanceX = playerPosition.x - transform.position.x;
            var distanceY = playerPosition.y - transform.position.y;

            edgeDetectAngle = Mathf.Atan(distanceY / distanceX);

            edgeDetect = true;
            edgeDetectTimer.Trigger(edgeDetectMoveTime);
        }
    }
}
