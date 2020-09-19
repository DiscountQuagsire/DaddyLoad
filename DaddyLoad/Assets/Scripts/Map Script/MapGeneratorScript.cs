using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

public class MapGeneratorScript
{
    public static int seed;

    public static int chunkSize = 10;
    public static int sightHalfWidth = 23;
    public static int sightHalfHeight = 12;

    public static ArrayList loadedChunkCoordinates = new ArrayList();
    public static Inventory inventory = new Inventory();

    public static void Update()
    {

        if (Input.GetKeyDown("l"))
        {
            Debug.Log("listing all ship upgrades: " + ProgressionScript.getShipUpgradesString());
        }

    }

    static int index = 0;
    public static void FixedUpdate()
    {
        index++;

        if (index % 5 == 0)
        {
            generateNearbyUnloadedChunks();
        }
        if (index % 50 == 0)
        {
            unloadFarAwayChunks();
        }
    }

    private static void unloadFarAwayChunks()
    { 
        Vector3 pos = GameObject.FindGameObjectWithTag("Player").transform.position;
        for (int i = 0; i < loadedChunkCoordinates.Count; i++)
        {
            Coordinate c = (Coordinate)loadedChunkCoordinates[i];
            if (c.isWithinSight(pos, sightHalfWidth*2, sightHalfHeight*2, chunkSize)) continue; // corrects for assymetry
            {                                                                                   // jestli jsem nic neposral
                removeChunkAtNew(c);
                loadedChunkCoordinates.Remove(c);
                i--;
            }
        }
    }

    public static void generateNearbyUnloadedChunks()
    {
        Vector3 pos = GameObject.FindGameObjectWithTag("Player").transform.position;
        int drillX = (int)pos.x;
        int drillY = (int)pos.y;

        for (int possibleX = 0; possibleX < drillX + sightHalfWidth*2; possibleX += chunkSize)
        for (int possibleY = 0; possibleY > drillY - sightHalfHeight*2; possibleY -= chunkSize)
        {
            Coordinate c = new Coordinate(possibleX, possibleY);
            if (c.isWithinSight(pos, sightHalfWidth, sightHalfHeight, chunkSize))
            { 
                if (c.isInArray(loadedChunkCoordinates)) continue;
                loadedChunkCoordinates.Add(c);
                BiomeManager.getBiomeAt(possibleX).actuallyGenerateChunk(c);
                return;
            }

        }
    }

    public static void removeChunkAtNew(Coordinate c)
    {
        DateTime before = System.DateTime.Now;

        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        int destroyedCount = 0;

        for (int i = blocks.Length - 1; i >= 0; i--)
        {
            if (blocks[i].transform.position.x >= c.x &&
                blocks[i].transform.position.x < c.x + chunkSize &&
                blocks[i].transform.position.y <= c.y &&
                blocks[i].transform.position.y > c.y - chunkSize)
            {
                GameObject.Destroy(blocks[i]);
                destroyedCount++;
                if (destroyedCount >= chunkSize * chunkSize) break;
            }
        }

        //Debug.Log("time to unload chunk using new: " + c.x + ", " + c.y + ": " + (System.DateTime.Now - before).TotalMilliseconds);
    }

    public static void removeBlockAt(int x, int y)
    {
        //Debug.Log("Called removeblock at  " + x + ", " + y);
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");

        foreach (GameObject b in blocks)
        {
            if (b.transform.position.x == x && b.transform.position.y == y)
            {
                //Debug.Log("Found object");
                GameObject.Destroy(b);
                return;
            }
        }
    }

    public static ProgressionScript ps()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<ProgressionScript>();
    }

}
public class BiomeManager
{

    public static Desert desert = new Desert();
    public static Base realBase = new Base(); 
    public static Hash h = new Hash();
    public static int chunkSize = 10;

    public static Biome getBiomeAt(int x)
    {
        if (x >= 0 && x < 20)
            return realBase;
        else
            return desert;
    }

}

