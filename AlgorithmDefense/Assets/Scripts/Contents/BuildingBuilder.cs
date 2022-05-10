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

    public override void Build(Vector3Int cellPos, Tile originalTile)
    {
        Tile tile = Instantiate(originalTile);
        tile.gameObject = Managers.Resource.Instantiate($"Buildings/{originalTile.name}");
        
        var buildPos = Managers.Tile.GetCellToWorld(Define.Tilemap.Building, cellPos);
        buildPos.y += 0.35f;
        tile.gameObject.transform.position = buildPos;

        //tile.color = Color.white;
        tile.color += new Color(0, 0, 0, 1);
        Managers.Tile.SetTile(Define.Tilemap.Building, cellPos, tile);

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
