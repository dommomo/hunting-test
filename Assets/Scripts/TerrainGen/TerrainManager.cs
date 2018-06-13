using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtDawn.TerrainGen
{
    public class TerrainManager : MonoBehaviour
    {
        public TerrainType[] terrainTypes;
        public string seed;
        public int[,] map;

        private SpriteRenderer[,] renderers;
        private int mapHeight = 250;
        private int mapWidth = 250;

        // Use this for initialization
        void Start()
        {
            GenerateMap();
            AttachRenderersAndDraw();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void GenerateMap()
        {
            GenerateTerrain();
            GenerateVegetationLayer();
        }

        private void GenerateVegetationLayer()
        {
            FloraGenerator flora = transform.gameObject.GetComponentInChildren<FloraGenerator>();
            map = flora.AddFloraToMap(map, seed);
        }

        private void GenerateTerrain()
        {
            RockGenerator rocks = transform.gameObject.GetComponentInChildren<RockGenerator>();
            //int[] wallsFilled = { 0, 0, 0, 0, 0, 0, 1, 1 };
            //map = rocks.GetMap(wallsFilled, seed);
            map = rocks.GetMap(seed);
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
                    renderer.sortingLayerName = "Terrain";
                    renderer.sortingOrder = sortIndex--;
                    tile.name = "Terrain " + tile.transform.position;
                    tile.transform.parent = transform;
                }
            }

            RedrawMap();
        }

        private void RedrawMap()
        {
            for (int x = 0; x < mapHeight; x++)
            {
                for (int y = 0; y < mapWidth; y++)
                {
                    var spriteRenderer = renderers[x, y];
                    var terrain = GetTerrainType(x, y);
                    spriteRenderer.sprite = terrain.GetTile(new Vector2(x, y), seed);
                }
            }
        }

        public TerrainType GetTerrainType(int xCoord, int yCoord)
        {
            return terrainTypes[map[xCoord, yCoord]];
        }

        public TerrainType GetTerrainType(int xCoord, int yCoord, int[,] myMap)
        {
            return terrainTypes[myMap[xCoord, yCoord]];
        }
    }
}
