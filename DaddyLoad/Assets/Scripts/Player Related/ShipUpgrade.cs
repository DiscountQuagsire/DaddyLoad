using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class ShipUpgrade : MonoBehaviour
{

    public GameObject reactor;
    public GameObject cockpit;
    public GameObject room1;
    public GameObject room2;
    public GameObject room3;
    public GameObject room4;
    public GameObject room5;
    public GameObject room6;

    public ArrayList dialogues = new ArrayList();

    public void Start()
    {
        dialogues.Add(reactor);
        dialogues.Add(cockpit);
        dialogues.Add(room1);
        dialogues.Add(room2);
        dialogues.Add(room3);
        dialogues.Add(room4);
        dialogues.Add(room5);
        dialogues.Add(room6);
    }

    public void hideAllDialogues()
    {
        for (int i=0; i<dialogues.Count; i++)
        {
            ((GameObject)dialogues[i]).transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void showDialogue(int index)
    {
        Debug.Log("fired at " + index);
        hideAllDialogues();

        GameObject panel = ((GameObject)dialogues[index]).transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        ((GameObject)dialogues[index]).transform.GetChild(0).gameObject.SetActive(true);
        TextMeshProUGUI canPurchase = panel.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        Inventory costInv = null;
        Button b = panel.transform.GetChild(3).GetComponent<Button>();
        int i = 0;
        bool maxLevel = false;
        switch (index)
        {
            case 0:
                i = ProgressionScript.getReactor();
                costInv = CostScript.getCost("reactor", i + 1);
                if (i >= 4) maxLevel = true;
                break;
            case 1:
                i = ProgressionScript.getCockpit();
                costInv = CostScript.getCost("cockpit", i + 1);
                if (i >= 2) maxLevel = true;
                break;
            case 2:
                i = ProgressionScript.getRoom1();
                costInv = CostScript.getCost("room1", i + 1);
                if (i >= 4) maxLevel = true;
                break;
            case 3:
                i = ProgressionScript.getRoom2();
                costInv = CostScript.getCost("room2", i + 1);
                if (i >= 4) maxLevel = true;
                break;
            case 4:
                i = ProgressionScript.getRoom3();
                costInv = CostScript.getCost("room3", i + 1);
                if (i >= 4) maxLevel = true;
                break;
            case 5:
                i = ProgressionScript.getRoom4();
                costInv = CostScript.getCost("room4", i + 1);
                if (i >= 4) maxLevel = true;
                break;
            case 6:
                i = ProgressionScript.getRoom5();
                costInv = CostScript.getCost("room5", i + 1);
                if (i >= 4) maxLevel = true;
                break;
            case 7:
                i = ProgressionScript.getRoom6();
                costInv = CostScript.getCost("room6", i + 1);
                if (i >= 4) maxLevel = true;
                break;

        }

        if (MapGeneratorScript.inventory.containsInventory(costInv))
        {
            canPurchase.text = "You can purchase this upgrade";
            b.interactable = true;
        }
        else
        {
            canPurchase.text = "You cannot purchase this upgrade";
            b.interactable = false;
        }

        panel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Cost: " + costInv.listInventory();
        panel.transform.GetChild(3).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Upgrade to level " + (i+1);

        if (maxLevel)
        {
            canPurchase.text = "You have already reached max level";
            panel.transform.GetChild(3).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Maxed out";
            b.interactable = false;
        }

    }


    public void callReactorUpgrade() //nejvetsi picovina, ja nemuzu direcly callovat static metody buttonem takze to mam takhle jak debil
    {
        ProgressionScript.upgradeReactor();
        showDialogue(0);
    }
    public void callCockpitUpgrade() 
    {
        ProgressionScript.upgradeCockpit();
        showDialogue(1);
    }

    public void callRoom1Upgrade()
    {
        ProgressionScript.upgradeRoom1();
        showDialogue(2);
    }

    public void callRoom2Upgrade()
    {
        ProgressionScript.upgradeRoom2();
        showDialogue(3);
    }

    public void callRoom3Upgrade()
    {
        ProgressionScript.upgradeRoom3();
        showDialogue(4);
    }

    public void callRoom4Upgrade()
    {
        ProgressionScript.upgradeRoom4();
        showDialogue(5);
    }

    public void callRoom5Upgrade()
    {
        ProgressionScript.upgradeRoom5();
        showDialogue(6);
    }

    public void callRoom6Upgrade()
    {
        ProgressionScript.upgradeRoom6();
        showDialogue(7);
    }

}
