using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using Photon.Pun;
using UnityEngine;

public class MapGeneratorScript : MonoBehaviour
{
    public GameObject dirt;
    public GameObject stone;
    public GameObject gold;
    public int seed;
    public Hash h = new Hash();

    public void Start()
    {
        //if (PhotonNetwork.IsMasterClient) return;
        //generateMap(minGeneratedX, maxGeneratedX - minGeneratedX, 0, true);
        //DateTime before = System.DateTime.Now;
        //this.generateMap(0, 50, -100, 50, true);
        //Debug.Log("time: " + (System.DateTime.Now - before).TotalMilliseconds);
    }

    public void Update()
    {

        if (Input.GetKeyDown("i"))
        {
            DateTime before = System.DateTime.Now;
            this.generateMapAroundYou(20, 20, true);
            Debug.Log("time: " + (System.DateTime.Now - before).TotalMilliseconds);
        }
        if (Input.GetKeyDown("k"))

        {
            DateTime before = System.DateTime.Now;
            this.generateMap(0, 10, -100, 10, true);
            Debug.Log("time: " + (System.DateTime.Now - before).TotalMilliseconds);
        }

    }


    int index = 0;
    public void FixedUpdate()
    {
        index++;

        if (index % 25 == 0)
        {
            //this.generateMapAroundYou(25, 12, true);
        }
    }

    public void generateMapAroundYou(int width, int height, bool hardLoad)
    {
        Vector3 pos = GameObject.FindGameObjectWithTag("Player").transform.position;

        Debug.Log("gmay at: " + pos);

        this.generateMap((int)pos.x, width, (int)pos.y, height, hardLoad);



    }



    public void generateMap(int xStart, int width, int topY, int height, bool hardLoad)
    {
        StartCoroutine(actuallyGenerateMap(xStart, width, topY, height , hardLoad));
    }


    IEnumerator actuallyGenerateMap(int xMid, int width, int midY, int height, bool hardLoad)
    {
        
        FileManager fm = GameObject.Find("FileManager").GetComponent<FileManager>();

        
        /*
        GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Block");
        ArrayList relevantBlocks = new ArrayList();
        
        foreach (GameObject b in allBlocks)
        {
            if (b.transform.position.x < xMid - width ||
                b.transform.position.x > xMid + width ||
                b.transform.position.y < midY - height ||
                b.transform.position.y > midY + height
                )

                continue;

            relevantBlocks.Add(b);
        }*/
           


        for (int x = xMid-width; x < xMid + width; x++)
        for (int y = midY + height;  y > midY - height; y--)
        {
            if (y > 0) continue;
            
            /*bool shouldSkip = false;
            foreach (GameObject b in relevantBlocks)
            {
                    if ((int)b.transform.position.x == x && (int)b.transform.position.y == y)
                    {
                        shouldSkip = true;
                        break;
                    }
            }

                if (shouldSkip) continue;*/


            
            if (fm.isDestroyed(x, y)) continue;
            //Debug.Log("Creating block at: " + x + "/" + y);
            //Instantiate(computeBlockAt(x, y, seed), new Vector3(x, y, 0), Quaternion.identity);
                Instantiate(dirt, new Vector3(x, y, 0), Quaternion.identity);
                if (!hardLoad)yield return null;
        }

    }

    public GameObject computeBlockAt(int x, int y, int seed)
    {
        y = -y;
        h.setHash(x, y, seed);
        if (y < 3) return dirt;
        else if (y == 3 && (h.v % 2 == 0 || h.v % 3 == 0)) return dirt;
        else if ((y == 4 || y == 5) && h.v % 2 == 0) return dirt;
        else if (h.v < 10000) return gold;
        else return stone;
    }

    public void removeBlockAt(int x, int y)
    {
        Debug.Log("Called removeblock at  " + x + ", " + y);
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");

        foreach (GameObject b in blocks)
        {
            if (b.transform.position.x == x && b.transform.position.y == y)
            {
                Debug.Log("Found object");
                Destroy(b);
                return;
            }
        }
    }

    public float getNearestNotGeneratedBlockDistance()
    {
        GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Block");
        ArrayList relevantBlocks = new ArrayList();



        return 0;
    }

}

public class Hash
{

    public int v;

    public void setHash(int x, int y, int seed)  
    {
        /*bool isLeft = x < 0;
        double rootSeed = Math.Pow(seed, 0.25);
        double modSeed8 = seed % 8;
        double modX64 = x > 0 ? (x % 64) : 1;
        double modY59 = y > 0 ? (y % 59) : 1;
        double x1 = Math.Pow(Math.Pow(x, 8) + seed * 8, 1f / 2f) / (modX64 + 2);
        double y1 = Math.Pow(Math.Pow(y, 7) + seed * 3.5, 1f / 3f) / (modY59 + 2);
        double big = Math.E * Math.Pow(x1, 0.5) * Math.Pow(y1, 0.5) / seed;
        big *= (this.getFullDecimal(x1 * y1 * Math.Pow(big, 0.5)) + modSeed8) * rootSeed;
        if (isLeft) big *= Math.Pow(seed / 18.5, 19f / 41f);
        v = int.Parse(this.getPart(big));
        //Debug.Log("x: " + x + ", y: " + y + ", v: " + v);*/

        double x1 = Math.Pow(Math.Pow(x, 8) + seed * 8, 1f / 2f) / ((x > 0 ? (x % 64) : 1) + 2);
        double y1 = Math.Pow(Math.Pow(y, 7) + seed * 3.5, 1f / 3f) / ((y > 0 ? (y % 59) : 1) + 2);
        double big = Math.E * Math.Pow(x1, 0.5) * Math.Pow(y1, 0.5) / seed;
        big *= (this.getFullDecimal(x1 * y1 * Math.Pow(big, 0.5)) + seed % 8) * Math.Pow(seed, 0.25);
        if (x < 0) big *= Math.Pow(seed / 18.5, 19f / 41f);
        v = int.Parse(this.getPart(big));
        //Debug.Log("x: " + x + ", y: " + y + ", v: " + v);
    }

    private double getFullDecimal(double input)
    {
        //String s = input.ToString();
        return double.Parse(input.ToString().Substring(input.ToString().IndexOf(".") + 1).Replace("E", ""));
    }

    private string getPart(double input)
    {
        string s = input.ToString();
        int index = s.IndexOf(".");
        try { return reverseString(s.Substring(index + 1, 5));} catch (Exception e){}
        try { return reverseString(s.Substring(index -5, 5));}  catch (Exception e){}
        Debug.Log("pruser jak brno in mapgen.getpart");
        return "99999";
    }

    private String reverseString(String str)
    {
        char[] chars = str.ToCharArray();
        char[] result = new char[chars.Length];
        for (int i = 0, j = str.Length - 1; i < str.Length; i++, j--)
            result[i] = chars[j];
        return new string(result);
    }
}

