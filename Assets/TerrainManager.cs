using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour {

    public enum Terrain { Dirt, Rock, Tree, Plant}
    public TerrainType[] terrainTypes;
    public string seed;
    public int[,] map;

    private SpriteRenderer[,] renderers;
    private int mapHeight = 250;
    private int mapWidth = 250;

	// Use this for initialization
	void Start () {
        GenerateTerrain();
        AttachRenderersAndDraw();
	}

    // Update is called once per frame
    void Update () {
		
	}

    private void GenerateTerrain()
    {
        int[] wallsFilled = { 0, 0, 0, 0, 0, 0, 1, 1 };
        RockGenerator rocks = transform.gameObject.AddComponent<RockGenerator>();
        map = rocks.GetMap(wallsFilled, seed);
    }

    private void AttachRenderersAndDraw()
    {
        int sortIndex = 0;
        var offset = new Vector3(0, 0, 0);
        renderers = new SpriteRenderer[mapHeight, mapWidth];
        for (int x = 0; x < mapHeight; x++)
        {
            for (int y = 0; y < mapWidth; y++)
            {
                var tile = new GameObject();
                tile.transform.position = new Vector3(x, y, 0) + offset;
                var renderer = renderers[x, y] = tile.AddComponent<SpriteRenderer>();
                renderer.sortingOrder = sortIndex--;
                tile.name = "Terrain " + tile.transform.position;
                tile.transform.parent = transform;
            }
        }

        RedrawMap();
    }

    private void RedrawMap()
    {
        var offset = new Vector3(0,0,0);
        for (int x = 0; x < mapHeight; x++)
        {
            for (int y = 0; y < mapWidth; y++)
            {
                var spriteRenderer = renderers[x, y];
                var terrain = SelectTerrain(
                    (int)offset.x + x,
                    (int)offset.y + y);
                spriteRenderer.sprite = terrain.GetTile(new Vector2(offset.x + x, offset.y + y), seed);
            }
        }
    }

    private TerrainType SelectTerrain(int x, int y)
    {
        //Debug.Log(map.GetLength(0) + " " + map.GetLength(1));
        //return terrainTypes[map[x,y]];
        return terrainTypes[0];
    }
}
