using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverviewCanvasScript : MonoBehaviour
{
    public GameObject parentPanel;
    public TextMeshProUGUI tmp;
    public GameObject overviewCanvas;

    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            parentPanel.SetActive(true);
            Debug.Log("down");
            reloadOverview();
        }
        if (Input.GetKeyUp("e"))
        {
            parentPanel.SetActive(false);
            Debug.Log("up");
        }
    }

    private void reloadOverview()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("GUI Removable");
        foreach(GameObject go in objects)
        {
            Destroy(go);
            Debug.Log("destroying go");
        }

        Debug.Log("reload fired");
        
        Dictionary<string, int> materials = MapGeneratorScript.inventory.materials;
        Vector3 globalLocalOffset = new Vector3(200, 0, 0);

        int yPos = 200;

        foreach (KeyValuePair<string, int> pair in materials)
        {
            GameObject newObj = new GameObject();
            Image newImage = newObj.AddComponent<Image>();
            string str = pair.Key;
            string name = char.ToUpper(str[0]) + str.Substring(1);
            Sprite s = (Sprite)Resources.Load("Sprites/Blocks/" + name, typeof(Sprite));
            newImage.sprite = s;
            newObj.GetComponent<RectTransform>().SetParent(parentPanel.transform);
            newObj.transform.position = new Vector3(500, yPos, 0);
            newObj.tag = "GUI Removable";
            newObj.SetActive(true);
            newObj.name = "Inventory icon: " + name;
            newObj.transform.localScale = new Vector3(0.5f, 0.5f, 0);

            TextMeshProUGUI TMPObject = Instantiate(tmp);
            TMPObject.transform.SetParent(newObj.transform);
            TMPObject.transform.Translate(newObj.transform.position + new Vector3(-50, -15, 0));
            TMPObject.SetText(pair.Value + "");
            yPos += 45;
        }
    }

}
