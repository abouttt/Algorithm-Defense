using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    private static TileSelector s_instance;
    public static TileSelector GetInstance { get { Init(); return s_instance; } }

    public Vector3Int CurrentMouseCellPos { get; private set; }

    private Camera _camera;
    private Tile _tile;
    private Vector3 _worldPos;
    private Vector3Int _prevCellPos;

    private void Start()
    {
        _camera = Camera.main;
        _tile = Managers.Resource.Load<Tile>("Tiles/Grounds/TileSelect");
    }

    private void Update()
    {
        _worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        CurrentMouseCellPos = Managers.Tile.GetGrid().WorldToCell(_worldPos);

        if (_prevCellPos != CurrentMouseCellPos)
        {
            if (Managers.Tile.GetTile(Define.Tilemap.Ground, CurrentMouseCellPos) != null)
            {
                Managers.Tile.SetTile(Define.Tilemap.GroundTemp, CurrentMouseCellPos, _tile);
                Managers.Tile.SetTile(Define.Tilemap.GroundTemp, _prevCellPos, null);
                _prevCellPos = CurrentMouseCellPos;
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
            if (tile)
            {
                var building = tile.gameObject.GetComponent<BaseBuilding>();
                if (building.CanSelect)
                {
                    building.ShowUIController();
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (BaseBuilder.IsBuilding)
            {
                return;
            }

            var building = Managers.Tile.GetTile(Define.Tilemap.Building, CurrentMouseCellPos) as Tile;
            if (building)
            {
                Managers.Tile.SetTile(Define.Tilemap.Building, CurrentMouseCellPos, null);
                Managers.Tile.SetTile(Define.Tilemap.Road, CurrentMouseCellPos, null);
                Managers.Resource.Destroy(Managers.Tile.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(CurrentMouseCellPos));
                Managers.Resource.Destroy(building.gameObject);
            }

            var road = Managers.Tile.GetTile(Define.Tilemap.Road, CurrentMouseCellPos);
            if (road)
            {
                Managers.Tile.SetTile(Define.Tilemap.Road, CurrentMouseCellPos, null);
                Managers.Resource.Destroy(Managers.Tile.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(CurrentMouseCellPos));
            }
        }
    }

    private static void Init()
    {
        if (!s_instance)
        {
            var go = GameObject.Find("@TileSelector");
            if (!go)
            {
                go = new GameObject { name = "@TileSelector" };
                go.AddComponent<TileSelector>();
            }

            s_instance = go.GetComponent<TileSelector>();
        }
    }
}
