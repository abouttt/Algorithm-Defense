using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingBuilder : BaseBuilder
{
    private static BuildingBuilder s_instance = null;
    public static BuildingBuilder GetInstance { get { init(); return s_instance; } }

    public static readonly string BUILDING_PATH = "Tiles/Buildings/";

    public override void SetTarget(Define.TileObject tileObject)
    {
        RoadBuilder.GetInstance.Release();
        Release();

        _target = Managers.Resource.Load<TileBase>($"{BUILDING_PATH}{tileObject.ToString()}");
        _targetTile = _target as Tile;
    }

    public override void Build(TileBase tileBase, Vector3Int cellPos)
    {
        if (_targetTile == null)
        {
            _targetTile = tileBase as Tile;
        }

        _targetTile.color = Color.white;
        Tile tile = Instantiate(tileBase) as Tile;
        Managers.Tile.SetTile(Define.Tilemap.Building, cellPos, tile);

        tile.gameObject = Managers.Resource.Instantiate($"{BUILDING_PATH}{tileBase.name}",
            _originalTilemap.transform);
        tile.gameObject.transform.position = Managers.Tile.GetCellCenterToWorld(Define.Tilemap.Global, cellPos);

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
            (_originalTilemap.GetTile(cellPos) != null))
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

        _originalTilemap = Managers.Tile.GetTilemap(Define.Tilemap.Building);
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
