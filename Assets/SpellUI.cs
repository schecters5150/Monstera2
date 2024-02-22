using System.Collections;
using System.Collections.Generic;
using TheraBytes.BetterUi;
using UnityEngine;

public class SpellUI : MonoBehaviour
{
    // Start is called before the first frame update
    private AttackService attackService;

    private void Start()
    {
        attackService = GameObject.FindGameObjectWithTag("Player").GetComponent<AttackService>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject currentSpell = attackService.CurrentSpellPrefab;
        GetComponent<BetterImage>().sprite = currentSpell.GetComponentInChildren<BetterImage>().sprite;
    }
}
