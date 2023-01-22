using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GauntletDoorTrigger : MonoBehaviour
{
    public GameObject wave1;
    public GameObject triggerDoor;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            wave1.SetActive(true);
            triggerDoor.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
