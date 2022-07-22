using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class MouseController : MonoBehaviour
{
    private static MouseController s_instance;
    public static MouseController GetInstance { get { Init(); return s_instance; } }

    public Vector3 MouseWorldPos { get; private set; }
    public Vector3Int MouseCellPos { get; private set; }

    private Camera _camera;

    private void Start()
    {
        Init();

        _camera = Camera.main;
    }

    private void Update()
    {
        MouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        MouseCellPos = Managers.Tile.GetGrid().WorldToCell(MouseWorldPos);

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (TileObjectBuilder.GetInstance.IsBuilding)
            {
                return;
            }

            var go = Managers.Tile.GetTilemap(Define.Tilemap.Building).GetInstantiatedObject(MouseCellPos);
            if (go)
            {
                var building = go.GetComponent<BaseBuilding>();
                if (building.HasUI)
                {
                    // TODO
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (TileObjectBuilder.GetInstance.IsBuilding)
            {
                return;
            }

            var building = Managers.Tile.GetTile(Define.Tilemap.Building, MouseCellPos);
            if (building)
            {
                Managers.Tile.SetTile(Define.Tilemap.Building, MouseCellPos, null);
                Managers.Resource.Destroy(Managers.Tile.GetTilemap(Define.Tilemap.Building).GetInstantiatedObject(MouseCellPos));
            }

            var road = Managers.Tile.GetTile(Define.Tilemap.Road, MouseCellPos);
            if (road)
            {
                Managers.Tile.SetTile(Define.Tilemap.Road, MouseCellPos, null);
                Managers.Resource.Destroy(Managers.Tile.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(MouseCellPos));
            }
        }
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            var go = GameObject.Find("@MouseController");
            if (go == null)
            {
                go = Util.CreateGameObject<MouseController>("@MouseController");
            }

            s_instance = go.GetComponent<MouseController>();
        }
    }
}
