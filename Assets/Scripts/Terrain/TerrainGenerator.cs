using System;
using UnityEngine;
using Mirror;

/*
 * Adapted from https://gamedevacademy.org/complete-guide-to-procedural-level-generation-in-unity-part-1/
 */

public class TerrainGenerator : NetworkBehaviour
{
    [SerializeField] GameObject tilePrefab = null;
    [SerializeField] int widthInTiles = 10;
    [SerializeField] int depthInTiles = 10;
    public float noiseScale = 10;
    public float heightMultiplier = 2;
    [SyncVar]
    public int seed = 0;
    public Wave[] waves;
    [SerializeField] TerrainType[] terrainTypes = new TerrainType[] {};

    void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        // get the tile dimensions from the tile Prefab
        Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;
        int tileWidth = (int)tileSize.x;
        int tileDepth = (int)tileSize.z;

        // for each Tile, instantiate a Tile in the correct position
        for (int x = 0; x < widthInTiles; x++) {
            for (int z = 0; z < depthInTiles; z++) {
                Vector3 tilePosition = new Vector3(
                    gameObject.transform.position.x + x * tileWidth, 
                    gameObject.transform.position.y, 
                    gameObject.transform.position.z + z * tileDepth
                );

                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;
                tile.name = tilePrefab.name + " (" + x.ToString() + "," + z.ToString() + ")";
                tile.transform.SetParent(gameObject.transform);

                TileGenerator generator = tile.GetComponent<TileGenerator>();
                generator.GenerateTile(this);
            }
        }
    }

    public TerrainType ChooseTerrainType(float height)
    {
        foreach(TerrainType tt in terrainTypes) {
            if (height <= tt.height) return tt;
        }

        return terrainTypes[terrainTypes.Length-1];
    }

}
