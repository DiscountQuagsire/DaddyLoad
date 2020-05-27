using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private GameObject quickStartButton;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        quickStartButton.SetActive(true);
    }

    public void QuickStart()
    {
        quickStartButton.SetActive(false);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Quick starting");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("failed to join room; creating");
        CreateRoom();
    }

    public void CreateRoom()
    {
        int random = Random.Range(0, 10000);
        RoomOptions ro = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)25 };
        PhotonNetwork.CreateRoom("room" + random, ro);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create toom; retrying");
        CreateRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
