using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class TileSelector : MonoBehaviour
{
    private static TileSelector s_instance;
    public static TileSelector GetInstance { get { init(); return s_instance; } }

    public Vector3Int MouseCellPos { get; private set; }

    private Camera _camera;
    private Vector3 _worldPos;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        _worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        MouseCellPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Building, _worldPos);

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (BaseBuilder.IsBuilding)
            {
                return;
            }

            var tile = Managers.Tile.GetTile(Define.Tilemap.Building, MouseCellPos) as Tile;
            if (tile != null)
            {
                var building = tile.gameObject.GetComponent<BaseBuilding>();
                if (building.CanSelect)
                {
                    building.ShowUIController();
                }
            }
        }
    }

    private static void init()
    {
        if (s_instance == null)
        {
            var go = GameObject.Find("@TileSelector");
            if (go == null)
            {
                go = new GameObject { name = "@TileSelector" };
                go.AddComponent<TileSelector>();
            }

            s_instance = go.GetComponent<TileSelector>();
        }
    }
}
