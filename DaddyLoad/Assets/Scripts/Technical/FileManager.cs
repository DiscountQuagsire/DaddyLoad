using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime;

public class FileManager : MonoBehaviour
{
    public ArrayList destroyedBlockCoords = new ArrayList();

    public void Start()
    {
        reloadEmptyBlocks();
        //getDestroyedBlockCoordinatesInString();
    }

    public void writeBlockDestroy(int x, int y)
    {
        File.AppendAllText(Application.dataPath + "/GameFiles/blocks.txt", x + "," + y + "\n");
        destroyedBlockCoords.Add(new Coordinate(x, y));
        Debug.Log("writing block destroy at: " + x + ", " + y);
    }

    public void reloadEmptyBlocks()
    {
        destroyedBlockCoords.Clear();
        string[] input = File.ReadAllLines(Application.dataPath + "/GameFiles/blocks.txt");

        foreach (string thisLine in input)
        {

            Coordinate newCoord = new Coordinate(thisLine);
            destroyedBlockCoords.Add(newCoord);
        }
    }

    public bool isDestroyed(int x, int y)
    {
        foreach(Coordinate c in destroyedBlockCoords)
        {
            if (c.x == x && c.y == y) return true;
        }
        return false;
    }

    public string getDestroyedBlockCoordinatesInString()
    {
        string output = "";
        foreach (Coordinate c in destroyedBlockCoords)
        {
            output += c.x + "," + c.y + "*";
        }

        return output;
    }

    public void loadDestroyedBlockCoordinatesFromString(string input)
    {
        string[] lines = input.Split('*');

        foreach (string thisLine in lines)
        {
            Coordinate newCoord = new Coordinate(thisLine);
            destroyedBlockCoords.Add(newCoord);
        }
    }
}

public class Coordinate
{
    public int x;
    public int y;

    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Coordinate(string s)
    {
        string[] segmented = s.Split(',');
        this.x = int.Parse(segmented[0]);
        this.y = int.Parse(segmented[1]);
    }
}
