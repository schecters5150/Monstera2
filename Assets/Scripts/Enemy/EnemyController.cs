using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Enemy;


public class EnemyController : MonoBehaviour
{
    public int damageDealt;
    public GameObject healthObj;

    private EnemyStatusModel enemyStatusModel;
    private EnemyHealth enemyHealth;




    void Start()
    {
        enemyStatusModel = GetComponentInParent<EnemyStatusModel>();
        if (healthObj != null) enemyHealth = healthObj.GetComponent<EnemyHealth>();
    }


    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            if (healthObj == null) if (enemyStatusModel.isStunnable) enemyHealth.TriggerHitstun();
        }
        if (collider.tag == "Ground") enemyStatusModel.isGrounded = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (healthObj == null) if (!enemyStatusModel.isInvincible) enemyHealth.HitDetection(collider);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Ground") enemyStatusModel.isGrounded = false;
    }
}
