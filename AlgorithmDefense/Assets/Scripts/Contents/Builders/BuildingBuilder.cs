using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingBuilder : BaseBuilder
{
    private static BuildingBuilder s_instance;
    public static BuildingBuilder GetInstance { get { Init(); return s_instance; } }

    public static readonly string BUILDING_TILE_PATH = "Tiles/Buildings/";
    public static readonly string BUILDING_OBJECT_PATH = "Prefabs/TileObject/Buildings/";

    private void Start()
    {
        _tempTilemap = Managers.Tile.GetTilemap(Define.Tilemap.BuildingTemp);
    }

    public override void SetTarget(Define.TileObject tileObject)
    {
        RoadBuilder.GetInstance.Release();
        Release();

        _target = Managers.Resource.Load<TileBase>($"{BUILDING_TILE_PATH}{tileObject.ToString()}");
        _targetTile = _target as Tile;
    }

    public override void Build(TileBase tileBase, Vector3Int cellPos)
    {
        if (!_target)
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

        var roadTile = Managers.Resource.Load<TileBase>($"{RoadBuilder.ROAD_RULETILE_PATH}Road_RuleTile");
        RoadBuilder.GetInstance.Build(roadTile, cellPos);

        Release();
    }

    private static void Init()
    {
        if (!s_instance)
        {
            var go = GameObject.Find("@BuildingBuilder");
            if (!go)
            {
                go = new GameObject { name = "@BuildingBuilder" };
                go.AddComponent<BuildingBuilder>();
            }

            s_instance = go.GetComponent<BuildingBuilder>();
        }
    }
}
