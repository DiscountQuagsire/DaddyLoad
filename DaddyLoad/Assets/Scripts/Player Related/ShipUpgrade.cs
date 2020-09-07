using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipUpgrade : MonoBehaviour
{

    public GameObject[] Dialogues;

    public void hideAllDialogues()
    {
        for (int i=0; i<Dialogues.Length; i++)
        {
            Dialogues[i].SetActive(false);
        }
    }

    public void showDialogue(int index)
    {
        hideAllDialogues();
        Dialogues[index].SetActive(true);
    }

}
