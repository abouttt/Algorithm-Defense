using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class BaseBuilder : MonoBehaviour
{
    public Tile Target { get; set; } = null;
    public static bool IsBuilding { get; protected set; } = false;

    protected Camera _camera = null;
    protected Define.Tilemap _tempTile = Define.Tilemap.None;

    protected Vector3Int _prevPos;
    protected Color _validColor = new Color(1, 1, 1, 0.5f);
    protected Color _unvalidColor = new Color(1, 0, 0, 0.5f);
    protected bool _canBuild = false;

    private void Update()
    {
        if (Target != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Release();
                return;
            }

            IsBuilding = true;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            Vector3Int cellPos = Managers.Tile.GetWorldToCell(_tempTile, worldPoint);

            CheckCanBuild(cellPos);

            if (_canBuild && Input.GetMouseButton(0))
            {
                Build(cellPos, Target);
            }
        }
    }

    protected void CheckCanBuild(Vector3Int cellPos)
    {
        if (_prevPos == cellPos)
        {
            return;
        }

        Managers.Tile.SetTile(_tempTile, _prevPos, null);
        _prevPos = cellPos;


        var tile = Managers.Tile.GetTile(Define.Tilemap.Ground, cellPos) as Tile;
        if ((Managers.Tile.GetTile(Define.Tilemap.Ground, cellPos) == null) ||
            (tile.gameObject != null) ||
            (Managers.Tile.GetTile(Define.Tilemap.Building, cellPos)) != null)
        {
            Target.color = _unvalidColor;
            _canBuild = false;
        }
        else
        {
            Target.color = _validColor;
            _canBuild = true;
        }

        Managers.Tile.SetTile(_tempTile, cellPos, Target);
    }

    public abstract void Build(Vector3Int cellPos, Tile tile);

    public void Release()
    {
        if ((_tempTile == Define.Tilemap.None) || 
            (Target == null))
        {
            return;
        }

        Managers.Tile.GetTilemap(_tempTile).ClearAllTiles();
        Target.color = Color.white;
        Target = null;
        IsBuilding = false;
    }
}
