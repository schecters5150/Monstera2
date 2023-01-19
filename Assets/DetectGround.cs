using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectGround : MonoBehaviour
{
    public EnemyStatusModel enemyStatusModel;
    // Start is called before the first frame update
    void Start()
    {
        enemyStatusModel = GetComponent<EnemyStatusModel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
        {
            enemyStatusModel.isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            enemyStatusModel.isGrounded = false;
        }
    }
}
