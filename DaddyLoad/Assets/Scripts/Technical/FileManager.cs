using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileManager : MonoBehaviour
{

    void CreateText()
    {
        string path = Application.dataPath + "/Log.txt";
        Debug.Log("path: " + path);
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "This is the title yo\n\n");
        }

        string content = "Log: " + System.DateTime.Now + "\n";

        File.AppendAllText(path, content);

        

    }

    private void Start()
    {
        string path = Application.dataPath + "/Log.txt";
        CreateText();

        string [] lines = File.ReadAllLines(path);

        foreach (string s in lines)
        {
            Debug.Log("hej " + s);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
