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
        _tempTilemap = Define.Tilemap.BuildingTemp;
    }

    public override void Build(Vector3Int cellPos, Tile originalTile)
    {
        Tile tile = Instantiate(originalTile);
        tile.gameObject = Managers.Resource.Load<GameObject>($"Prefabs/Buildings/{originalTile.name}");
        tile.gameObject.transform.position = Managers.Tile.GetCellToWorld(Define.Tilemap.Building, cellPos);

        tile.color = Color.white;
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
