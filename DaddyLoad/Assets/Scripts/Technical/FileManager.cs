﻿using System.Collections;
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
            mgs.inventory.materials = this.getDictionaryFromGlobalInventory();
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

    public void writeDownInventory() 
    {
        foreach (KeyValuePair<string, int> pair in mgs.inventory.materials)
        {
            File.AppendAllText(Application.dataPath + "/GameFiles/globalinventory.txt", pair.Key + "/" + pair.Value + "\n");
        }
    }
    
    public void updateInventoryForEveryone()
    {
        CommunicationScript cs = GameObject.FindGameObjectWithTag("Player").GetComponent<CommunicationScript>();
        Dictionary<string, int> materials = mgs.inventory.materials;

        foreach (KeyValuePair<string, int> pair in materials)
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

    public void loadShipUpgradesFromFile()
    {
        string[] input = File.ReadAllLines(Application.dataPath + "/GameFiles/shipinfo.txt");
        this.loadShipUpgradesFromString(input[0]);
    }

    public void loadShipUpgradesFromString(string input)
    {
        // thrusters/temp shields/pres shields/bodywork/reactor/comm room/circuitry/windows/flaps
        // 0    1    2            3            4        5       6         7         8       9
        string[] segmented = input.Split('/');
        ProgressionScript ps = GameObject.FindGameObjectWithTag("Player").GetComponent<ProgressionScript>();

        ps.setThrusters(int.Parse(segmented[0]));
        ps.setTempShields(int.Parse(segmented[1]));
        ps.setPresShields(int.Parse(segmented[2]));
        ps.setBodywork(int.Parse(segmented[3]));
        ps.setReactor(int.Parse(segmented[4]));
        ps.setCommRoom(int.Parse(segmented[5]) == 1 ? true : false);
        ps.setCircuitry(int.Parse(segmented[6]) == 1 ? true : false);
        ps.setWindows(int.Parse(segmented[7]) == 1 ? true : false);
        ps.setFlaps(int.Parse(segmented[8]) == 1 ? true : false);

        if (PhotonNetwork.IsMasterClient)
        {
            this.writeShipUpgradesToFile();
        }
    }

    public string getShipUpgradesString()
    {
        return File.ReadAllLines(Application.dataPath + "/GameFiles/shipinfo.txt")[0];
    }

    public void writeShipUpgradesToFile()
    {
        Debug.Log("writing ship upgrades to file");
        ProgressionScript ps = GameObject.FindGameObjectWithTag("Player").GetComponent<ProgressionScript>();
        string output = "";

        output += ps.getThrusterLevel()+"/";
        output += ps.getTemperatureShieldsLevel() + "/";
        output += ps.getPressureShieldsLevel() + "/";
        output += ps.getBodyworkLevel() + "/";
        output += ps.getReactorLevel() + "/";
         
        output += ps.getCommRoom() ? 1 + "/" : 0 +"/" ;
        output += ps.getCircuitry() ? 1 + "/" : 0 + "/";
        output += ps.getWindows() ? 1 + "/" : 0 + "/";
        output += ps.getFlaps() ? 1 : 0;

        File.WriteAllText(Application.dataPath + "/GameFiles/shipinfo.txt", output);
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
