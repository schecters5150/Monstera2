using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    private GameObject Enemy;

    // Start is called before the first frame update
    void Start()
    {
        Enemy = this.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            Enemy.GetComponent<EnemyStatusModel>().isInLOS = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            Enemy.GetComponent<EnemyStatusModel>().isInLOS = false;
        }
    }
}
