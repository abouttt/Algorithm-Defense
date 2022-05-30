using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class BuildingSelector : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (BaseBuilder.IsBuilding)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            Vector3Int cellPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Building, worldPoint);
            var tile = Managers.Tile.GetTile(Define.Tilemap.Building, cellPos) as Tile;

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
}
