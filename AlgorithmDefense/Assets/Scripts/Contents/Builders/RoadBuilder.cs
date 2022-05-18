using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoadBuilder : BaseBuilder
{
    private static RoadBuilder s_instance = null;
    public static RoadBuilder GetInstance { get { init(); return s_instance; } }

    public static readonly string ROAD_RULETILE_PATH = "Tiles/RuleTiles/";
    public static readonly string ROAD_PATH = "Tiles/Roads2/";

    private void Start()
    {
        _camera = Camera.main;
        _tempTilemap = Define.Tilemap.RoadTemp;
    }

    public override void SetTarget(Define.TileObject tileObject)
    {
        Release();

        _target = Managers.Resource.Load<TileBase>($"{ROAD_RULETILE_PATH}{tileObject.ToString()}");
        _targetTile = Managers.Resource.Load<Tile>($"{ROAD_PATH}{tileObject.ToString().Replace("_RuleTile", "")}");
    }

    public override void Build(TileBase tileBase, Vector3Int cellPos)
    {
        Managers.Tile.SetTile(Define.Tilemap.Road, cellPos, tileBase);
        var go = Managers.Tile.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(cellPos);
    }

    public override void CheckCanBuild(Vector3Int cellPos)
    {
        Managers.Tile.SetTile(_tempTilemap, _prevPos, null);
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

        Managers.Tile.SetTile(_tempTilemap, cellPos, _targetTile);
    }

    public override void Release()
    {
        if (_target == null)
        {
            return;
        }

        Managers.Tile.SetTile(_tempTilemap, _prevPos, null);

        _targetTile.color = Color.white;
        _targetTile = null;
        _target = null;
        IsBuilding = false;
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
