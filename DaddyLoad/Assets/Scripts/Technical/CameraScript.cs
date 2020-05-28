using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject player;

    // Update is called once per frame
    private void LateUpdate()
    {
        if (player == null)
        {
            Debug.Log("player is null");
            player = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            Debug.Log(player + " player");
        }

        transform.position = player.transform.position + new Vector3(0, 0, -20);

    }
}
