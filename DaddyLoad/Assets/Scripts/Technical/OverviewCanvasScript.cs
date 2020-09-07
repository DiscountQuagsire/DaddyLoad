using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverviewCanvasScript : MonoBehaviour
{

    public GameObject parentPanel;
    public Sprite sprite;
    private GameObject newObj;

    void Start()
    {
        newObj = new GameObject();
        Image newImage = newObj.AddComponent<Image>();
        newImage.sprite = sprite;
        newObj.GetComponent<RectTransform>().SetParent(parentPanel.transform);
        newObj.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKey("g"))
        {
            newObj.transform.position = new Vector3(900, 500, 0);
        }
        if (Input.GetKey("f"))
        {
            reloadOverview();
        }
    }

    private void reloadOverview()
    {
        MapGeneratorScript mgs = this.mgs();
        int yPos = 0;
        Dictionary<string, int> localMaterials = mgs.localInventory.materials;
        Dictionary<string, int> globalMaterials = mgs.globalInventory.materials;


        newObj = new GameObject();
        Image newImage = newObj.AddComponent<Image>();
        Sprite s = (Sprite)Resources.Load("Sprites/Blocks/Diamond", typeof(Sprite));
        newImage.sprite = s;
        newObj.GetComponent<RectTransform>().SetParent(parentPanel.transform);
        newObj.transform.position = new Vector3(500, 500, 0);
        newObj.SetActive(true);


    }

    private MapGeneratorScript mgs()
    {
        return GameObject.Find("MapGenerator").GetComponent<MapGeneratorScript>();
    }


}
