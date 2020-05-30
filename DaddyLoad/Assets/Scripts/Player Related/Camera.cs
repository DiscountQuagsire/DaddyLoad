using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Camera : MonoBehaviourPunCallbacks
{
    public GameObject playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            playerCamera.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
