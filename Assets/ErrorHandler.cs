using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ErrorHandler : MonoBehaviour
{
    private string path;

    // Start is called before the first frame update
    void Start()
    {
        path = Application.dataPath + "/ErrorHandler.txt";

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void WriteToLog(string msg)
    {
        // This text is added only once to the file.
        if (!File.Exists(path))
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine("Rachel fucked up somewhere lmao");
                sw.WriteLine("The question is where");
                sw.WriteLine("Oh the places I will go\n\n");
            }
        }

        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine(msg);
        }
    }
}
