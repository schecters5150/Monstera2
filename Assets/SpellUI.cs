using System.Collections;
using System.Collections.Generic;
using TheraBytes.BetterUi;
using UnityEngine;

public class SpellUI : MonoBehaviour
{
    // Start is called before the first frame update
    private SpellManager spellManager;

    private void Start()
    {
        spellManager = GameObject.FindGameObjectWithTag("Player").GetComponent<SpellManager>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject currentSpell = spellManager.activeSpellPrefab;
        if (currentSpell != null)
        {
            GetComponent<BetterImage>().sprite = currentSpell.GetComponentInChildren<BetterImage>().sprite;
        }
    }
}
