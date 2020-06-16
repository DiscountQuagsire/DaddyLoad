using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Chat;

public class CommunicationScript : MonoBehaviourPunCallbacks
{
    // single point of entry pro vsechnu komunikaci

    //FileManager fm;
    //MapGeneratorScript mgs;

    public void Start()
    {
        //fm = GameObject.Find("FileManager").GetComponent<FileManager>();
        //mgs = GameObject.Find("MapGenerator").GetComponent<MapGeneratorScript>();
    }

    [PunRPC]
    public void receiveMessage(string message)
    {
        Debug.Log("received message: " + message);
        string[] segmented = message.Split('/');

        if (segmented[0] == "blockdestroy")
            receiveBlockDestroyInfo(segmented[1], int.Parse(segmented[2]), int.Parse(segmented[3]));

        if (segmented[0] == "setseed")
            receiveSeedUpdate(int.Parse(segmented[1]));

        if (segmented[0] == "mapinfo")
            receiveMapInfo(segmented[1]);

        if (segmented[0] == "materialupdate")
            updateGlobalInventoryMaterial(segmented[1], int.Parse(segmented[2]));

    }

    // locally called metoda ktera updatne 1 material v mgs.globalinventory
    private void updateGlobalInventoryMaterial(string material, int amount)
    {
        GameObject.Find("MapGenerator").GetComponent<MapGeneratorScript>().globalInventory.materials[material] = amount;
    }

    // locally called metoda ktera removne block; pokud jsi master tak ho i logne
    private void receiveBlockDestroyInfo(string resourceName, int x, int y) 
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject.Find("FileManager").GetComponent<FileManager>().writeBlockDestroy(x, y);
        }
        GameObject.Find("MapGenerator").GetComponent<MapGeneratorScript>().removeBlockAt(x, y);
    }

    // locally called; updatne seed
    private void receiveSeedUpdate(int newSeed)
    {
        Debug.Log("Setting seed to: " + newSeed);
        GameObject.Find("MapGenerator").GetComponent<MapGeneratorScript>().setSeed(newSeed); 
    }

    private void receiveMapInfo(string mapInfo)
    {
        Debug.Log("receiving map info: " + mapInfo);
        GameObject.Find("FileManager").GetComponent<FileManager>().loadDestroyedBlockCoordinatesFromString(mapInfo);
       
    }


    // v tyhle metode master posle map info newly connected playerovi pri connectu
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New player joined");
        if (!PhotonNetwork.IsMasterClient) return;

        // misto newplayer bylo all
        photonView.RPC ("receiveMessage", newPlayer, "setseed/" + GameObject.Find("MapGenerator").GetComponent<MapGeneratorScript>().seed);

        string msg = GameObject.Find("FileManager").GetComponent<FileManager>().getDestroyedBlockCoordinatesInString();
        photonView.RPC("receiveMessage", newPlayer, "mapinfo/" + msg);

        foreach (KeyValuePair<string, int> pair in GameObject.Find("MapGenerator").GetComponent<MapGeneratorScript>().globalInventory.materials)
        {
            photonView.RPC("receiveMessage", newPlayer, "materialupdate/" + pair.Key + "/" + pair.Value);
        }
    }
}