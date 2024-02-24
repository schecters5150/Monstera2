using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    List<GameObject> allSpellObjects = new List<GameObject>();
    List<SpellTypes> spellsInInventory = new List<SpellTypes>();
    List<SpellTypes> activeSpellList = new List<SpellTypes>();
    SpellTypes activeSpell;
    public GameObject activeSpellPrefab;

    InputManager inputManager;
    InventoryModel inventoryModel;

    int spellIndex;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<InputManager>();
        inventoryModel = GetComponent<InventoryModel>();
        GetSpellLists();
        activeSpell = activeSpellList.FirstOrDefault();
        SetCurrentSpellPrefab();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SwapSpell();
    }

    public void GetSpellLists()
    {
        var spellArr = Resources.LoadAll("Assets/Scripts/Player/Spells");
        foreach (var spell in spellArr)
        {
            allSpellObjects.Add(spell as GameObject);
        }

        spellsInInventory = inventoryModel.jsonModel.unlockedSpells;
        activeSpellList = inventoryModel.jsonModel.activeSpells;
    }

    public void SwapSpell()
    {

        if (inputManager.SpellSwapTriggered())
        {
            spellIndex++;
            if (spellIndex == activeSpellList.Count) spellIndex = 0;

            if (activeSpellList.Count > 0) activeSpell = activeSpellList[spellIndex];
            SetCurrentSpellPrefab();
        }
    }

    public void SetCurrentSpellPrefab()
    {
        activeSpellPrefab = allSpellObjects.Where(x => x.GetComponent<SpellName>().spellType == activeSpell).FirstOrDefault();
    }
}

public enum SpellTypes
{
    horizontalLob,
    seedDrop,
    postureBurst
}