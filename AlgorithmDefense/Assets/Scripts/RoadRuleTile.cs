using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "2D/Tiles/Custom Tiles/RoadRuleTile")]
public class RoadRuleTile : RuleTile<RoadRuleTile.Neighbor>
{
    public bool customField;

    private Vector3Int _thisPos;
    private Vector3Int _neighborPos;

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int SameGroup = 3;
    }

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        switch (neighbor)
        {
            case Neighbor.This: return IsSameGroup(tile);
            case Neighbor.NotThis: return !IsSameGroup(tile);
        }

        return base.RuleMatch(neighbor, tile);
    }

    public override Vector3Int GetOffsetPosition(Vector3Int position, Vector3Int offset)
    {
        _thisPos = position;
        _neighborPos = position + offset;
        return base.GetOffsetPosition(position, offset);
    }

    bool IsSameGroup(TileBase tile)
    {
        // 건물 체크.
        var building = Managers.Tile.GetTilemap(Define.Tilemap.Building).GetInstantiatedObject(_neighborPos);
        if (building)
        {
            return true;
        }

        // 길 체크.
        var thisGo = Managers.Tile.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(_thisPos);
        var neighborGo = Managers.Tile.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(_neighborPos);
        if (!thisGo || !neighborGo)
        {
            return false;
        }

        if (TileObjectBuilder.GetInstance.RoadGroupNumberDatas.ContainsKey(_thisPos) &&
            TileObjectBuilder.GetInstance.RoadGroupNumberDatas.ContainsKey(_neighborPos))
        {
            return TileObjectBuilder.GetInstance.RoadGroupNumberDatas[_thisPos] ==
                TileObjectBuilder.GetInstance.RoadGroupNumberDatas[_neighborPos];
        }

        return false;
    }
}