using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingBuilder : BaseBuilder
{
    private static BuildingBuilder s_instance;
    public static BuildingBuilder GetInstance { get { init(); return s_instance; } }

    public static readonly string BUILDING_TILE_PATH = "Tiles/Buildings/";
    public static readonly string BUILDING_OBJECT_PATH = "Prefabs/TileObject/Buildings/";

    public override void SetTarget(Define.TileObject tileObject)
    {
        RoadBuilder.GetInstance.Release();
        Release();

        _target = Managers.Resource.Load<TileBase>($"{BUILDING_TILE_PATH}{tileObject.ToString()}");
        _targetTile = _target as Tile;
    }

    public override void Build(TileBase tileBase, Vector3Int cellPos)
    {
        if (_target == null)
        {
            _target = tileBase;
            _targetTile = _target as Tile;
        }

        _targetTile.color = Color.white;
        Tile tile = Instantiate(_targetTile);
        Managers.Tile.SetTile(Define.Tilemap.Building, cellPos, tile);

        tile.gameObject = Managers.Resource.Instantiate($"{BUILDING_OBJECT_PATH}{_target.name}");
        tile.gameObject.transform.SetParent(Managers.Tile.GetTilemap(Define.Tilemap.Building).transform);
        tile.gameObject.transform.position = Managers.Tile.GetCellCenterToWorld(Define.Tilemap.Building, cellPos);

        Release();
    }

    public override void CheckCanBuild(Vector3Int cellPos)
    {
        if (_prevPos == cellPos)
        {
            return;
        }

        _tempTilemap.SetTile(_prevPos, null);
        _prevPos = cellPos;

        if ((Managers.Tile.GetTile(Define.Tilemap.Ground, cellPos) == null) ||
            (Managers.Tile.GetTile(Define.Tilemap.Building, cellPos) != null))
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
        _tempTilemap = Managers.Tile.GetTilemap(Define.Tilemap.BuildingTemp);
    }

    private static void init()
    {
        if (s_instance == null)
        {
            var go = GameObject.Find("@BuildingBuilder");
            if (go == null)
            {
                go = new GameObject { name = "@BuildingBuilder" };
                go.AddComponent<BuildingBuilder>();
            }

            s_instance = go.GetComponent<BuildingBuilder>();
        }
    }
}
