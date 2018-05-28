using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    [Range(0,100)]
    public int randomFillPercent;

    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;
    public bool allWallsFilled;
    public bool filledTopLeft, filledTopRight, filledRightTop, filledRightBottom, filledBottomRight, filledBottomLeft, filledLeftBottom, filledLeftTop;
    public int filledEdgeThickness = 2;
    public int smoothingIterations = 5;
    public const int smoothPivot = 4;

    int[,] map;

    void Start()
    {
        GenerateMap();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerateMap();
        }
    }

    void GenerateMap()
    {
        map = new int[width,height];
        RandomFillMap();

        for (int i = 0; i < smoothingIterations; i++)
        {
            SmoothMap();
        }
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random psuedoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = (psuedoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                if (allWallsFilled)
                {
                    if (x == 0 || x == width-1 || y == 0 || y == height-1)
                    {
                        map[x, y] = 1;
                    }
                }

                //ugly but will do for now - clockwise from top of screen left side
                if(filledTopLeft)
                {
                    if (y >= height - filledEdgeThickness && x < (width/2) - 1)
                        map[x, y] = 1;
                }
                if (filledTopRight)
                {
                    if (y >= height - filledEdgeThickness && x > (width / 2) - 1)
                        map[x, y] = 1;
                }
                if (filledRightTop)
                {
                    if (x >= width - filledEdgeThickness && y > (height / 2) - 1)
                        map[x, y] = 1;
                }
                if (filledRightBottom)
                {
                    if (x >= width - filledEdgeThickness && y < (height / 2) - 1)
                        map[x, y] = 1;
                }
                if (filledBottomRight)
                {
                    if (y < filledEdgeThickness && x > (width / 2) - 1)
                        map[x, y] = 1;
                }
                if (filledBottomLeft)
                {
                    if (y < filledEdgeThickness && x < (width / 2) - 1)
                        map[x, y] = 1;
                }
                if (filledLeftBottom)
                {
                    if (x < filledEdgeThickness && y < (height / 2) - 1)
                        map[x, y] = 1;
                }
                if (filledLeftTop)
                {
                    if (x < filledEdgeThickness && y > (height / 2) - 1)
                        map[x, y] = 1;
                }

            }
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > smoothPivot)
                    map[x, y] = 1;
                else if (neighbourWallTiles < smoothPivot)
                    map[x, y] = 0;
                //else leave as is if equal?
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX +1; neighbourX++) //tile grid 3x3 surrounding self
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >=0 && neighbourY < height) //avoid out of bounds tiles
                {
                    if (neighbourX != gridX || neighbourY != gridY) //avoid middle tile (self)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    if (allWallsFilled)
                    {
                        wallCount++;    //encourage growth of walls around the map if desired
                    }
                }
            }
        }

        return wallCount;
    }

    private void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                    //Vector3 pos = new Vector3(-width/2 + x + 0.5f, 0 , -height/2 + y + 0.5f); //orig course---but we want x and y not x and z
                    Vector3 pos = new Vector3(-width / 2 + x + 0.5f, -height / 2 + y + 0.5f, 0);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
}
