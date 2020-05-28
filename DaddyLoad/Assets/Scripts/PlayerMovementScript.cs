using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviourPunCallbacks
{
    // Update is called once per frame

    private void Start()
    {
        if (!photonView.IsMine) return;
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        if (Input.GetKey("w")) transform.position += new Vector3(0, 0.1f, 0);
        if (Input.GetKey("a")) transform.position += new Vector3(-0.1f, 0, 0);
        if (Input.GetKey("s")) transform.position += new Vector3(0, -0.1f, 0);
        if (Input.GetKey("d")) transform.position += new Vector3(0.1f, 0, 0);
    }
}
