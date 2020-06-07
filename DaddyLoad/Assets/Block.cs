﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Block : MonoBehaviourPunCallbacks
{
    public float maxHealth;
    private float currentHealth;
    private GameObject lastPlayerWhoDamaged;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void takeDamage(float damage, GameObject p)
    {
        lastPlayerWhoDamaged = p;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            myVeryOwnOnDestroy();
        }
    }

    private void myVeryOwnOnDestroy()
    {
        if (lastPlayerWhoDamaged == null) return; //tohle nemuze nastat
        lastPlayerWhoDamaged.GetComponent<CommunicationScript>().photonView.RPC
        ("receiveMessage", RpcTarget.All, "blockdestroy/name/" + (int)transform.position.x + "/" + (int)transform.position.y);
    }

}
