using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileManager : MonoBehaviour
{
    private ArrayList coords = new ArrayList();

    public void Start()
    {
        loadEmptyBlocks();

        for(int i = 0; i < coords.Count; i++)
        {
            Debug.Log("block destroed: " + coords[i]);
        }


    }

    public void registerBlockDestroy(int x, int y)
    {
   
        File.AppendAllText(Application.dataPath + "/GameFiles/blocks.txt", x + "/" + y + "\n");
        Debug.Log("Registered block destroy at: " + x + ", " + y);
    }

    public void loadEmptyBlocks()
    {
        coords.Clear();
        string[] input = File.ReadAllLines(Application.dataPath + "/GameFiles/blocks.txt");

        foreach (string thisLine in input)
        {
            string[] splitLine = thisLine.Split('/');
            var newTuple = (int.Parse(splitLine[0]), int.Parse(splitLine[1]));
            coords.Add(newTuple);
            Debug.Log("new tuple: " + newTuple);
        }
    }

    public bool isDestroyed(int x, int y)
    {
        var newTuple = (x, y);
        if (x < 6 && x > -6 && y < 6) Debug.Log("x " + x + " y " + y + ": " + coords.Contains(newTuple));
        
        return coords.Contains(newTuple);
    }
}
