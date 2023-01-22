using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

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
    public bool debugFire;
    public int debugFertilizer;
    public bool debugDodge;


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
            jsonModel = new InventoryJsonModel()
            {
                sword = debugSword,
                doubleJump = debugDoubleJump,
                wallCling = debugWallCling,
                hover = debugHover,
                dodge = debugDodge,
                deflect = debugDeflect,
                fire = debugFire,
                fertilizer = debugFertilizer
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
    public bool fire;
    public bool dodge;
    public int fertilizer;
}
