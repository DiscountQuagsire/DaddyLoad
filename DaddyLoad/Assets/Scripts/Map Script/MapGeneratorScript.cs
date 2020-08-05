using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

public class MapGeneratorScript : MonoBehaviour
{
    public int seed;

    public int chunkSize = 10;
    public int sightHalfWidth = 23;
    public int sightHalfHeight = 12;

    public ArrayList loadedChunkCoordinates = new ArrayList();
    FileManager fm;
    BiomeManager bm;
    public Inventory localInventory = new Inventory();
    public Inventory globalInventory = new Inventory();

    public void Start()
    {
        fm = GameObject.Find("FileManager").GetComponent<FileManager>();
        bm = new BiomeManager(fm);
        bm.setSeed(seed);
    }

    public void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            localInventory.listInventory();
            globalInventory.listInventory();
        }

        if (Input.GetKeyDown("h"))
        {
            Debug.Log("Local to global; updating for everyone");
            fm.moveLocalInventoryToGlobalInventory();
            fm.updateGlobalInventoryForEveryone();
        }

        if (Input.GetKeyDown("k"))
        {
            fm.writeUnwrittenBlocksToFile();
        }


    }

    int index = 0;
    public void FixedUpdate()
    {
        index++;

        if (index % 5 == 0)
        {
            this.generateNearbyUnloadedChunks();
        }
        if (index % 50 == 0)
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
            {                                                                                   // jestli jsem nic neposral
                this.removeChunkAtNew(c);
                loadedChunkCoordinates.Remove(c);
                i--;
            }
        }
    }

    public void generateNearbyUnloadedChunks()
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
                bm.getBiomeAt(possibleX).actuallyGenerateChunk(c);
                return;
            }

        }
    }

    public void removeChunkAtNew(Coordinate c)
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
                Destroy(blocks[i]);
                destroyedCount++;
                if (destroyedCount >= chunkSize * chunkSize) break;
            }
        }

        //Debug.Log("time to unload chunk using new: " + c.x + ", " + c.y + ": " + (System.DateTime.Now - before).TotalMilliseconds);
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

    public void setSeed(int newSeed)
    {
        bm.seed = newSeed;
    }
}
public class BiomeManager
{

    public Desert desert;
    public Base realBase; // base je keyword takze to nemuzu pouzit lmao
    public Hash h;
    public int seed;
    public int chunkSize = 10;
    public FileManager fm;

    public BiomeManager(FileManager fm)
    {
        h = new Hash();

        //h.setNewHash(589, 154, 12345);


        this.fm = fm;
        desert = new Desert(this, fm);
        realBase = new Base(this, fm);
    }

    public Biome getBiomeAt(int x)
    {
        if (x >= 0 && x < 20)
            return realBase;
        else
            return desert;
    }

    public void setSeed(int newSeed)
    {
        seed = newSeed;
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

    public BiomeManager bm;
    public FileManager fm;

    public Biome(BiomeManager bm, FileManager fm)
    {

        this.bm = bm;
        this.fm = fm;

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
}

public class Desert : Biome
{

    public Dictionary<GameObject, int> count = new Dictionary<GameObject, int>();

    public Desert(BiomeManager bm, FileManager fm) :  base(bm, fm) { }

    public override GameObject getBlockAt(int x, int y)
    {
        y = -y;

        if (y < 3) return sand;

        bm.h.setNewHash(x, y, bm.seed);

        if (y == 3 && (bm.h.v % 2 == 0 || bm.h.v % 3 == 0)) return sand;



        else if (y >= 450 && y < 500 && bm.h.v >= 0 && bm.h.v < 1000) return diamond;
        else if (y >= 400 && y < 500 && bm.h.v >= 1000 && bm.h.v < 2000) return titanium;
        else if (y >= 350 && y < 500 && bm.h.v >= 2000 && bm.h.v < 3000) return iridium;
        else if (y >= 300 && y < 500 && bm.h.v >= 3000 && bm.h.v < 4000) return platinum;
        else if (y >= 250 && y < 500 && bm.h.v >= 4000 && bm.h.v < 5000) return copper;
        else if (y >= 150 && y < 350 && bm.h.v >= 5000 && bm.h.v < 6500) return gold;
        else if (y >= 150 && y < 180 && bm.h.v >= 6500 && bm.h.v < 8000) return lead;
        else if (y >= 100 && y < 300 && bm.h.v >= 8000 && bm.h.v < 9500) return silver;
        else if (y >= 50 && y < 150 && bm.h.v >= 9500 && bm.h.v < 11000) return iron;
        else if (y >= 40 && y < 150 && bm.h.v >= 11000 && bm.h.v < 12500) return aluminum;
        else if (y >= 10 && y < 120 && bm.h.v >= 12500 && bm.h.v < 15000) return plastic;
        else if (y >= 10 && y < 120 && bm.h.v >= 15000 && bm.h.v < 17500) return bronze;

        return sandstone;

       
    }

    public override void actuallyGenerateChunk(Coordinate c)
    {
        //DateTime before = System.DateTime.Now;

        for (int x = c.x; x < c.x + bm.chunkSize; x++)
        for (int y = c.y; y > c.y - bm.chunkSize; y--)
        {

            if (y > 0) continue;

            if (fm.isDestroyed(x, y))
            {
                GameObject.Instantiate(background1, new Vector3(x, y, 0), Quaternion.identity);
                continue;
            }

                GameObject.Instantiate(this.getBlockAt(x, y), new Vector3(x, y, 0), Quaternion.identity);
            }
        //Debug.Log("time to generate chunk: " + c.x + ", " + c.y + ": " + (System.DateTime.Now - before).TotalMilliseconds);
    }

}

public class Base : Biome
{

    public Base(BiomeManager bm, FileManager fm) : base(bm, fm){}

    public override GameObject getBlockAt(int x, int y)
    {
        y = -y;
        
        bm.h.setNewHash(x, y, bm.seed);

        if (y == 0) return grass;
        if (y < 3) return dirt;
        else if (y == 3 && (bm.h.v % 2 == 0 || bm.h.v % 3 == 0)) return dirt;
        else if ((y == 4 || y == 5) && bm.h.v % 2 == 0) return dirt;
        else if (bm.h.v < 10000) return gold;
        else return stone;
    }

    public override void actuallyGenerateChunk(Coordinate c)
    {
        //DateTime before = System.DateTime.Now;

        for (int x = c.x; x < c.x + bm.chunkSize; x++)
        for (int y = c.y; y > c.y - bm.chunkSize; y--)
        {

            if (y > 0) continue;

                if (fm.isDestroyed(x, y))
                {
                    GameObject.Instantiate(background1, new Vector3(x, y, 0), Quaternion.identity);
                    continue;
                }

                GameObject.Instantiate(this.getBlockAt(x, y), new Vector3(x, y, 0), Quaternion.identity);
            }
        //Debug.Log("time to generate chunk: " + c.x + ", " + c.y + ": " + (System.DateTime.Now - before).TotalMilliseconds);
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

