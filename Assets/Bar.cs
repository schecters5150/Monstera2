using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public GameObject bar;
    public PlayerHealth playerHealth;
    public BarType barType;

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    public void Update()
    {
        if (barType == BarType.Health)
        {
            bar.transform.localScale = new Vector3((float)playerHealth.health / (float)playerHealth.maxHealth, 1, 1);
        }
        if (barType == BarType.Stamina)
        {
            bar.transform.localScale = new Vector3(playerHealth.stamina / playerHealth.maxStamina, 1, 1);
        }
    }

}

public enum BarType
{
    Health,
    Stamina
}