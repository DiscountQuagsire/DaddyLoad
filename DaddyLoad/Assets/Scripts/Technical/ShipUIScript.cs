using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipUIScript : MonoBehaviour
{

    public GameObject cockpit;
    public Sprite cockpit0;
    public Sprite cockpit1;
    public Sprite cockpit2;
    public Sprite cockpit3;
    public Sprite cockpit4;

    public GameObject reactor;
    public Sprite reactor0;
    public Sprite reactor1;
    public Sprite reactor2;
    public Sprite reactor3;
    public Sprite reactor4;

    public GameObject room1;
    public Sprite room1_0;
    public Sprite room1_1;

    public GameObject room2;
    public Sprite room2_0;
    public Sprite room2_1;

    public GameObject room3;
    public Sprite room3_0;
    public Sprite room3_1;

    public GameObject room4;
    public Sprite room4_0;
    public Sprite room4_1;

    public GameObject room5;
    public Sprite room5_0;
    public Sprite room5_1;

    public GameObject room6;
    public Sprite room6_0;
    public Sprite room6_1;

    private void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            Debug.Log("updating imagew");
            reloadSprites();
        }
        if (Input.GetKeyDown("o"))
        {
            Debug.Log("upgrading r");
            ProgressionScript.upgradeReactor();
        }
    }

    public void reloadSprites()
    {
       switch(ProgressionScript.getReactor())
        {
            case 0: cockpit.GetComponent<Image>().sprite = reactor0; break;
            case 1: reactor.GetComponent<Image>().sprite = reactor0; break;
            case 2: reactor.GetComponent<Image>().sprite = reactor0; break;
            case 3: reactor.GetComponent<Image>().sprite = reactor0; break;
            case 4: reactor.GetComponent<Image>().sprite = reactor0; break;
            case 5: reactor.GetComponent<Image>().sprite = reactor0; break;
        }
    }



}
