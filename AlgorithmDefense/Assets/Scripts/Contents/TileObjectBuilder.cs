using System;
using System.Collections;
using System.Collections.Generic;
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
        }
    }

    public void SetRoadTarget()
    {
        Clear();

        _target = Managers.Resource.Load<TileBase>($"{Define.RULE_TILE_PATH}RoadRuleTile");
        _targetTile = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_B");
    }

    public void SetBuildingTarget(Define.Building building)
    {
        Clear();

        _target = Managers.Resource.Load<TileBase>($"{Define.BUILDING_TILE_PATH}{building.ToString()}");
        _targetTile = _target as Tile;
    }

    public void Build(TileBase tileBase, Vector3Int cellPos)
    {
        if (tileBase.name.Equals("RoadRuleTile"))
        {
            Managers.Tile.SetTile(Define.Tilemap.Road, cellPos, tileBase);
        }
        else
        {
            Managers.Tile.SetTile(Define.Tilemap.Building, cellPos, tileBase);
            SetRoadTarget();
            Managers.Tile.SetTile(Define.Tilemap.Road, cellPos, _target);
            Clear();
        }
    }

    private void CheckCanBuild(Vector3Int cellPos)
    {
        if (_prevCellPos == cellPos)
        {
            return;
        }

        Managers.Tile.SetTile(Define.Tilemap.Temp, _prevCellPos, null);

        if (Managers.Tile.GetTile(Define.Tilemap.Building, cellPos))
        {
            _targetTile.color = Color.red;
            _canBuild = false;
        }
        else
        {
            _targetTile.color = Color.white;
            _canBuild = true;
        }

        Managers.Tile.SetTile(Define.Tilemap.Temp, cellPos, _targetTile);

        _prevCellPos = cellPos;
    }

    public void Clear()
    {
        if (!_target)
        {
            return;
        }

        IsBuilding = false;
        Managers.Tile.SetTile(Define.Tilemap.Temp, _prevCellPos, null);
        _targetTile.color = Color.white;
        _targetTile = null;
        _target = null;
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
