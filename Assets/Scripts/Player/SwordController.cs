using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    MoveService moveService;
    AttackService attackService;
    PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        moveService = gameObject.GetComponentInParent<MoveService>();
        attackService = gameObject.GetComponentInParent<AttackService>();
        playerHealth = gameObject.GetComponentInParent<PlayerHealth>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy" || collider.tag == "BouncePad")
        {
            if (attackService.attackDownFlag)
            {
                moveService.Bounce();
                moveService.ResetJumps();
                playerHealth.AddStamina(playerHealth.maxStamina / 4);
            }
        }
    }
}
