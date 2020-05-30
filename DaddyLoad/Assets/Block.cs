using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Block : MonoBehaviourPunCallbacks
{
    public float maxHealth;
    private float health;
    private string playerID;

    // Start is called before the first frame update

    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {

            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        //Give drops to player with playerID
        photonView.RPC("sendMessage", RpcTarget.All);
    }

    public void takeDamage(float damage, string ID)
    {
        playerID = ID;
        health -= damage;
    }

    [PunRPC]
    void sendMessage()
    {
        Debug.Log(gameObject.name + " destroyed at" + transform.position + " by " + playerID);
    }
}
