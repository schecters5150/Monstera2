using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBar : MonoBehaviour
{
    public GameObject bar;
    public EnemyHealth health;
    public EnemyBarType barType;


    private void Start()
    {
        
    }

    public void Update()
    {
        if (barType == EnemyBarType.Health)
        {
            bar.transform.localScale = new Vector3(2.9f * health.health / health.maxHealth, bar.transform.localScale.y, bar.transform.localScale.z);
        }
        if (barType == EnemyBarType.Posture)
        {
            bar.transform.localScale = new Vector3(2.9f * health.poise / health.maxPoise, bar.transform.localScale.y, bar.transform.localScale.z);
        }
    }

}

public enum EnemyBarType
{
    Health,
    Posture
}