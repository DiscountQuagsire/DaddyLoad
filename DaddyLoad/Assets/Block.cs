using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Block : MonoBehaviourPunCallbacks
{
    public float maxHealth;
    private float health;
    private GameObject player;

    void Start()
    {
        health = maxHealth;
    }


    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        //Give drops to player
        if (player != null)
        {
            player.GetComponent<CommunicationScript>().photonView.RPC
            ("receiveMessage", RpcTarget.All, "blockdestroy/name/" + (int)transform.position.x + "/" +(int)transform.position.y);
        }
    }

    public void takeDamage(float damage, GameObject p)
    {
        player = p;
        health -= damage;
    }

}
