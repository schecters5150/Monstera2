using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauntlet : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;

    // Update is called once per frame
    void Update()
    {
        if (target == null) Destroy(gameObject);
    }
}
