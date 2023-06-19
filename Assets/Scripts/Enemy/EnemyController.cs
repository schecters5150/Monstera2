using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Enemy;


public class EnemyController : MonoBehaviour
{
    public bool isStunnable;
    public int damageDealt;
    public GameObject healthObj;

    private EnemyStatusModel enemyStatusModel;
    private EnemyHealth enemyHealth;




    void Start()
    {
        enemyStatusModel = GetComponentInParent<EnemyStatusModel>();
        enemyHealth = healthObj.GetComponent<EnemyHealth>();

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
        if (!enemyStatusModel.isInvincible) enemyHealth.HitDetection(collider);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Ground") enemyStatusModel.isGrounded = false;
    }
}
