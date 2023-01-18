using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class IntersceneStatusModel : MonoBehaviour
{
    public bool debugLoading;
    public int debugCheckpointId;
    public PlayerHealth playerHealth;


    public Transform GetLoadPosition()
    {
        var isSaveLoad = ES3.Load<bool>("loadToSavePoint", true);
        if (isSaveLoad)
        {
            var saveZone = GameObject.FindGameObjectsWithTag("SaveZone").FirstOrDefault();
            ES3.Save("loadToSavePoint", false);
            return saveZone.transform;
        }
        else
        {
            var checkpoints = GameObject.FindGameObjectsWithTag("CheckPoint");
            var selected = checkpoints.Where(x => x.GetComponent<CheckpointId>().id == ES3.Load<int>("checkpointId", 0)).FirstOrDefault();
            if (selected == null) selected = checkpoints.Where(x => x.GetComponent<CheckpointId>().id == 0).FirstOrDefault();
            return selected.transform;
        }
        
    }
}


