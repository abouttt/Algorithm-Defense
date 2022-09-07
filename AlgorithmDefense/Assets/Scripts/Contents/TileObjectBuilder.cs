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
    public Dictionary<Vector3Int, int> RoadGroupNumberDatas = new();

    private TileBase _target;
    private Tile _targetTile;
    private Vector3Int _prevCellPos;
    private bool _canBuild;

    private int _roadGroupNumber = 1;
    private int _roadGroupCount = 0;

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

            if (_canBuild && Input.GetMouseButtonDown(0))
            {
                Build(_target, MouseController.GetInstance.MouseCellPos);
            }
        }
    }

    public void SetRoadTarget()
    {
        Clear();

        _target = Managers.Resource.Load<TileBase>($"{Define.RULE_TILE_PATH}Test/RoadRuleTile");
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
            var go = Managers.Tile.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(cellPos);
            go.GetComponent<Road>().GroupNumber = _roadGroupNumber;
            RoadGroupNumberDatas[cellPos] = _roadGroupNumber;

            if (++_roadGroupCount >= 3)
            {
                Debug.Log("Group Number Plus");
                _roadGroupNumber++;
                _roadGroupCount = 0;
            }
        }
        else
        {
            Managers.Tile.SetTile(Define.Tilemap.Building, cellPos, tileBase);
            //SetRoadTarget();
            //Managers.Tile.SetTile(Define.Tilemap.Road, cellPos, _target);
            Clear();
        }

        Managers.Tile.GetTilemap(Define.Tilemap.Road).RefreshAllTiles();
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

    private void CheckCanBuild(Vector3Int cellPos)
    {
        Managers.Tile.SetTile(Define.Tilemap.Temp, _prevCellPos, null);


        if (Managers.Tile.GetTile(Define.Tilemap.Road, cellPos) ||
            Managers.Tile.GetTile(Define.Tilemap.Building, cellPos) ||
            Managers.Tile.GetTile(Define.Tilemap.Rampart, cellPos))
        {
            SetCanBuildFalse();
        }
        else
        {
            SetCanBuildTrue();
        }

        Managers.Tile.SetTile(Define.Tilemap.Temp, cellPos, _targetTile);

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
