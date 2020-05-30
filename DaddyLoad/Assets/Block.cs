using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public float maxHealth;
    private float health;
    public GameObject deathEffect;
    private string playerID;

    // Start is called before the first frame update

    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health >= 0)
        {

            Destroy(this);
        }
    }

    private void OnDestroy()
    {            
        //Give drops to player with playerID
        Debug.Log(gameObject.name + " destroyed at" + transform.position);
    }

    public void takeDamage(float damage, string ID)
    {
        playerID = ID;
        health -= damage;
    }
}
