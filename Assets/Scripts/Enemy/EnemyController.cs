using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Enemy;


public class EnemyController : MonoBehaviour
{
    public bool isStunnable;
    public int damageDealt;

    private EnemyStatusModel enemyStatusModel;
    private EnemyHealth enemyHealth;




    void Start()
    {
        enemyStatusModel = GetComponent<EnemyStatusModel>();
        enemyHealth = GetComponent<EnemyHealth>();

        enemyStatusModel.isStunnable = isStunnable;
    }


    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            if (isStunnable) enemyHealth.TriggerHitstun();
        }
        if (collider.tag == "Ground") enemyStatusModel.isGrounded = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Hitbox")
        {
            if (!enemyStatusModel.isInvincible)
            {
                enemyHealth.ReduceHealth(5); // TODO 
                enemyHealth.TriggerInvincibility();              
                if (isStunnable) enemyHealth.TriggerHitstun();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Ground") enemyStatusModel.isGrounded = false;
    }
}
