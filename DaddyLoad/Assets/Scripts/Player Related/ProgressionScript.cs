using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProgressionScript : MonoBehaviourPunCallbacks
{

    public int getThrusterLevel() {return thrusters; }
    public int getTemperatureShieldsLevel() { return tempShields; }
    public int getPressureShieldsLevel() { return presShields; }
    public int getBodyworkLevel() { return bodywork; }
    public int getReactorLevel() { return reactor; }
    public bool getCommRoom() { return commRoom; }
    public bool getCircuitry() { return circuitry; }
    public bool getWindows() { return windows; }
    public bool getFlaps() { return flaps; }


    /// 


    private int thrusters = 0;
    private int tempShields = 0;
    private int presShields = 0;
    private int bodywork = 0;
    private int reactor = 0;
    private bool commRoom = false;
    private bool circuitry = false;
    private bool windows = false;
    private bool flaps = false;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject.Find("FileManager").GetComponent<FileManager>().loadShipUpgradesFromFile();
        }
    }

    private void Update()
    {

    }

    public void setThrusters(int t)
    {
        thrusters = t;
    }
    public void setTempShields(int t)
    {
        tempShields = t;
    }
    public void setPresShields(int t)
    {
        presShields = t;
    }
    public void setBodywork(int t)
    {
        bodywork = t;
    }
    public void setReactor(int t)
    {
        reactor = t;
    }
    public void setCommRoom(bool t)
    {
        commRoom = t;
    }
    public void setCircuitry(bool t)
    {
        circuitry = t;
    }

    public void setWindows(bool t)
    {
        windows = t;
    }
    public void setFlaps(bool t)
    {
        flaps = t;
    }



    public void sendShipUpgradeInfoToEverybody()
    {
        string output = "";

        output += this.getThrusterLevel() + "/";
        output += this.getTemperatureShieldsLevel() + "/";
        output += this.getPressureShieldsLevel() + "/";
        output += this.getBodyworkLevel() + "/";
        output += this.getReactorLevel() + "/";

        output += this.getCommRoom() ? 1 + "/" : 0 + "/";
        output += this.getCircuitry() ? 1 + "/" : 0 + "/";
        output += this.getWindows() ? 1 + "/" : 0 + "/";
        output += this.getFlaps() ? 1 : 0;

        GameObject.FindGameObjectWithTag("Player").GetComponent<CommunicationScript>().photonView.RPC(
            "receiveMessage", RpcTarget.All, "shipinfo/" + output);
    }



}
