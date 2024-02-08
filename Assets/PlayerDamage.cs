using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public float baseHealthDamage;
    public float healthDamage;
    public float basePoiseDamage;
    public float poiseDamage;

    private float healthModifier;
    private float poiseModifier;
    // Start is called before the first frame update
    void Start()
    {
        healthDamage = baseHealthDamage * (1 + healthModifier);
        poiseDamage = basePoiseDamage * (1 + poiseModifier);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
