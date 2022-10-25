using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileObjectBuilder : MonoBehaviour
{
    private static TileObjectBuilder s_instance;
    public static TileObjectBuilder GetInstance { get { Init(); return s_instance; } }

    public bool IsBuilding { get; private set; }

    private TileBase _target;
    private Tile _targetTile;
    private Vector3Int _prevCellPos;
    private bool _canBuild;
    private bool _isRoadBuild;

    private void Update()
    {
        if (_target)
        {
            IsBuilding = true;

            if (Input.GetMouseButtonDown(1))
            {
                Clear();
                return;
            }

            CheckCanBuild(MouseController.GetInstance.MouseCellPos);

            if (_canBuild && Input.GetMouseButton(0))
            {
                Build(_target, MouseController.GetInstance.MouseCellPos);
            }

            if (_isRoadBuild && Input.GetMouseButtonUp(0))
            {
                _isRoadBuild = false;
            }
        }
    }

    public void SetRoadTarget()
    {
        Clear();

        _target = Managers.Resource.Load<TileBase>($"{Define.ROAD_TILE_PATH}Road_B");
        _targetTile = _target as Tile;
    }

    public void SetBuildingTarget(Define.Building building)
    {
        Clear();

        _target = Managers.Resource.Load<TileBase>($"{Define.BUILDING_TILE_PATH}{building}");
        _targetTile = _target as Tile;
    }

    public void Build(TileBase tileBase, Vector3Int cellPos)
    {
        if (tileBase.name.Equals("Road_B"))
        {
            return;
        }
        else
        {
            TileManager.GetInstance.SetTile(Define.Tilemap.Building, cellPos, tileBase);
            Clear();
        }
    }

    public void Clear()
    {
        if (!_target)
        {
            return;
        }

        IsBuilding = false;
        TileManager.GetInstance.SetTile(Define.Tilemap.Temp, _prevCellPos, null);
        _targetTile.color = Color.white;
        _targetTile = null;
        _target = null;
        _canBuild = false;
        _isRoadBuild = false;
    }

    private void CheckCanBuild(Vector3Int cellPos)
    {
        TileManager.GetInstance.SetTile(Define.Tilemap.Temp, _prevCellPos, null);

        if (TileManager.GetInstance.GetTile(Define.Tilemap.Road, cellPos) ||
            TileManager.GetInstance.GetTile(Define.Tilemap.Building, cellPos) ||
            TileManager.GetInstance.GetTile(Define.Tilemap.Rampart, cellPos))
        {
            SetCanBuildFalse();
        }
        else
        {
            SetCanBuildTrue();
        }

        TileManager.GetInstance.SetTile(Define.Tilemap.Temp, cellPos, _targetTile);

        _prevCellPos = cellPos;
    }

    private void SetCanBuildTrue()
    {
        _targetTile.color = Color.white;
        _canBuild = true;
    }

    private void SetCanBuildFalse()
    {
        _targetTile.color = Color.red;
        _canBuild = false;
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            var go = GameObject.Find("@TileObjectBuilder");
            if (!go)
            {
                go = Util.CreateGameObject<TileObjectBuilder>("@TileObjectBuilder");
            }

            s_instance = go.GetComponent<TileObjectBuilder>();
        }
    }
}
