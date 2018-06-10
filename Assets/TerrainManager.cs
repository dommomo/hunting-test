using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour {

    public enum Terrain { Dirt, Rock, Tree, Plant}
    public TerrainType[] terrainTypes;
    public string seed;
    public int[,] terrain;

    private SpriteRenderer[,] renderers;
    private int height = 250;
    private int width = 250;

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
        RockGenerator rocks = new RockGenerator();
        terrain = rocks.GetMap(wallsFilled, seed);
    }

    private void AttachRenderersAndDraw()
    {
        int sortIndex = 0;
        var offset = new Vector3(0, 0, 0);
        renderers = new SpriteRenderer[height, width];
        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
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
        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                var spriteRenderer = renderers[x, y];
                //var terrain = SelectTerrain(
                //    offset.x + x,
                //    offset.y + y);
                //spriteRenderer.sprite = terrain.GetTile(offset.x + x, offset.y + y, seed);
                //var animator = spriteRenderer.gameObject.GetComponent<Animator>();
                //if (terrain.IsAnimated)
                //{
                //    if (animator == null)
                //    {
                //        animator = spriteRenderer.gameObject.AddComponent<Animator>();
                //        animator.runtimeAnimatorController = terrain.animationController;
                //    }
                //}
                //else
                //{
                //    if (animator != null)
                //    {
                //        GameObject.Destroy(animator);
                //    }
                //}
            }
        }
    }
}
