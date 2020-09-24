using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime;
using Photon.Pun;
using System;

public class FileManager
{
    public static ArrayList destroyedBlockCoords = new ArrayList();
    public static ArrayList unwrittenBlockCoords = new ArrayList();
    public static BaseScript bs;

    public static void Start()
    {
        bs = GameObject.Find("BaseManager").GetComponent<BaseScript>();
        if (PhotonNetwork.IsMasterClient)
        {
            reloadEmptyBlocksFromOwnFiles();
            MapGeneratorScript.inventory.materials = getDictionaryFromGlobalInventory();
        }


    }
    
    public static void reloadEmptyBlocksFromOwnFiles()
    {
        destroyedBlockCoords.Clear();
        string[] input = File.ReadAllLines(Application.dataPath + "/GameFiles/blocks.txt");

        foreach (string thisLine in input)
        {
            destroyedBlockCoords.Add(new Coordinate(thisLine));
        }
    }

    public static bool isDestroyed(int x, int y)
    {
        foreach(Coordinate c in destroyedBlockCoords)
        {
            if (c.x == x && c.y == y) return true;
        }
        return false;
    }

    public static string getDestroyedBlockCoordinatesInString()
    {
        string output = "";
        foreach (Coordinate c in destroyedBlockCoords)
        {
            output += c.x + "," + c.y + "*";
        }

        return output;
    }


    public static void updateInventoryForEveryone()
    {
        CommunicationScript cs = GameObject.FindGameObjectWithTag("Player").GetComponent<CommunicationScript>();
        Dictionary<string, int> materials = MapGeneratorScript.inventory.materials;

        foreach (KeyValuePair<string, int> pair in materials)
        {
            cs.photonView.RPC("receiveMessage", RpcTarget.Others, "materialupdate/" + pair.Key + "/" + pair.Value);
        }
        cs.photonView.RPC("receiveMessage", RpcTarget.All, "mastersaveinv");
    }

    public static Dictionary<string, int> getDictionaryFromGlobalInventory()
    {
        string[] input = File.ReadAllLines(Application.dataPath + "/GameFiles/globalinventory.txt");
        Dictionary<string, int> globalInventory = new Dictionary<string, int>();

        foreach (string line in input)
        {
            string[] segmented = line.Split('/');
            globalInventory.Add(segmented[0], int.Parse(segmented[1]));
        }

        return globalInventory;
    }

    public static void loadDestroyedBlockCoordinatesFromString(string input)
    {
        string[] lines = input.Split('*');

        foreach (string thisLine in lines)
        {
            if (thisLine.Length == 0) continue;
            Coordinate newCoord = new Coordinate(thisLine);
            destroyedBlockCoords.Add(newCoord);
        }
    }

    public static void loadShipUpgradesFromFile()
    {
        string[] input = File.ReadAllLines(Application.dataPath + "/GameFiles/shipinfo.txt");
        ProgressionScript.loadShipUpgradesFromString(input[0]);
    }

  
    public static string getShipUpgradesString()
    {
        return File.ReadAllLines(Application.dataPath + "/GameFiles/shipinfo.txt")[0];
    }

    public static ArrayList getBasesFromFile()
    {
        ArrayList list = new ArrayList();
        Debug.Log("load bases from file fired");
        string[] input = File.ReadAllLines(Application.dataPath + "/GameFiles/bases.txt");

        foreach (string thisLine in input)
        {
            list.Add(float.Parse(thisLine));
        }

        return list;
    }

    public static void writeDownInventory() //tohle urcite
    {
        Debug.Log("Writing inventory to file");
        File.WriteAllText(Application.dataPath + "/GameFiles/globalinventory.txt", "");
        foreach (KeyValuePair<string, int> pair in MapGeneratorScript.inventory.materials)
        {
            File.AppendAllText(Application.dataPath + "/GameFiles/globalinventory.txt", pair.Key + "/" + pair.Value + "\n");
        }
    }
    public static void writeUnwrittenBlocksToFile() // tohle urcite
    {
        Debug.Log("Writing newly destroyed blocks to file");
        foreach (Coordinate c in unwrittenBlockCoords)
        {
            File.AppendAllText(Application.dataPath + "/GameFiles/blocks.txt", c.x + ", " + c.y + "\n");
        }
        unwrittenBlockCoords.Clear();
    }

    public static void writeShipUpgradesToFile() //tohle urcite
    {
        Debug.Log("Writing ship upgrades to file");
        string output = ProgressionScript.getShipUpgradesString();
        File.WriteAllText(Application.dataPath + "/GameFiles/shipinfo.txt", output);
    }

    public static void writeBasesToFile()
    {
        Debug.Log("Writing bases to file");
        File.WriteAllText(Application.dataPath + "/GameFiles/bases.txt", "");
        foreach(float xPos in bs.baseCoordinates)
        {
            File.AppendAllText(Application.dataPath + "/GameFiles/bases.txt", xPos + "\n");
        }
    }

}

public class Inventory
{
    
    public Dictionary<string, int> materials = new Dictionary<string, int>();
    
    public void addMaterial(string material, int amount)
    {
        if (materials.ContainsKey(material))
        {
            materials[material] = materials[material] + amount;
        }
        else
        {
            materials.Add(material, amount);
        }

    }

    public string listInventory()
    {
        string output = "";
        foreach (KeyValuePair<string, int> pair in materials)
        {
            output += pair.Value + " " + pair.Key + ", ";
        }
        if (output.Length < 2) return "";
        return output.Substring(0, output.Length - 2);
    }

    public bool containsInventory(Inventory comparison)
    {

        foreach (KeyValuePair<string, int> pair in comparison.materials)
        {
            if (!materials.ContainsKey(pair.Key)) return false;
            if (materials[pair.Key] < pair.Value) return false;
        }

        return true;
    }

    public void removeInventoryContents(Inventory toRemove)
    {
        foreach (KeyValuePair<string, int> pair in toRemove.materials)
        {
            if (!materials.ContainsKey(pair.Key)) //cannot happen
            {
                Debug.Log("Error in remove inventory contents");
                return;
            }
            materials[pair.Key] = materials[pair.Key] - pair.Value;
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
        x = int.Parse(segmented[0]);
        y = int.Parse(segmented[1]);
    }

    public bool isInArray(ArrayList haystack)
    {
        foreach (Coordinate currentCoordinate in haystack)
            if (currentCoordinate.x == x && currentCoordinate.y == y) return true;
        return false;
    }

    public bool isWithinSight(Vector3 whichPointWeMeasureFrom, float sightHalfWidth, float sightHalfHeight, int chunkSize)
    {

        if (x < whichPointWeMeasureFrom.x - sightHalfWidth - chunkSize)     return false;
        if (x > whichPointWeMeasureFrom.x + sightHalfWidth)                 return false;
        if (y < whichPointWeMeasureFrom.y - sightHalfHeight)                return false;
        if (y > whichPointWeMeasureFrom.y + sightHalfHeight + chunkSize)    return false;

        return true;
    }
}
