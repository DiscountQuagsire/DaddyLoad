using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Chat;

public class CommunicationScript : MonoBehaviourPunCallbacks
{
    [PunRPC]
    public void receiveMessage(string message)
    {
        Debug.Log("received message: " + message);
        string[] segmented = message.Split('/');

        if (segmented[0] == "blockdestroy")
            this.receiveBlockDestroyInfo(segmented[1], int.Parse(segmented[2]), int.Parse(segmented[3]));

        if (segmented[0] == "setseed")
            this.receiveSeedUpdate(int.Parse(segmented[1]));

    }

    private void receiveBlockDestroyInfo(string name, int x, int y)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject.Find("FileManager").GetComponent<FileManager>().writeBlockDestroy(x, y);
        }
        GameObject.Find("MapGenerator").GetComponent<MapGeneratorScript>().removeBlockAt(x, y);
    }

    private void receiveSeedUpdate(int newSeed)
    {
        Debug.Log("Setting seed to: " + newSeed);
        GameObject.Find("MapGenerator").GetComponent<MapGeneratorScript>().seed = newSeed;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        Debug.Log("New player joined");

    }

}
