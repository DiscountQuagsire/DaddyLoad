using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime;
using Photon.Pun;
using System;

public class FileManager : MonoBehaviour
{
    public ArrayList destroyedBlockCoords = new ArrayList();
    public ArrayList unwrittenBlockCoords = new ArrayList();
    public MapGeneratorScript mgs;

    public void Start()
    {
        mgs = GameObject.Find("MapGenerator").GetComponent<MapGeneratorScript>();
        if (PhotonNetwork.IsMasterClient)
        {
            reloadEmptyBlocksFromOwnFiles();
            mgs.globalInventory.materials = this.getDictionaryFromGlobalInventory();
        }
    }



    public void writeBlockDestroy(int x, int y)
    {
        //File.AppendAllText(Application.dataPath + "/GameFiles/blocks.txt", x + ", " + y + "\n");
        destroyedBlockCoords.Add(new Coordinate(x, y));
        unwrittenBlockCoords.Add(new Coordinate(x, y));
        //Debug.Log("writing block destroy at: " + x + ", " + y);
    }

    public void writeUnwrittenBlocksToFile()
    {
        Debug.Log("Saving map");
        foreach (Coordinate c in unwrittenBlockCoords)
        {
            File.AppendAllText(Application.dataPath + "/GameFiles/blocks.txt", c.x + ", " + c.y + "\n");
        }
        unwrittenBlockCoords.Clear();
    }

    public void reloadEmptyBlocksFromOwnFiles()
    {
        destroyedBlockCoords.Clear();
        string[] input = File.ReadAllLines(Application.dataPath + "/GameFiles/blocks.txt");

        foreach (string thisLine in input)
        {
            destroyedBlockCoords.Add(new Coordinate(thisLine));
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

    public void writeDownGlobalInventory()
    {
        
        foreach (KeyValuePair<string, int> pair in mgs.globalInventory.materials)
        {
            File.AppendAllText(Application.dataPath + "/GameFiles/globalinventory.txt", pair.Key + "/" + pair.Value + "\n");
        }
    }

    public void moveLocalInventoryToGlobalInventory()
    {
        Dictionary<string, int> localMaterials = mgs.localInventory.materials;
        Dictionary<string, int> globalMaterials = mgs.globalInventory.materials;

        File.WriteAllText(Application.dataPath + "/GameFiles/globalinventory.txt", String.Empty);

        foreach (KeyValuePair<string, int> pair in localMaterials)
        {
            if (globalMaterials.ContainsKey(pair.Key))

                globalMaterials[pair.Key] = globalMaterials[pair.Key] + pair.Value;

            else

                globalMaterials.Add(pair.Key, pair.Value);

        }

        mgs.globalInventory.materials = globalMaterials;
        Debug.Log("New global inventory on next line: ");
        mgs.globalInventory.listInventory();
        localMaterials.Clear();
    }

    public void updateGlobalInventoryForEveryone()
    {
        CommunicationScript cs = GameObject.FindGameObjectWithTag("Player").GetComponent<CommunicationScript>();
        Dictionary<string, int> globalMaterials = mgs.globalInventory.materials;

        foreach (KeyValuePair<string, int> pair in globalMaterials)
        {
            cs.photonView.RPC("receiveMessage", RpcTarget.Others, "materialupdate/" + pair.Key + "/" + pair.Value);
        }
        cs.photonView.RPC("receiveMessage", RpcTarget.All, "mastersaveinv");
    }

    public Dictionary<string, int> getDictionaryFromGlobalInventory()
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

    public void loadDestroyedBlockCoordinatesFromString(string input)
    {
        string[] lines = input.Split('*');

        foreach (string thisLine in lines)
        {
            if (thisLine.Length == 0) continue;
            Coordinate newCoord = new Coordinate(thisLine);
            destroyedBlockCoords.Add(newCoord);
        }
    }

    public void loadPlayerUpgradesFromFile(string name)
    {
        // name/drill/hpressure/lpressure/htemp/ltemp/rad/fuel/cargo
        // 0    1     2         3         4     5     6   7    8

        string[] input = File.ReadAllLines(Application.dataPath + "/GameFiles/playerinfo.txt");
        foreach (string line in input)
        {
            string[] segmented = line.Split('/');
            if (segmented[0] != name) continue;

            int drill = int.Parse(segmented[1]);
            int hPressure = int.Parse(segmented[2]);
            int lPressure = int.Parse(segmented[3]);
            int hTemp = int.Parse(segmented[4]);
            int lTemp = int.Parse(segmented[5]);
            int rad = int.Parse(segmented[6]);
            int fuel = int.Parse(segmented[7]);
            int cargo = int.Parse(segmented[8]);

            return; // o p t i m i y e d

        }
    }

}

public class Inventory
{
    
    public Dictionary<string, int> materials = new Dictionary<string, int>();
    
    public void addMaterial(string material)
    {
        if (materials.ContainsKey(material))
        {
            materials[material] = materials[material] + 1;
        }
        else
        {
            materials.Add(material, 1);
        }

    }

    public void listInventory()
    {
        string output = "Inventory: ";
        foreach (KeyValuePair<string, int> pair in materials)
        {
            output += pair.Key + ": " + pair.Value + "  //  ";
        }
        Debug.Log(output);
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
