using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipUIScript : MonoBehaviour
{
    public GameObject ship;

    public GameObject cockpit;
    public Sprite cockpit0;
    public Sprite cockpit1;
    public Sprite cockpit2; 

    public GameObject reactor;
    public Sprite reactor0;
    public Sprite reactor1;
    public Sprite reactor2;
    public Sprite reactor3;
    public Sprite reactor4;

    public GameObject room1;
    public Sprite room1_0;
    public Sprite room1_1;
    public Sprite room1_2;
    public Sprite room1_3;
    public Sprite room1_4;

    public GameObject room2;
    public Sprite room2_0;
    public Sprite room2_1;
    public Sprite room2_2;
    public Sprite room2_3;
    public Sprite room2_4;

    public GameObject room3;
    public Sprite room3_0;
    public Sprite room3_1;
    public Sprite room3_2;
    public Sprite room3_3;
    public Sprite room3_4;

    public GameObject room4;
    public Sprite room4_0;
    public Sprite room4_1;
    public Sprite room4_2;
    public Sprite room4_3;
    public Sprite room4_4;

    public GameObject room5;
    public Sprite room5_0;
    public Sprite room5_1;
    public Sprite room5_2;
    public Sprite room5_3;
    public Sprite room5_4;

    public GameObject room6;
    public Sprite room6_0;
    public Sprite room6_1;
    public Sprite room6_2;
    public Sprite room6_3;
    public Sprite room6_4;

    private void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            ship.SetActive(!ship.activeSelf);
            this.reloadSprites();
        }
    }

    public void reloadSprites()
    {
        switch (ProgressionScript.getCockpit())
        {
            case 0: cockpit.GetComponent<Image>().sprite = cockpit0; break;
            case 1: cockpit.GetComponent<Image>().sprite = cockpit1; break;
            case 2: cockpit.GetComponent<Image>().sprite = cockpit2; break;
        }

        switch (ProgressionScript.getReactor())
        {
            case 0: reactor.GetComponent<Image>().sprite = reactor0; break;
            case 1: reactor.GetComponent<Image>().sprite = reactor1; break;
            case 2: reactor.GetComponent<Image>().sprite = reactor2; break;
            case 3: reactor.GetComponent<Image>().sprite = reactor3; break;
            case 4: reactor.GetComponent<Image>().sprite = reactor4; break;
        }

        switch (ProgressionScript.getRoom1())
        {
            case 0: room1.GetComponent<Image>().sprite = room1_0; break;
            case 1: room1.GetComponent<Image>().sprite = room1_1; break;
            case 2: room1.GetComponent<Image>().sprite = room1_2; break;
            case 3: room1.GetComponent<Image>().sprite = room1_3; break;
            case 4: room1.GetComponent<Image>().sprite = room1_4; break;
        }

        switch (ProgressionScript.getRoom2())
        {
            case 0: room2.GetComponent<Image>().sprite = room2_0; break;
            case 1: room2.GetComponent<Image>().sprite = room2_1; break;
            case 2: room2.GetComponent<Image>().sprite = room2_2; break;
            case 3: room2.GetComponent<Image>().sprite = room2_3; break;
            case 4: room2.GetComponent<Image>().sprite = room2_4; break;
        }

        switch (ProgressionScript.getRoom3())
        {
            case 0: room3.GetComponent<Image>().sprite = room3_0; break;
            case 1: room3.GetComponent<Image>().sprite = room3_1; break;
            case 2: room3.GetComponent<Image>().sprite = room3_2; break;
            case 3: room3.GetComponent<Image>().sprite = room3_3; break;
            case 4: room3.GetComponent<Image>().sprite = room3_4; break;
        }

        switch (ProgressionScript.getRoom4())
        {
            case 0: room4.GetComponent<Image>().sprite = room4_0; break;
            case 1: room4.GetComponent<Image>().sprite = room4_1; break;
            case 2: room4.GetComponent<Image>().sprite = room4_2; break;
            case 3: room4.GetComponent<Image>().sprite = room4_3; break;
            case 4: room4.GetComponent<Image>().sprite = room4_4; break;
        }

        switch (ProgressionScript.getRoom5())
        {
            case 0: room5.GetComponent<Image>().sprite = room5_0; break;
            case 1: room5.GetComponent<Image>().sprite = room5_1; break;
            case 2: room5.GetComponent<Image>().sprite = room5_2; break;
            case 3: room5.GetComponent<Image>().sprite = room5_3; break;
            case 4: room5.GetComponent<Image>().sprite = room5_4; break;
        }

        switch (ProgressionScript.getRoom6())
        {
            case 0: room6.GetComponent<Image>().sprite = room6_0; break;
            case 1: room6.GetComponent<Image>().sprite = room6_1; break;
            case 2: room6.GetComponent<Image>().sprite = room6_2; break;
            case 3: room6.GetComponent<Image>().sprite = room6_3; break;
            case 4: room6.GetComponent<Image>().sprite = room6_4; break;
        }

    }



}
