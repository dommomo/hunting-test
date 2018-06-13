using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtDawn.TerrainGen
{
    public class FloraGenerator : MonoBehaviour
    {
        [Range(0,100)]
        public int fillPercentTrees;
        [Range(0, 100)]
        public int fillPercentPlants;


        private string seed;

        public int[,] AddFloraToMap(int[,] map, string seed)
        {
            this.seed = seed;
            int[,] mapToReturn = this.AddTrees(map);
            mapToReturn = this.AddPlants(map);

            return mapToReturn;
        }

        private int[,] AddPlants(int[,] map)
        {
            int[,] mapToReturn = map;
            for (int x = 0; x < mapToReturn.GetLength(0); x++)
            {
                for (int y = 0; y < mapToReturn.GetLength(1); y++)
                {
                    if ((map[x, y]) == (int)TypeOfTerrain.Dirt)
                    {
                        map[x, y] = (int)((RandomHelper.PercentAsInt(x, y, seed.GetHashCode()) < fillPercentPlants) ? TypeOfTerrain.Plant : TypeOfTerrain.Dirt);
                    }
                }
            }

            return mapToReturn;
        }

        private int[,] AddTrees(int[,] map)
        {
            int[,] mapToReturn = map;
            for (int x = 0; x < mapToReturn.GetLength(0); x++)
            {
                for (int y = 0; y < mapToReturn.GetLength(1); y++)
                {
                    if ((map[x, y]) == (int)TypeOfTerrain.Dirt)
                    {
                        map[x, y] = (int)((RandomHelper.PercentAsInt(x, y, seed.GetHashCode()) < fillPercentTrees) ? TypeOfTerrain.Tree : TypeOfTerrain.Dirt);
                    }
                }
            }

            return mapToReturn;
        }
    }
}
