using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformToPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Platform")
        {
            transform.parent.transform.SetParent(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Platform")
        {
            transform.parent.transform.SetParent(null);
        }
    }
}