public abstract class Biome 
{
    public GameObject dirt;
    public GameObject stone;
    public GameObject sand;
    public GameObject sandstone;
    public GameObject grass;

    public GameObject bronze;
    public GameObject plastic;
    public GameObject aluminum;
    public GameObject iron;
    public GameObject silver;
    public GameObject lead;
    public GameObject gold;
    public GameObject copper;
    public GameObject platinum;
    public GameObject iridium;
    public GameObject titanium;
    public GameObject diamond;
    public GameObject emerald;
    public GameObject magmastone;
    public GameObject magma;
    public GameObject pinkore;
    
    public GameObject GameObject;

    public GameObject error;
    public GameObject background1;

    public Hash h;


    public Biome()
    {
        h = new Hash();

        dirt =      (GameObject)Resources.Load("Dirt Variant",      typeof(GameObject));
        stone =     (GameObject)Resources.Load("Stone Variant",     typeof(GameObject));
        sand =      (GameObject)Resources.Load("Sand Variant",      typeof(GameObject));
        sandstone = (GameObject)Resources.Load("Sandstone Variant", typeof(GameObject));
        grass =     (GameObject)Resources.Load("Grass Variant",     typeof(GameObject));

        bronze =    (GameObject)Resources.Load("Bronze Variant",    typeof(GameObject));
        plastic =   (GameObject)Resources.Load("Plastic Variant",   typeof(GameObject));
        aluminum =  (GameObject)Resources.Load("Aluminum Variant",  typeof(GameObject));
        iron =      (GameObject)Resources.Load("Iron Variant",      typeof(GameObject));
        silver =    (GameObject)Resources.Load("Silver Variant",    typeof(GameObject));
        lead =      (GameObject)Resources.Load("Lead Variant",      typeof(GameObject));
        gold =      (GameObject)Resources.Load("Gold Variant",      typeof(GameObject));
        copper =    (GameObject)Resources.Load("Copper Variant",    typeof(GameObject));
        platinum =  (GameObject)Resources.Load("Platinum Variant",  typeof(GameObject));
        iridium =   (GameObject)Resources.Load("Iridium Variant",   typeof(GameObject));
        titanium =  (GameObject)Resources.Load("Titanium Variant",  typeof(GameObject));
        diamond =   (GameObject)Resources.Load("Diamond Variant",   typeof(GameObject));

        emerald =   (GameObject)Resources.Load("Emerald Variant",   typeof(GameObject));
        magmastone =(GameObject)Resources.Load("Magmastone Variant",typeof(GameObject));
        magma =     (GameObject)Resources.Load("Magma Variant",     typeof(GameObject));
        pinkore =   (GameObject)Resources.Load("Pinkore Variant",   typeof(GameObject));
        

        error =     (GameObject)Resources.Load("Error Variant",     typeof(GameObject));
        background1=(GameObject)Resources.Load("Background Blocks/Background Underground 1 Variant",     typeof(GameObject));
    }

    public abstract GameObject getBlockAt(int x, int y);
    public abstract void actuallyGenerateChunk(Coordinate c);

    public abstract float getTemperature(float depth);
    public abstract float getPressure(float depth);
    public void addBackground(int x, int y)
    {
        GameObject.Instantiate(background1, new Vector3(x, y, 0), Quaternion.identity);
    }
}

public class Desert : Biome
{


    public Desert() :  base() { }

