using Photon.Pun;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class GameSetupScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate("PlayerPrefab", new Vector3(Random.Range(0, 3), 1, 0), Quaternion.identity);
        Debug.Log("Player Created");
    }

}
