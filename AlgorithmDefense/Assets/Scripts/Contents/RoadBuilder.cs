using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoadBuilder : BaseBuilder
{
    private static RoadBuilder s_instance = null;
    public static RoadBuilder GetInstance { get { init(); return s_instance; } }

    private void Start()
    {
        init();

        _camera = Camera.main;
        _tempTile = Define.Tilemap.GroundTemp;
    }

    public override void Build(Vector3Int cellPos, Tile originalTile)
    {
        Tile tile = Instantiate(originalTile);
        tile.gameObject = Managers.Resource.Instantiate($"Roads/{originalTile.name}");

        tile.color = Color.white;
        Managers.Tile.SetTile(Define.Tilemap.Ground, cellPos, tile);
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
