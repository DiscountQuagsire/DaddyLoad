using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject canvas;
    public Text nameText;
    public Text dialogueText;

    public Dialogue dialogue;
    private int currentLine = 0; 

    public void StartDialogue(Dialogue newDialogue)
    {
        canvas.SetActive(true);
        dialogue = newDialogue;
        nameText.text = dialogue.characterName;

        currentLine = 0;
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (currentLine >= dialogue.sentences.Length | dialogue == null)
        {
            EndDialogue();
        }
        else
        {
            dialogueText.text = dialogue.sentences[currentLine];
            currentLine += 1;
        }

    }

    public void EndDialogue()
    {
        canvas.SetActive(false);
    }


}

