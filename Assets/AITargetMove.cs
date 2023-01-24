using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITargetMove : MonoBehaviour
{
    public GameObject target;
    public GameObject los;
    public float distanceX;
    public float distanceY;
    private int directionX;
    private int directionY;

    private Transform playerTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.position.x > transform.position.x) directionX = -1;
        if (playerTransform.position.x < transform.position.x) directionX = 1;
        if (playerTransform.position.y > transform.position.y) directionY = -1;
        if (playerTransform.position.y < transform.position.y) directionY = 1;
        target.transform.position = new Vector3(playerTransform.position.x + distanceX*directionX, playerTransform.position.y + distanceY * directionY, 0);
    }
}
