using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Chat;

public class CommunicationScript : MonoBehaviourPunCallbacks
{
    // single point of entry pro vsechnu komunikaci
    [PunRPC]
    public void receiveMessage(string message) 
    {
        Debug.Log("received message: " + message);
        string[] segmented = message.Split('/');

        if (segmented[0] == "blockdestroy")
            this.receiveBlockDestroyInfo(segmented[1], int.Parse(segmented[2]), int.Parse(segmented[3]));

        if (segmented[0] == "setseed")
            this.receiveSeedUpdate(int.Parse(segmented[1]));

        if (segmented[0] == "mapinfo")
            this.receiveMapInfoAndLoadMap(segmented[1]);

    }

    // locally called metoda ktera removne block; pokud jsi master tak ho i logne
    private void receiveBlockDestroyInfo(string name, int x, int y) 
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
        GameObject.Find("MapGenerator").GetComponent<MapGeneratorScript>().seed = newSeed;
    }

    private void receiveMapInfoAndLoadMap(string mapInfo)
    {
        Debug.Log("receiving map info: " + mapInfo);
        GameObject.Find("FileManager").GetComponent<FileManager>().loadDestroyedBlockCoordinatesFromString(mapInfo);
        GameObject.Find("MapGenerator").GetComponent<MapGeneratorScript>().generateMap(-50, 100, -25, true);
    }


    // v tyhle metode master posle map info newly connected playerovi pri connectu
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New player joined");
        if (!PhotonNetwork.IsMasterClient) return;
        MapGeneratorScript mgs = GameObject.Find("MapGenerator").GetComponent<MapGeneratorScript>();

        // misto newplayer bylo all
        photonView.RPC ("receiveMessage", newPlayer, "setseed/" + mgs.seed);

        string msg = GameObject.Find("FileManager").GetComponent<FileManager>().getDestroyedBlockCoordinatesInString();
        photonView.RPC("receiveMessage", newPlayer, "mapinfo/" + msg);
    }

    public override void OnJoinedRoom()
    {
       


    }

}
