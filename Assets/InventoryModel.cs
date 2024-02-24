using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEditor;
using System.Linq;

public class InventoryModel : MonoBehaviour
{
    // Start is called before the first frame update
    public InventoryJsonModel jsonModel;
    public bool debug;
    public bool debugSword;
    public bool debugDoubleJump;
    public bool debugWallCling;
    public bool debugHover;
    public bool debugDeflect;
    public int debugFertilizer;
    public bool debugDodge;
    public bool debugParry;

    public int spellSlots;
    public List<SpellTypes> unlockedSpells;
    public List<SpellTypes> activeSpells;


    public void SaveFile()
    {
        string destination = Application.persistentDataPath + "/inventory.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, jsonModel);
        file.Close();
    }

    public InventoryJsonModel LoadFile()
    {


        string destination = Application.persistentDataPath + "/inventory.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            return new InventoryJsonModel();
        }

        BinaryFormatter bf = new BinaryFormatter();
        InventoryJsonModel data = (InventoryJsonModel)bf.Deserialize(file);
        file.Close();

        return data;
    }

    void Start()
    {
        if (debug)
        {
            var spellArr = AssetDatabase.LoadAllAssetsAtPath("Assets/Scripts/Player/Spells");

            foreach (var spell in spellArr)
            {
                //unlockedSpells.Add(spell as GameObject);
            }

            jsonModel = new InventoryJsonModel()
            {
                sword = debugSword,
                doubleJump = debugDoubleJump,
                wallCling = debugWallCling,
                hover = debugHover,
                dodge = debugDodge,
                deflect = debugDeflect,
                fertilizer = debugFertilizer,
                parry = debugParry,

                spellSlots = Enum.GetNames(typeof(SpellTypes)).Length,
                unlockedSpells = Enum.GetValues(typeof(SpellTypes)).Cast<SpellTypes>().ToList(),
                activeSpells = Enum.GetValues(typeof(SpellTypes)).Cast<SpellTypes>().ToList()
            };
        }
        else jsonModel = LoadFile();
    }

    // Update is called once per frame
    void Update()
    {

    }

}

[Serializable]
public class InventoryJsonModel
{
    public bool sword;
    public bool doubleJump;
    public bool wallCling;
    public bool hover;
    public bool deflect;
    public bool dodge;
    public int fertilizer;
    public bool parry;
    public List<SpellTypes> unlockedSpells;
    public List<SpellTypes> activeSpells;
    public int spellSlots;
}
