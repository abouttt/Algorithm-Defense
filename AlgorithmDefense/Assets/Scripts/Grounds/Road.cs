using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [field: SerializeField]
    public Define.Road RoadType { get; private set; }
    public int GroupNumber;

    private void Start()
    {
        var cellPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Road, transform.position);
        if (TileObjectBuilder.GetInstance.RoadGroupNumberDatas.ContainsKey(cellPos))
        {
            GroupNumber = TileObjectBuilder.GetInstance.RoadGroupNumberDatas[cellPos];
        }

        Managers.Tile.GetTilemap(Define.Tilemap.Road).RefreshTile(cellPos);
    }
}
