using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoadBuilder : BaseBuilder
{
    private static RoadBuilder s_instance;
    public static RoadBuilder GetInstance { get { Init(); return s_instance; } }

    public static readonly string ROAD_TILE_PATH = "Tiles/Roads/";
    public static readonly string ROAD_RULETILE_PATH = "Tiles/RuleTiles/";

    private void Start()
    {
        _tempTilemap = Managers.Tile.GetTilemap(Define.Tilemap.RoadTemp);
    }

    public override void SetTarget(Define.TileObject tileObject)
    {
        BuildingBuilder.GetInstance.Release();
        Release();

        _target = Managers.Resource.Load<TileBase>($"{ROAD_RULETILE_PATH}{tileObject.ToString()}");
        _targetTile = Managers.Resource.Load<Tile>($"{ROAD_TILE_PATH}Road_B");
    }

    public override void Build(TileBase tileBase, Vector3Int cellPos)
    {
        Managers.Tile.SetTile(Define.Tilemap.Road, cellPos, tileBase);
    }

    private static void Init()
    {
        if (!s_instance)
        {
            var go = GameObject.Find("@RoadBuilder");
            if (!go)
            {
                go = new GameObject { name = "@RoadBuilder" };
                go.AddComponent<RoadBuilder>();
            }

            s_instance = go.GetComponent<RoadBuilder>();
        }
    }
}
