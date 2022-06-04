using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoadBuilder : BaseBuilder
{
    private static RoadBuilder s_instance;
    public static RoadBuilder GetInstance { get { init(); return s_instance; } }

    public static readonly string ROAD_RULETILE_PATH = "Tiles/RuleTiles/";
    public static readonly string ROAD_TILE_PATH = "Tiles/Roads/";

    public override void SetTarget(Define.TileObject tileObject)
    {
        BuildingBuilder.GetInstance.Release();
        Release();

        _target = Managers.Resource.Load<TileBase>($"{ROAD_RULETILE_PATH}{tileObject.ToString()}");
        _targetTile = Managers.Resource.Load<Tile>($"{ROAD_TILE_PATH}{tileObject.ToString().Replace("_RuleTile", "")}");
    }

    public override void Build(TileBase tileBase, Vector3Int cellPos)
    {
        Managers.Tile.SetTile(Define.Tilemap.Road, cellPos, tileBase);
    }

    public override void CheckCanBuild(Vector3Int cellPos)
    {
        _tempTilemap.SetTile(_prevPos, null);
        _prevPos = cellPos;

        if ((Managers.Tile.GetTile(Define.Tilemap.Ground, cellPos) == null) ||
            (Managers.Tile.GetTile(Define.Tilemap.Building, cellPos)) != null)
        {
            _targetTile.color = _unvalidColor;
            _canBuild = false;
        }
        else
        {
            _targetTile.color = _validColor;
            _canBuild = true;
        }

        _tempTilemap.SetTile(cellPos, _targetTile);
    }

    protected override void Init()
    {
        _tempTilemap = Managers.Tile.GetTilemap(Define.Tilemap.RoadTemp);
    }

    private static void init()
    {
        if (s_instance == null)
        {
            var go = GameObject.Find("@RoadBuilder");
            if (go == null)
            {
                go = new GameObject { name = "@RoadBuilder" };
                go.AddComponent<RoadBuilder>();
            }

            s_instance = go.GetComponent<RoadBuilder>();
        }
    }
}
