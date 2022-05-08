using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingBuilder : BaseBuilder
{
    private static BuildingBuilder s_instance = null;
    public static BuildingBuilder GetInstance { get { init(); return s_instance; } }

    private void Start()
    {
        init();

        _camera = Camera.main;
        _tempTile = Define.Tilemap.BuildingTemp;
    }

    public override void Build(Vector3Int cellPos, Tile tile)
    {
        var go = Managers.Resource.Instantiate($"Buildings/{tile.name}");
        tile.gameObject = go;

        var buildPos = Managers.Tile.GetCellToWorld(Define.Tilemap.Building, cellPos);
        buildPos.y += 0.35f;
        go.transform.position = buildPos;
        go.name.Replace("(Clone)", "");

        //tile.color = Color.white;
        tile.color += new Color(0, 0, 0, 1);
        Managers.Tile.SetTile(Define.Tilemap.Building, cellPos, tile);

        Managers.Resource.Destroy(go);
        Release();
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