    public override GameObject getBlockAt(int x, int y)
    {
        y = -y;

        if (y < 3) return sand;

        h.setNewHash(x, y, MapGeneratorScript.seed);

        if (y == 3 && (h.v % 2 == 0 || h.v % 3 == 0)) return sand;



        else if (y >= 450 && y < 500 && h.v >= 0 && h.v < 1000) return diamond;
        else if (y >= 400 && y < 500 && h.v >= 1000 && h.v < 2000) return titanium;
        else if (y >= 350 && y < 500 && h.v >= 2000 && h.v < 3000) return iridium;
        else if (y >= 300 && y < 500 && h.v >= 3000 && h.v < 4000) return platinum;
        else if (y >= 250 && y < 500 && h.v >= 4000 && h.v < 5000) return copper;
        else if (y >= 150 && y < 350 && h.v >= 5000 && h.v < 6500) return gold;
        else if (y >= 150 && y < 180 && h.v >= 6500 && h.v < 8000) return lead;
        else if (y >= 100 && y < 300 && h.v >= 8000 && h.v < 9500) return silver;
        else if (y >= 50 && y < 150 && h.v >= 9500 && h.v < 11000) return iron;
        else if (y >= 40 && y < 150 && h.v >= 11000 && h.v < 12500) return aluminum;
        else if (y >= 10 && y < 120 && h.v >= 12500 && h.v < 15000) return plastic;
        else if (y >= 10 && y < 120 && h.v >= 15000 && h.v < 17500) return bronze;

        return sandstone;
    }

    public override void actuallyGenerateChunk(Coordinate c)
    {
        //DateTime before = System.DateTime.Now;

        for (int x = c.x; x < c.x + BiomeManager.chunkSize; x++)
        for (int y = c.y; y > c.y - BiomeManager.chunkSize; y--)
        {

            if (y > 0) continue;

            if (FileManager.isDestroyed(x, y))
            {
                addBackground(x, y);
                continue;
            }

                GameObject.Instantiate(this.getBlockAt(x, y), new Vector3(x, y, 0), Quaternion.identity);
            }
        //Debug.Log("time to generate chunk: " + c.x + ", " + c.y + ": " + (System.DateTime.Now - before).TotalMilliseconds);
    }

    public override float getTemperature(float depth)
    {
        return depth / 2 + 70;
    }

    public override float getPressure(float depth)
    {
        return depth / 4 + 50;
    }

}

public class Base : Biome
{

    public Base() : base(){}

    public override GameObject getBlockAt(int x, int y)
    {
        y = -y;
        
        h.setNewHash(x, y, MapGeneratorScript.seed);

        if (y == 0) return grass;
        if (y < 3) return dirt;
        else if (y == 3 && (h.v % 2 == 0 || h.v % 3 == 0)) return dirt;
        else if ((y == 4 || y == 5) && h.v % 2 == 0) return dirt;
        else if (h.v < 10000) return gold;
        else return stone;
    }

    public override void actuallyGenerateChunk(Coordinate c)
    {
        //DateTime before = System.DateTime.Now;

        for (int x = c.x; x < c.x + BiomeManager.chunkSize; x++)
        for (int y = c.y; y > c.y - BiomeManager.chunkSize; y--)
        {

            if (y > 0) continue;

                if (FileManager.isDestroyed(x, y))
                {
                    addBackground(x, y); 
                    continue;
                }

                GameObject.Instantiate(this.getBlockAt(x, y), new Vector3(x, y, 0), Quaternion.identity);
            }
        //Debug.Log("time to generate chunk: " + c.x + ", " + c.y + ": " + (System.DateTime.Now - before).TotalMilliseconds);
    }

    public override float getTemperature(float depth)
    {
        return depth / 2 + 50;
    }

    public override float getPressure(float depth)
    {
        return depth / 4 + 50;
    }


}


public class Hash /////////////////////////////////////////////////
{

    public int v;
    public float more50;
    public float less50;
    public bool shouldBeTrulyRandom = false;

    public void setNewHash(int x, int y, int seed)
    {

        double x1 = Math.Pow(x + 1, 19f / 59f);
        double y1 = Math.Pow(y + 1, 41f / 59f);
        double big = x1 * y1 * Math.Pow(seed, 31f / 59f) * Mathf.PI;

        v = (int)(100000 * new Random((int)((big - Math.Floor(big)) * 2100000000)).NextDouble());

    }

}

