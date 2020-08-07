using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    private bool controlPanelToggle = false;
    private bool ControlPanelToggle
    {
        get
        {
            return controlPanelToggle;
        }
        set
        {
            controlPanelToggle = value;
            StartCoroutine("toggleControlPanel", controlPanelToggle);
        }
    }

    private float TabTimerStart = 1f;
    private float TabTimer;


    void Start()
    {
        TabTimer = TabTimerStart;
    }


    void Update()
    {
        if (Input.GetKeyDown("tab"))
        {
            if (TabTimer < 0f)
            {
                //Starts coroutine to hide/reveal control panel
                ControlPanelToggle = !controlPanelToggle;
                TabTimer = TabTimerStart;
            }
        }

        TabTimer -= Time.deltaTime;
    }


    //Hides or reveals Control panel
    IEnumerator toggleControlPanel(bool toggle)
    {
        Transform ControlPanel = gameObject.transform;
        if (toggle)
        {
            Vector3 target = new Vector3(-120, 311, 0);
            while (Vector3.Distance(ControlPanel.position, target) > 0.1f)
            {
                ControlPanel.position = Vector3.Lerp(ControlPanel.position, target, 10f * Time.deltaTime);
                yield return null;
            }
        }
        else if (!toggle)
        {
            Vector3 target = new Vector3(135, 311, 0);
            while (Vector3.Distance(ControlPanel.position, target) > 0.1f)
            {
                ControlPanel.position = Vector3.Lerp(ControlPanel.position, target, 10f * Time.deltaTime);
                yield return null;
            }
        }
    }
}
