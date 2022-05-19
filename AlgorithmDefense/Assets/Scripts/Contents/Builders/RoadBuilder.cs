using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoadBuilder : BaseBuilder
{
    private static RoadBuilder s_instance = null;
    public static RoadBuilder GetInstance { get { init(); return s_instance; } }

    public static readonly string ROAD_RULETILE_PATH = "Tiles/RuleTiles/";
    public static readonly string ROAD_PATH = "Tiles/Roads/";

    public override void SetTarget(Define.TileObject tileObject)
    {
        Release();

        _target = Managers.Resource.Load<TileBase>($"{ROAD_RULETILE_PATH}{tileObject.ToString()}");
        _targetTile = Managers.Resource.Load<Tile>($"{ROAD_PATH}{tileObject.ToString().Replace("_RuleTile", "")}");
    }

    public override void Build(TileBase tileBase, Vector3Int cellPos)
    {
        _originalTilemap.SetTile(cellPos, tileBase);
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

    public override void Release()
    {
        if (_target == null)
        {
            return;
        }

        _tempTilemap.SetTile(_prevPos, null);
        _targetTile.color = Color.white;
        _targetTile = null;
        _target = null;
        IsBuilding = false;
    }

    protected override void Init()
    {
        base.Init();

        _originalTilemap = Managers.Tile.GetTilemap(Define.Tilemap.Road);
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
