using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipUIScript : MonoBehaviour
{

    public GameObject cockpit;
    public Sprite cockpit0;
    public Sprite cockpit1;

    private void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            Debug.Log("updating imagew");
            cockpit.GetComponent<Image>().sprite = cockpit1;
        }
    }



}
