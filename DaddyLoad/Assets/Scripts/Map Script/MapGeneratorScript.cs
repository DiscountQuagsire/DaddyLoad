using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using Photon.Pun;
using UnityEngine;

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
            Debug.Log("Moving local inventory to global inventory");
            fm.moveLocalInventoryToGlobalInventory();
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

    public GameObject iron;
    public GameObject gold;
    public GameObject copper;
    public GameObject diamond;
    public GameObject emerald;

    public GameObject GameObject;

    public GameObject error;

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

        iron =      (GameObject)Resources.Load("Iron Variant",      typeof(GameObject));
        gold =      (GameObject)Resources.Load("Gold Variant",      typeof(GameObject));
        copper =    (GameObject)Resources.Load("Copper Variant",    typeof(GameObject));
        diamond =   (GameObject)Resources.Load("Diamond Variant",   typeof(GameObject));
        emerald =   (GameObject)Resources.Load("Emerald Variant",   typeof(GameObject));
        
        error =     (GameObject)Resources.Load("Error Variant",     typeof(GameObject));


    }

    public abstract GameObject getBlockAt(int x, int y);
    public abstract void actuallyGenerateChunk(Coordinate c);
}

public class Desert : Biome
{

    public Desert(BiomeManager bm, FileManager fm) :  base(bm, fm)
    {
        Debug.Log("Desert biome created; dirt = " + dirt);
    }

    public override GameObject getBlockAt(int x, int y)
    {
        y = -y;

        if (y < 3) return sand;
        bm.h.setNewHash(x, y, bm.seed);
        
        if (y == 3 && (bm.h.v % 2 == 0 || bm.h.v % 3 == 0)) return sand;
        else if ((y == 4 || y == 5) && bm.h.v % 2 == 0) return sand;
        else if (bm.h.v < 10000) return copper;
        else if (bm.h.v < 20000) return diamond;
        else if (bm.h.v < 30000) return iron;
        else if (bm.h.v < 40000) return emerald;

        else return sandstone;
    }

    public override void actuallyGenerateChunk(Coordinate c)
    {
        DateTime before = System.DateTime.Now;

        for (int x = c.x; x < c.x + bm.chunkSize; x++)
        for (int y = c.y; y > c.y - bm.chunkSize; y--)
        {

            if (y > 0) continue;
            if (fm.isDestroyed(x, y)) continue;

                GameObject.Instantiate(this.getBlockAt(x, y), new Vector3(x, y, 0), Quaternion.identity);
            }
        //Debug.Log("time to generate chunk: " + c.x + ", " + c.y + ": " + (System.DateTime.Now - before).TotalMilliseconds);
    }
}

public class Base : Biome
{

    public Base(BiomeManager bm, FileManager fm) : base(bm, fm)
    {
        dirt = (GameObject)Resources.Load("Dirt Variant", typeof(GameObject));
        Debug.Log("base (really base) biome created");
    }

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

        //return null;
    }

    public override void actuallyGenerateChunk(Coordinate c)
    {
        DateTime before = System.DateTime.Now;

        for (int x = c.x; x < c.x + bm.chunkSize; x++)
        for (int y = c.y; y > c.y - bm.chunkSize; y--)
        {

            if (y > 0) continue;

            if (fm.isDestroyed(x, y)) continue;

                GameObject.Instantiate(this.getBlockAt(x, y), new Vector3(x, y, 0), Quaternion.identity);
            }
        //Debug.Log("time to generate chunk: " + c.x + ", " + c.y + ": " + (System.DateTime.Now - before).TotalMilliseconds);
    }
}


public class Hash /////////////////////////////////////////////////
{

    public int v;

    public void setNewHash(int x, int y, int seed)
    {
        seed = 12345;
        double x1 = Math.Pow(x+1, 19f / 59f);
        double y1 = Math.Pow(y+1, 41f / 59f);
        double rootSeed = Math.Pow(seed, 31 / 59f);

        double big = x1 * y1 * rootSeed;
        double cut = big - Math.Floor(big);

        v = (int)Math.Floor(cut * 100000);

        if (v == 0) v = -1;
    }
}

