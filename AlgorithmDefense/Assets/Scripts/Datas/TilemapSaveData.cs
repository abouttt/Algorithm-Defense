using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TilemapSaveData
{
    public Define.Tilemap Tilemap;
    public List<TileBase> Tiles = new List<TileBase>();
    public List<Vector3Int> CellPoses = new List<Vector3Int>();
}
