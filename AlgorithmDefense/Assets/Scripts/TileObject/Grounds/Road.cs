using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Road : MonoBehaviour
{
    [field: SerializeField]
    public Define.Road RoadType { get; private set; }

    Vector3Int? _frontNeighborPos = null;
    Vector3Int? _prevNeighborPos = null;

    private static Define.Road s_point;

    public void RefreshTile(Vector3Int cellPos)
    {
        s_point = RoadType;

        Rule(cellPos, isSelf: true);

        if (_frontNeighborPos.HasValue)
        {
            var go = Managers.Tile.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(_frontNeighborPos.Value);
            if (go)
            {
                go.GetComponent<Road>().Rule(_frontNeighborPos.Value, isSelf: false);
            }
        }

        if (_prevNeighborPos.HasValue)
        {
            var go = Managers.Tile.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(_prevNeighborPos.Value);
            if (go)
            {
                go.GetComponent<Road>().Rule(_prevNeighborPos.Value, isSelf: false);
            }
        }

        _frontNeighborPos = null;
        _prevNeighborPos = null;
    }

    private void Rule(Vector3Int cellPos, bool isSelf)
    {
        if (RoadType != Define.Road.B &&
            RoadType != Define.Road.BU &&
            RoadType != Define.Road.BD &&
            RoadType != Define.Road.BL &&
            RoadType != Define.Road.BR)
        {
            return;
        }

        Tile nextTile = null;
        TileBase neighborTile = null;

        // 주변 막다른 길 찾기
        for (int i = 0; i < 4; i++)
        {
            int nx = cellPos.x + Define.DX[i];
            int ny = cellPos.y + Define.DY[i];

            _frontNeighborPos = new Vector3Int(nx, ny, 0);

            // 막다른 길인지 평가.
            var go = Managers.Tile.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(_frontNeighborPos.Value);
            if (!go)
            {
                continue;
            }

            var neighborRoad = go.GetComponent<Road>();
            if (neighborRoad)
            {
                if (isSelf)
                {
                    if (neighborRoad.RoadType == Define.Road.B ||
                    neighborRoad.RoadType == Define.Road.BU ||
                    neighborRoad.RoadType == Define.Road.BD ||
                    neighborRoad.RoadType == Define.Road.BL ||
                    neighborRoad.RoadType == Define.Road.BR)
                    {
                        neighborTile = Managers.Tile.GetTile(Define.Tilemap.Road, _frontNeighborPos.Value);
                        break;
                    }
                    else
                    {
                        neighborTile = null;
                    }
                }
                else
                {
                    neighborTile = Managers.Tile.GetTile(Define.Tilemap.Road, _frontNeighborPos.Value);
                    break;
                }
            }
        }

        if (neighborTile)
        {
            var gapPos = cellPos - _frontNeighborPos.Value;

            // 첫번째 조건문 : 상대 타일이 방향의 반대방향 (예) Vector3Int.up = 아래.
            // 두번재 조건문 : 또 다른 상대 타일이 방향의 정방향.
            if (gapPos == Vector3Int.up)
            {
                if (Managers.Tile.GetTile(Define.Tilemap.Road, cellPos + Vector3Int.up) ||
                    Managers.Tile.GetTile(Define.Tilemap.Building, cellPos + Vector3Int.up))
                {
                    _prevNeighborPos = cellPos + Vector3Int.up;
                    nextTile = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_UD");
                }
                else if (Managers.Tile.GetTile(Define.Tilemap.Road, cellPos + Vector3Int.right) ||
                    Managers.Tile.GetTile(Define.Tilemap.Building, cellPos + Vector3Int.right))
                {
                    _prevNeighborPos = cellPos + Vector3Int.right;
                    nextTile = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_CUR");
                }
                else if (Managers.Tile.GetTile(Define.Tilemap.Road, cellPos + Vector3Int.left) ||
                    Managers.Tile.GetTile(Define.Tilemap.Building, cellPos + Vector3Int.left))
                {
                    _prevNeighborPos = cellPos + Vector3Int.left;
                    nextTile = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_CUL");
                }
                else
                {
                    nextTile = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_BD");
                }
            }
            else if (gapPos == Vector3Int.down)
            {
                if (Managers.Tile.GetTile(Define.Tilemap.Road, cellPos + Vector3Int.down) ||
                    Managers.Tile.GetTile(Define.Tilemap.Building, cellPos + Vector3Int.down))
                {
                    _prevNeighborPos = cellPos + Vector3Int.down;
                    nextTile = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_UD");
                }
                else if (Managers.Tile.GetTile(Define.Tilemap.Road, cellPos + Vector3Int.right) ||
                    Managers.Tile.GetTile(Define.Tilemap.Building, cellPos + Vector3Int.right))
                {
                    _prevNeighborPos = cellPos + Vector3Int.right;
                    nextTile = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_CDR");
                }
                else if (Managers.Tile.GetTile(Define.Tilemap.Road, cellPos + Vector3Int.left) ||
                    Managers.Tile.GetTile(Define.Tilemap.Building, cellPos + Vector3Int.left))
                {
                    _prevNeighborPos = cellPos + Vector3Int.left;
                    nextTile = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_CDL");
                }
                else
                {
                    nextTile = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_BU");
                }
            }
            else if (gapPos == Vector3Int.left)
            {
                if (Managers.Tile.GetTile(Define.Tilemap.Road, cellPos + Vector3Int.left) ||
                    Managers.Tile.GetTile(Define.Tilemap.Building, cellPos + Vector3Int.left))
                {
                    _prevNeighborPos = cellPos + Vector3Int.left;
                    nextTile = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_LR");
                }
                else if (Managers.Tile.GetTile(Define.Tilemap.Road, cellPos + Vector3Int.up) ||
                    Managers.Tile.GetTile(Define.Tilemap.Building, cellPos + Vector3Int.up))
                {
                    _prevNeighborPos = cellPos + Vector3Int.up;
                    nextTile = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_CDR");
                }
                else if (Managers.Tile.GetTile(Define.Tilemap.Road, cellPos + Vector3Int.down) ||
                    Managers.Tile.GetTile(Define.Tilemap.Building, cellPos + Vector3Int.down))
                {
                    _prevNeighborPos = cellPos + Vector3Int.down;
                    nextTile = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_CUR");
                }
                else
                {
                    nextTile = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_BR");
                }
            }
            else if (gapPos == Vector3Int.right)
            {
                if (Managers.Tile.GetTile(Define.Tilemap.Road, cellPos + Vector3Int.right) ||
                    Managers.Tile.GetTile(Define.Tilemap.Building, cellPos + Vector3Int.right))
                {
                    _prevNeighborPos = cellPos + Vector3Int.right;
                    nextTile = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_LR");
                }
                else if (Managers.Tile.GetTile(Define.Tilemap.Road, cellPos + Vector3Int.up) ||
                    Managers.Tile.GetTile(Define.Tilemap.Building, cellPos + Vector3Int.up))
                {
                    _prevNeighborPos = cellPos + Vector3Int.up;
                    nextTile = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_CDL");
                }
                else if (Managers.Tile.GetTile(Define.Tilemap.Road, cellPos + Vector3Int.down) ||
                    Managers.Tile.GetTile(Define.Tilemap.Building, cellPos + Vector3Int.down))
                {
                    _prevNeighborPos = cellPos + Vector3Int.down;
                    nextTile = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_CUL");
                }
                else
                {
                    nextTile = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_BL");
                }
            }
        }

        if (_prevNeighborPos.HasValue)
        {
            var go = Managers.Tile.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(_prevNeighborPos.Value);
            var road = go.GetComponent<Road>();
            if (road)
            {
                if (road.RoadType != Define.Road.B &&
                    road.RoadType != Define.Road.BU &&
                    road.RoadType != Define.Road.BD &&
                    road.RoadType != Define.Road.BL &&
                    road.RoadType != Define.Road.BR)
                {
                    _prevNeighborPos = null;
                }
            }
        }

        if (nextTile)
        {
            Managers.Tile.SetTile(Define.Tilemap.Road, cellPos, nextTile);
        }
    }

    private void CheckSelf()
    {

    }

    private void CheckPoint()
    {

    }
}
