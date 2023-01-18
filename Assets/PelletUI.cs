using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletUI : MonoBehaviour
{
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }


    // Update is called once per frame
    void Update()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = playerHealth.heals.ToString();
    }
}

