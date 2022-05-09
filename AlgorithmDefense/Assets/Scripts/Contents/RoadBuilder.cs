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

    public override void Build(Vector3Int cellPos, Tile tile)
    {
        var go = Managers.Resource.Instantiate($"Roads/{tile.name}");
        tile.gameObject = go;

        var buildPos = Managers.Tile.GetCellToWorld(Define.Tilemap.Ground, cellPos);
        buildPos.y += 0.35f;
        go.transform.position = buildPos;
        go.name.Replace("(Clone)", "");

        //tile.color = Color.white;
        tile.color += new Color(0, 0, 0, 1);
        Managers.Tile.SetTile(Define.Tilemap.Ground, cellPos, tile);

        Managers.Resource.Destroy(go);
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
