using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnZeroChildren : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nextWave;

    // Update is called once per frame
    void Update()
    {
        if(transform.childCount == 0)
        {
            if(nextWave != null) nextWave.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
