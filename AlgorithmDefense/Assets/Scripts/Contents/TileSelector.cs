using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class TileSelector : MonoBehaviour
{
    private static TileSelector s_instance;
    public static TileSelector GetInstance { get { init(); return s_instance; } }

    public Vector3Int CurrentMouseCellPos { get; private set; }

    private Camera _camera;
    private Tile _tile;
    private Vector3 _worldPos;
    private Vector3Int _prevPos;

    private void Start()
    {
        _camera = Camera.main;
        _tile = Managers.Resource.Load<Tile>("Tiles/Grounds/TileSelect");
    }

    private void Update()
    {
        _worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        CurrentMouseCellPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Building, _worldPos);
        
        
        if (_prevPos != CurrentMouseCellPos)
        {
            if (Managers.Tile.GetTile(Define.Tilemap.Ground, CurrentMouseCellPos) != null)
            {
                Managers.Tile.SetTile(Define.Tilemap.GroundTemp, CurrentMouseCellPos, _tile);
                Managers.Tile.SetTile(Define.Tilemap.GroundTemp, _prevPos, null);
                _prevPos = CurrentMouseCellPos;
            }
        }

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

            var tile = Managers.Tile.GetTile(Define.Tilemap.Building, CurrentMouseCellPos) as Tile;
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
