using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TerrainType {

    public string terrainName;
    public bool walkable;
    public Sprite[] tiles;

    public Sprite GetTile(Vector2 pos, string seed)
    {
        return tiles[RandomHelper.Range(pos, seed.GetHashCode(), tiles.Length)];
    }

}
