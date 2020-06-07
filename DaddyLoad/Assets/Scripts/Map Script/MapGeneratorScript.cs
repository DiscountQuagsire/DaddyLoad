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

    public int chunkSize = 10;

    public int sightHalfWidth = 23;
    public int sightHalfHeight = 10;

    public ArrayList loadedChunkCoordinates = new ArrayList();
    FileManager fm;

    public void Start()
    {
        fm = GameObject.Find("FileManager").GetComponent<FileManager>();
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
            this.generateNearbyUnloadedChunks();
        }

        if (Input.GetKeyDown("c"))
        {
            this.unloadFarAwayChunks();
        }

    }


    int index = 0;
    public void FixedUpdate()
    {
        index++;

        if (index % 49 == 0)
        {
            this.generateNearbyUnloadedChunks();
        }
        if (index % 25 == 0)
        {
            this.unloadFarAwayChunks();
        }
    }

    private void unloadFarAwayChunks()
    {
        Vector3 pos = GameObject.FindGameObjectWithTag("Player").transform.position;
        for (int i = 0; i < loadedChunkCoordinates.Count; i++)
        {
            Coordinate c = (Coordinate)loadedChunkCoordinates[i];
            if (c.isWithinSight(pos, sightHalfWidth*2, sightHalfHeight*2, chunkSize)) continue; // corrects for assymetry
            {                                                                                   // jestli jsem nic neposral, that is
                StartCoroutine(this.removeChunkAt(c, true));
                loadedChunkCoordinates.Remove(c);
                i--;
            }
        }
    }

    public void generateNearbyUnloadedChunks() // what even is optimization
    {

        Vector3 pos = GameObject.FindGameObjectWithTag("Player").transform.position;
        int drillX = (int)pos.x;
        int drillY = (int)pos.y;

        for (int possibleX = 0; possibleX < drillX + sightHalfWidth*2; possibleX += chunkSize)
        for (int possibleY = 0; possibleY > drillY - sightHalfHeight*2; possibleY -= chunkSize)
        {
            Coordinate c = new Coordinate(possibleX, possibleY);
            if (c.isWithinSight(pos, sightHalfWidth, sightHalfHeight, chunkSize))
                this.attemptToGenerateChunk(new Coordinate(possibleX, possibleY), true);
            
        }
    }

    IEnumerator removeChunkAt(Coordinate c, bool hardLoad)
    {
        DateTime before = System.DateTime.Now;

        for (int x = c.x; x < c.x + chunkSize; x++)
            for (int y = c.y; y > c.y - chunkSize; y--)
            {
                removeBlockAt(x, y);
                if (!hardLoad) yield return null;
            }
        Debug.Log("time to generate chunk: " + c.x + ", " + c.y + ": " + (System.DateTime.Now - before).TotalMilliseconds);
    }

    public void attemptToGenerateChunk(Coordinate c, bool hardLoad)
    {
        if (c.isInArray(loadedChunkCoordinates)) return;
        loadedChunkCoordinates.Add(c);
        StartCoroutine(this.actuallyGenerateChunk(c, hardLoad));
    }

    IEnumerator actuallyGenerateChunk(Coordinate c, bool hardLoad)
    {
        DateTime before = System.DateTime.Now;

        for (int x = c.x; x < c.x + chunkSize; x++)
        for (int y = c.y; y > c.y - chunkSize; y--)
        {
                
            if (y > 0) continue;
            if (fm.isDestroyed(x, y)) continue;
            //Debug.Log("Creating block at: " + x + "/" + y);
            Instantiate(computeBlockAt(x, y, seed), new Vector3(x, y, 0), Quaternion.identity);
            if (!hardLoad) yield return null;
        }
        yield return null;
        Debug.Log("time to generate chunk: " + c.x + ", " + c.y + ": " + (System.DateTime.Now - before).TotalMilliseconds);
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
        //Debug.Log("Called removeblock at  " + x + ", " + y);
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");

        foreach (GameObject b in blocks)
        {
            if (b.transform.position.x == x && b.transform.position.y == y)
            {
                //Debug.Log("Found object");
                Destroy(b);
                return;
            }
        }
    }
}

public class Hash
{

    public int v;

    public void setHash(int x, int y, int seed)  
    {
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
        string s = input.ToString("F99").TrimEnd('0');
        //return double.Parse(input.ToString().Substring(input.ToString().IndexOf(".") + 1).Replace("E","").Replace("+","").Replace("-", ""));
        try
        {
            return double.Parse(input.ToString().Substring(s.IndexOf(".") + 1));
        }
        catch (Exception e)
        {
            Debug.Log("problem");
            return 12456789; 
        }
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
