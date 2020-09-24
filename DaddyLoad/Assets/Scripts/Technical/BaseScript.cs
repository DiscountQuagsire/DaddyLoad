using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class BaseScript: MonoBehaviourPunCallbacks
{
    public ArrayList baseCoordinates = new ArrayList();
    GameObject player;
    public GameObject escCanvas;
    public Button saveButton;
    private bool isNearBase = false;

    void Start()
    {
        FileManager.Start();
        ProgressionScript.Start();
        Debug.Log("BS Start");
        if (PhotonNetwork.IsMasterClient)
        {
            ArrayList list = FileManager.getBasesFromFile();
            foreach(float f in list)
            {
                this.addBase(f);
            }
        }
        saveButton.GetComponent<Button>().onClick.AddListener(FileManager.writeBasesToFile);
        saveButton.GetComponent<Button>().onClick.AddListener(FileManager.writeDownInventory);
        saveButton.GetComponent<Button>().onClick.AddListener(FileManager.writeShipUpgradesToFile);
        saveButton.GetComponent<Button>().onClick.AddListener(FileManager.writeUnwrittenBlocksToFile);

    }

    public void addBase(float x)
    {
        baseCoordinates.Add(x);
        PhotonNetwork.Instantiate("BasePrefab", new Vector3(x, 3f, 0), Quaternion.identity);
    }

    public void addBaseHere()
    {
        this.addBase(player.transform.position.x);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool active = escCanvas.activeSelf;
            if (active)
            {
                escCanvas.SetActive(false);
            }
            else
            {
                escCanvas.SetActive(true);

                if (!PhotonNetwork.IsMasterClient)
                {
                    saveButton.interactable = false;
                    return;
                }

                if (isNearBase)
                {
                    saveButton.interactable = true;
                }
                else
                {
                    saveButton.interactable = false;
                }
            }
            
        }
    }

    int index = 0;
    void FixedUpdate()
    {
        MapGeneratorScript.FixedUpdate();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }

        index++;
        if (index % 50 == 0)
        {
            this.reloadBases();
        }

        isNearBase = false;
        foreach (float xPos in baseCoordinates)
        {
            if (Mathf.Abs(player.transform.position.x - xPos) < 5 && player.transform.position.y > 0)
            {
                isNearBase = true;
            }
            if (player.transform.position.x < 10)
            {
                isNearBase = true;
            }
        }
    }

    private void reloadBases()
    {
        baseCoordinates.Clear();
        GameObject[] bases = GameObject.FindGameObjectsWithTag("Base");
        foreach (GameObject go in bases)
        {
            baseCoordinates.Add(go.transform.position.x);
        }
    }
}