using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    MoveService moveService;
    AttackService attackService;

    // Start is called before the first frame update
    void Start()
    {
        moveService = gameObject.GetComponentInParent<MoveService>();
        attackService = gameObject.GetComponentInParent<AttackService>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            if (attackService.attackDownFlag)
            {
                moveService.Bounce();
                moveService.ResetJumps();
            }
            attackService.ResetDownFlag();
        }
    }
}
