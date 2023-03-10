using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClingDetect : MonoBehaviour
{
    MoveService moveService;
    InventoryModel inventoryModel;
    public int moveDireciton;

    // Start is called before the first frame update
    void Start()
    {
        moveService = gameObject.GetComponentInParent<MoveService>();
        inventoryModel = gameObject.GetComponentInParent<InventoryModel>();
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Ground" && inventoryModel.debugWallCling)
        {
            gameObject.GetComponentInParent<StatusModel>().isCling = true;
            moveService.SetClingJumpDirection(moveDireciton);
            moveService.ResetJumps();
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Ground")
        {          
            gameObject.GetComponentInParent<StatusModel>().isCling = false;
        }
    }
}
