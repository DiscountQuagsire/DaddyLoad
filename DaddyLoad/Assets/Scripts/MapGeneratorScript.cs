using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorScript : MonoBehaviour
{
    public GameObject dirt;
    public GameObject stone;
    public int seed;

    public void Update()
    {
        if (!Input.GetKeyDown("g")) return;
        generateMap();
    }

    public void generateMap()
    {
        
        for (int x = 0; x < 10; x++)
        for (int y = 0; y < 10; y++)
        {
                if (y > 5)
                {
                    Instantiate(dirt, new Vector3(x, y, 0), Quaternion.identity);
                }
                else
                {
                    Instantiate(stone, new Vector3(x, y, 0), Quaternion.identity);
                }
        }
    }
}
