using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingBuilder : MonoBehaviour
{
    private static BuildingBuilder s_instance = null;
    public static BuildingBuilder GetInstance { get { init(); return s_instance; } }

    public Tile Target { get; set; } = null;
    public bool IsBuilding { get; set; } = false;

    private Camera _camera = null;

    private Vector3Int _prevPos;
    private Color _validColor = new Color(1, 1, 1, 0.5f);
    private Color _unvalidColor = new Color(1, 0, 0, 0.5f);

    private void Start()
    {
        init();

        _camera = Camera.main;
    }

    private void Update()
    {
        if (Target != null)
        {
            IsBuilding = true;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            Vector3Int cellPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Temp, worldPoint);

            if (_prevPos != cellPos)
            {
                Managers.Tile.SetTile(Define.Tilemap.Temp, _prevPos, null);
                _prevPos = cellPos;

                if ((Managers.Tile.GetTile(Define.Tilemap.Ground, cellPos) == null) ||
                    (Managers.Tile.GetTile(Define.Tilemap.Main, cellPos)) != null)
                {
                    Target.color = _unvalidColor;
                }
                else
                {
                    Target.color = _validColor;
                }

                Managers.Tile.SetTile(Define.Tilemap.Temp, cellPos, Target);
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (Managers.Tile.GetTile(Define.Tilemap.Ground, cellPos) == null)
                {
                    return;
                }

                if (Managers.Tile.GetTile(Define.Tilemap.Main, cellPos) != null)
                {
                    return;
                }

                Build(cellPos, Target);

                reset();
            }

            if (Input.GetMouseButtonDown(1))
            {
                reset();
            }
        }
    }

    public void Build(Vector3Int cellPos, Tile tile)
    {
        var go = Managers.Resource.Instantiate($"Buildings/{tile.name}");
        tile.gameObject = go;

        var buildPos = Managers.Tile.GetCellToWorld(Define.Tilemap.Main, cellPos);
        buildPos.y += 0.35f;
        go.transform.position = buildPos;

        tile.color += new Color(0, 0, 0, 1);
        Managers.Tile.SetTile(Define.Tilemap.Main, cellPos, tile);

        Managers.Resource.Destroy(go);
    }

    private void reset()
    {
        Managers.Tile.GetTilemap(Define.Tilemap.Temp).ClearAllTiles();
        Target.color = Color.white;
        Target = null;
        IsBuilding = false;
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
