using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationScript : MonoBehaviour
{
    public void receiveMessage(string message)
    {
        Debug.Log("received message: " + message);
        string[] segmented = message.Split('/');

        foreach (string s in segmented)
        {
            Debug.Log("segment: " + s);
        }

        if (segmented[0] == "blockdestroy")
        {
            this.receiveBlockDestroyInfo(segmented[1], int.Parse(segmented[2]), int.Parse(segmented[3]));
        }
    }

    private void receiveBlockDestroyInfo(string name, int x, int y)
    {
        Debug.Log("recieved blockdestroy info in cm");
        GameObject.Find("FileManager").GetComponent<FileManager>().writeBlockDestroy(x, y);
        GameObject.Find("MapGenerator").GetComponent<MapGeneratorScript>().removeBlockAt(x, y);
    }

}
