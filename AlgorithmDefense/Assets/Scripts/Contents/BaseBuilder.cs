using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class BaseBuilder : MonoBehaviour
{
    public Tile Target { get; set; } = null;
    public bool IsBuilding { get; protected set; } = false;

    protected Camera _camera = null;
    protected Define.Tilemap _tempTile;

    protected Vector3Int _prevPos;
    protected Color _validColor = new Color(1, 1, 1, 0.5f);
    protected Color _unvalidColor = new Color(1, 0, 0, 0.5f);

    private void Update()
    {
        if (Target != null)
        {
            IsBuilding = true;
            CheckCanBuild();
        }
    }

    protected void CheckCanBuild()
    {
        IsBuilding = true;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector3Int cellPos = Managers.Tile.GetWorldToCell(_tempTile, worldPoint);

        if (_prevPos != cellPos)
        {
            Managers.Tile.SetTile(_tempTile, _prevPos, null);
            _prevPos = cellPos;

            if ((Managers.Tile.GetTile(Define.Tilemap.Ground, cellPos) == null) ||
                (Managers.Tile.GetTile(Define.Tilemap.Road, cellPos) != null) ||
                (Managers.Tile.GetTile(Define.Tilemap.Building, cellPos)) != null)
            {
                Target.color = _unvalidColor;
            }
            else
            {
                Target.color = _validColor;
            }

            Managers.Tile.SetTile(_tempTile, cellPos, Target);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Managers.Tile.GetTile(Define.Tilemap.Ground, cellPos) == null)
            {
                return;
            }

            if (Managers.Tile.GetTile(Define.Tilemap.Road, cellPos) != null)
            {
                return;
            }

            if (Managers.Tile.GetTile(Define.Tilemap.Building, cellPos) != null)
            {
                return;
            }

            Build(cellPos, Target);

            Reset();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Reset();
        }
    }

    public abstract void Build(Vector3Int cellPos, Tile tile);

    protected void Reset()
    {
        Managers.Tile.GetTilemap(Define.Tilemap.RoadTemp).ClearAllTiles();
        Managers.Tile.GetTilemap(Define.Tilemap.BuildingTemp).ClearAllTiles();
        Target.color = Color.white;
        Target = null;
        IsBuilding = false;
    }
}
