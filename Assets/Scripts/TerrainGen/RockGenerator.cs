using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtDawn.TerrainGen
{
    public class RockGenerator : MonoBehaviour
    {
        [Range(0, 100)]
        public int randomFillPercentInitial;
        [Range(0, 100)]
        public int randomFillPercentPostStretch;

        public int initialWidth;
        public int initialHeight;

        public int currentWidth;
        public int currentHeight;

        private string seed;
        public bool allWallsFilled;
        public bool filledTopLeft, filledTopRight, filledRightTop, filledRightBottom, filledBottomRight, filledBottomLeft, filledLeftBottom, filledLeftTop;
        public int filledEdgeThickness = 2;
        public int preStretchSmoothingIterations = 2;
        public int stretchMulti = 5;
        public int postStretchSmoothingIterations = 1;
        public const int smoothPivot = 4;

        private int[,] map;

        public int[,] GetMap()
        {
            GenerateMap();
            return map;
        }

        public int[,] GetMap(int[] filledPanels, string seed)
        {
            this.seed = seed;
            allWallsFilled = false;
            filledTopLeft = filledPanels[0] > 0 ? true : false;
            filledTopRight = filledPanels[1] > 0 ? true : false;
            filledRightTop = filledPanels[2] > 0 ? true : false;
            filledRightBottom = filledPanels[3] > 0 ? true : false;
            filledBottomRight = filledPanels[4] > 0 ? true : false;
            filledBottomLeft = filledPanels[5] > 0 ? true : false;
            filledLeftBottom = filledPanels[6] > 0 ? true : false;
            filledLeftTop = filledPanels[7] > 0 ? true : false;
            GenerateMap();
            return map;
        }

        private void GenerateMap()
        {
            map = new int[initialWidth, initialHeight];
            currentWidth = initialWidth;
            currentHeight = initialHeight;
            RandomFillMapInitial();

            //initial map smoothing
            for (int i = 0; i < preStretchSmoothingIterations; i++)
            {
                SmoothMap();
            }

            StretchMap(stretchMulti);
            RandomFillMapPostStretch();

            //more smooth after stretch to get rid of blocky resolution
            for (int i = 0; i < postStretchSmoothingIterations; i++)
            {
                SmoothMap();
            }
        }

        private void RandomFillMapPostStretch()
        {

            for (int x = 0; x < currentWidth; x++)
            {
                for (int y = 0; y < currentHeight; y++)
                {
                    if ((map[x, y]) == (int)TypeOfTerrain.Dirt)
                    {
                        map[x, y] = (int)((RandomHelper.PercentAsInt(x, y, seed.GetHashCode()) < randomFillPercentPostStretch) ? TypeOfTerrain.Rock : TypeOfTerrain.Dirt);
                    }
                }
            }
        }

        private void StretchMap(int stretchMulti)
        {
            int[,] oldMap = map;
            int stretchedWidth = initialWidth * stretchMulti;
            int stretchedHeight = initialHeight * stretchMulti;
            map = new int[stretchedWidth, stretchedHeight];

            for (int x = 0; x < initialWidth; x++)
            {
                for (int y = 0; y < initialHeight; y++)
                {
                    int val = oldMap[x, y];
                    for (int xStretch = x * stretchMulti; xStretch < ((x + 1) * stretchMulti); xStretch++)
                    {
                        for (int yStretch = y * stretchMulti; yStretch < ((y + 1) * stretchMulti); yStretch++)
                        {
                            map[xStretch, yStretch] = val;
                        }
                    }
                }
            }

            currentWidth = stretchedWidth;
            currentHeight = stretchedHeight;
        }

        private void RandomFillMapInitial()
        {
            for (int x = 0; x < initialWidth; x++)
            {
                for (int y = 0; y < initialHeight; y++)
                {
                    map[x, y] = (int)((RandomHelper.PercentAsInt(x, y, seed.GetHashCode()) < randomFillPercentInitial) ? TypeOfTerrain.Rock : TypeOfTerrain.Dirt);
                    if (allWallsFilled)
                    {
                        if (x == 0 || x == initialWidth - 1 || y == 0 || y == initialHeight - 1)
                        {
                            map[x, y] = (int)TypeOfTerrain.Rock;
                        }
                    }

                    //ugly but will do for now - clockwise from top of screen left side
                    if (filledTopLeft)
                    {
                        if (y >= initialHeight - filledEdgeThickness && x < (initialWidth / 2) - 1)
                            map[x, y] = (int)TypeOfTerrain.Rock;
                    }
                    if (filledTopRight)
                    {
                        if (y >= initialHeight - filledEdgeThickness && x > (initialWidth / 2) - 1)
                            map[x, y] = (int)TypeOfTerrain.Rock;
                    }
                    if (filledRightTop)
                    {
                        if (x >= initialWidth - filledEdgeThickness && y > (initialHeight / 2) - 1)
                            map[x, y] = (int)TypeOfTerrain.Rock;
                    }
                    if (filledRightBottom)
                    {
                        if (x >= initialWidth - filledEdgeThickness && y < (initialHeight / 2) - 1)
                            map[x, y] = (int)TypeOfTerrain.Rock;
                    }
                    if (filledBottomRight)
                    {
                        if (y < filledEdgeThickness && x > (initialWidth / 2) - 1)
                            map[x, y] = (int)TypeOfTerrain.Rock;
                    }
                    if (filledBottomLeft)
                    {
                        if (y < filledEdgeThickness && x < (initialWidth / 2) - 1)
                            map[x, y] = (int)TypeOfTerrain.Rock;
                    }
                    if (filledLeftBottom)
                    {
                        if (x < filledEdgeThickness && y < (initialHeight / 2) - 1)
                            map[x, y] = (int)TypeOfTerrain.Rock;
                    }
                    if (filledLeftTop)
                    {
                        if (x < filledEdgeThickness && y > (initialHeight / 2) - 1)
                            map[x, y] = (int)TypeOfTerrain.Rock;
                    }

                }
            }
        }

        private void SmoothMap()
        {
            for (int x = 0; x < currentWidth; x++)
            {
                for (int y = 0; y < currentHeight; y++)
                {
                    int neighbourWallTiles = GetSurroundingWallCount(x, y);

                    if (neighbourWallTiles > smoothPivot)
                        map[x, y] = (int)TypeOfTerrain.Rock;
                    else if (neighbourWallTiles < smoothPivot)
                        map[x, y] = (int)TypeOfTerrain.Dirt;
                    //else leave as is if equal?
                }
            }
        }

        int GetSurroundingWallCount(int gridX, int gridY)
        {
            int wallCount = 0;
            for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) //tile grid 3x3 surrounding self
            {
                for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
                {
                    if (neighbourX >= 0 && neighbourX < currentWidth && neighbourY >= 0 && neighbourY < currentHeight) //avoid out of bounds tiles
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

        //private void OnDrawGizmos()
        //{
        //    if (map != null)
        //    {
        //        for (int x = 0; x < currentWidth; x++)
        //        {
        //            for (int y = 0; y < currentHeight; y++)
        //            {
        //                Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
        //                //Vector3 pos = new Vector3(-width/2 + x + 0.5f, 0 , -height/2 + y + 0.5f); //orig course---but we want x and y not x and z
        //                Vector3 pos = new Vector3(-currentWidth / 2 + x + 0.5f, -currentHeight / 2 + y + 0.5f, 0);
        //                Gizmos.DrawCube(pos, Vector3.one);
        //            }
        //        }
        //    }
        //}
    }
}
