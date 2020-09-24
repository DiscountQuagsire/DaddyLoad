using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProgressionScript 
{

    public static void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            FileManager.loadShipUpgradesFromFile();
        }
    }

    public static string getShipUpgradesString()
    {
        string output = "";

        output += getCockpit() + "/";
        output += getReactor() + "/";
        output += getRoom1() + "/";
        output += getRoom2() + "/";
        output += getRoom3() + "/";
        output += getRoom4() + "/";
        output += getRoom5() + "/";
        output += getRoom6() + "/";

        return output;
    }
    public static void sendShipUpgradeInfoToEverybody()
    {
        string output = getShipUpgradesString();

        GameObject.FindGameObjectWithTag("Player").GetComponent<CommunicationScript>().photonView.RPC(
            "receiveMessage", RpcTarget.All, "shipinfo/" + output);
    }

    public static void loadShipUpgradesFromString(string input)
    {

        string[] segmented = input.Split('/');

        setCockpit(int.Parse(segmented[0]));
        setReactor(int.Parse(segmented[1]));
        setRoom1(int.Parse(segmented[2]));
        setRoom2(int.Parse(segmented[3]));
        setRoom3(int.Parse(segmented[4]));
        setRoom4(int.Parse(segmented[5]));
        setRoom5(int.Parse(segmented[6]));
        setRoom6(int.Parse(segmented[7]));

    }

    private static int reactor = 0;
    private static int cockpit = 0;
    private static int room1 = 0;
    private static int room2 = 0;
    private static int room3 = 0;
    private static int room4 = 0;
    private static int room5 = 0;
    private static int room6 = 0;

    public static int getCockpit() { return cockpit; }
    public static int getReactor() { return reactor; }
    public static int getRoom1() { return room1; }
    public static int getRoom2() { return room2; }
    public static int getRoom3() { return room3; }
    public static int getRoom4() { return room4; }
    public static int getRoom5() { return room5; }
    public static int getRoom6() { return room6; }


    public static void upgradeCockpit()
    {
        if (!MapGeneratorScript.inventory.containsInventory(CostScript.getCost("cockpit", cockpit+1)))
        {
            Debug.Log("not enough materials in upgrade; this should never happen");
            return;
        }
        MapGeneratorScript.inventory.removeInventoryContents(CostScript.getCost("cockpit", cockpit + 1));

        setCockpit(cockpit + 1);
        sendShipUpgradeInfoToEverybody();
        GameObject.Find("Ship Upgrade UI").GetComponent<ShipUIScript>().reloadSprites();
    }
    public static void upgradeReactor()
    {
        if (!MapGeneratorScript.inventory.containsInventory(CostScript.getCost("reactor", reactor + 1)))
        {
            Debug.Log("not enough materials in upgrade; this should never happen");
            return;
        }
        MapGeneratorScript.inventory.removeInventoryContents(CostScript.getCost("reactor", reactor + 1));

        setReactor(reactor + 1);
        sendShipUpgradeInfoToEverybody();
        GameObject.Find("Ship Upgrade UI").GetComponent<ShipUIScript>().reloadSprites();
    }
    public static void upgradeRoom1()
    {
        if (!MapGeneratorScript.inventory.containsInventory(CostScript.getCost("room1", room1 + 1)))
        {
            Debug.Log("not enough materials in upgrade; this should never happen");
            return;
        }
        MapGeneratorScript.inventory.removeInventoryContents(CostScript.getCost("room1", room1 + 1));

        setRoom1(room1 + 1);
        sendShipUpgradeInfoToEverybody();
        GameObject.Find("Ship Upgrade UI").GetComponent<ShipUIScript>().reloadSprites();
    }
    public static void upgradeRoom2()
    {
        if (!MapGeneratorScript.inventory.containsInventory(CostScript.getCost("room2", room2 + 1)))
        {
            Debug.Log("not enough materials in upgrade; this should never happen");
            return;
        }
        MapGeneratorScript.inventory.removeInventoryContents(CostScript.getCost("room2", room2 + 1));

        setRoom2(room2 + 1);
        sendShipUpgradeInfoToEverybody();
        GameObject.Find("Ship Upgrade UI").GetComponent<ShipUIScript>().reloadSprites();
    }
    public static void upgradeRoom3()
    {
        if (!MapGeneratorScript.inventory.containsInventory(CostScript.getCost("room3", room3 + 1)))
        {
            Debug.Log("not enough materials in upgrade; this should never happen");
            return;
        }
        MapGeneratorScript.inventory.removeInventoryContents(CostScript.getCost("room3", room3 + 1));

        setRoom3(room3 + 1);
        sendShipUpgradeInfoToEverybody();
        GameObject.Find("Ship Upgrade UI").GetComponent<ShipUIScript>().reloadSprites();
    }
    public static void upgradeRoom4()
    {
        if (!MapGeneratorScript.inventory.containsInventory(CostScript.getCost("room4", room4 + 1)))
        {
            Debug.Log("not enough materials in upgrade; this should never happen");
            return;
        }
        MapGeneratorScript.inventory.removeInventoryContents(CostScript.getCost("room4", room4 + 1));

        setRoom4(room4 + 1);
        sendShipUpgradeInfoToEverybody();
        GameObject.Find("Ship Upgrade UI").GetComponent<ShipUIScript>().reloadSprites();
    }
    public static void upgradeRoom5()
    {
        if (!MapGeneratorScript.inventory.containsInventory(CostScript.getCost("room5", room5 + 1)))
        {
            Debug.Log("not enough materials in upgrade; this should never happen");
            return;
        }
        MapGeneratorScript.inventory.removeInventoryContents(CostScript.getCost("room5", room5 + 1));

        setRoom5(room5 + 1);
        sendShipUpgradeInfoToEverybody();
        GameObject.Find("Ship Upgrade UI").GetComponent<ShipUIScript>().reloadSprites();
    }
    public static void upgradeRoom6()
    {
        if (!MapGeneratorScript.inventory.containsInventory(CostScript.getCost("room6", room6 + 1)))
        {
            Debug.Log("not enough materials in upgrade; this should never happen");
            return;
        }
        MapGeneratorScript.inventory.removeInventoryContents(CostScript.getCost("room6", room6 + 1));

        setRoom6(room6 + 1);
        sendShipUpgradeInfoToEverybody();
        GameObject.Find("Ship Upgrade UI").GetComponent<ShipUIScript>().reloadSprites();

    }

    /// 
    public static void setReactor(int i)
    {
        reactor = i;
    }

    public static void setCockpit(int i)
    {
        cockpit = i;
    }

    public static void setRoom1(int i)
    {
        room1 = i;
    }

    public static void setRoom2(int i)
    {
        room2 = i;
    }

    public static void setRoom3(int i)
    {
        room3 = i;
    }

    public static void setRoom4(int i)
    {
        room4 = i;
    }

    public static void setRoom5(int i)
    {
        room5 = i;
    }
    public static void setRoom6(int i)
    {
        room6 = i;
    }





}
