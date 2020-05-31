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
    }

    public void writeBlockDestroy(int x, int y)
    {
        File.AppendAllText(Application.dataPath + "/GameFiles/blocks.txt", x + "/" + -y + "\n");
        //Debug.Log("writing block destroy at: " + x + ", " + -y);
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
            //Debug.Log("new tuple: " + newTuple);
        }
    }

    public bool isDestroyed(int x, int y)
    {
        var newTuple = (x, y);
        return coords.Contains(newTuple);
    }
}
