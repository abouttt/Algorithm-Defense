using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectBuilder : MonoBehaviour
{
    private static ObjectBuilder s_instance = null;
    public static ObjectBuilder GetInstance { get { init(); return s_instance; } }

    public static readonly string ROAD_PATH = "Tiles/Roads/";
    public static readonly string BUILDING_PATH = "Tiles/Buildings/";

    public bool IsBuilding { get; private set; } = false;

    private Tile _target = null;
    private Camera _camera = null;
    private Define.Tilemap _tempTilemap = Define.Tilemap.None;

    private Vector3Int _prevPos;
    private Color _validColor = new Color(1, 1, 1, 0.5f);
    private Color _unvalidColor = new Color(1, 0, 0, 0.5f);
    private bool _canBuild = false;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_target != null)
        {
            IsBuilding = true;

            if (Input.GetMouseButtonDown(1))
            {
                Release();
                return;
            }

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            Vector3Int cellPos = Managers.Tile.GetWorldToCell(_tempTilemap, worldPoint);

            CheckCanBuild(cellPos);

            if (_tempTilemap == Define.Tilemap.GroundTemp && Input.GetKeyDown(KeyCode.R))
            {
                _target = _target.name.Equals("Road_UD") ? 
                    Managers.Resource.Load<Tile>($"{ROAD_PATH}Road_LR") : Managers.Resource.Load<Tile>($"{ROAD_PATH}Road_UD");
            }

            if (_canBuild && Input.GetMouseButton(0))
            {
                switch (_tempTilemap)
                {
                    case Define.Tilemap.GroundTemp:
                        Build(_target, Define.BuildType.Ground, cellPos);
                        break;
                    case Define.Tilemap.BuildingTemp:
                        Build(_target, Define.BuildType.Building, cellPos);
                        break;
                }
            }
        }
    }

    public void SetTarget(Define.BuildType buildType, Define.TileObject tileObject)
    {
        Release();

        switch (buildType)
        {
            case Define.BuildType.Ground:
                _tempTilemap = Define.Tilemap.GroundTemp;
                _target = Managers.Resource.Load<Tile>($"{ROAD_PATH}{tileObject.ToString()}");
                break;
            case Define.BuildType.Building:
                _tempTilemap = Define.Tilemap.BuildingTemp;
                _target = Managers.Resource.Load<Tile>($"{BUILDING_PATH}{tileObject.ToString()}");
                break;
        }
    }

    public void CheckCanBuild(Vector3Int cellPos)
    {
        if ((_tempTilemap == Define.Tilemap.BuildingTemp) &&
            (_prevPos == cellPos))
        {
            return;
        }

        Managers.Tile.SetTile(_tempTilemap, _prevPos, null);
        _prevPos = cellPos;

        if ((Managers.Tile.GetTile(Define.Tilemap.Ground, cellPos) == null) ||
            (Managers.Tile.GetTile(Define.Tilemap.Building, cellPos)) != null)
        {
            _target.color = _unvalidColor;
            _canBuild = false;
        }
        else
        {
            _target.color = _validColor;
            _canBuild = true;
        }

        if (_tempTilemap == Define.Tilemap.GroundTemp)
        {
            var groundTile = Managers.Tile.GetTile(Define.Tilemap.Ground, cellPos) as Tile;
            if (groundTile != null && groundTile.gameObject != null)
            {
                _target.color = _unvalidColor;
                _canBuild = false;
            }
        }

        Managers.Tile.SetTile(_tempTilemap, cellPos, _target);
    }

    public void Build(Tile originalTile, Define.BuildType buildType, Vector3Int cellPos)
    {
        Tile tile = Instantiate(originalTile);
        tile.color = Color.white;

        switch (buildType)
        {
            case Define.BuildType.Ground:
                Managers.Tile.SetTile(Define.Tilemap.Ground, cellPos, tile);
                tile.gameObject = Managers.Resource.Instantiate($"{ROAD_PATH}{originalTile.name}",
                    Managers.Tile.GetTilemap(Define.Tilemap.Ground).transform);
                tile.gameObject.transform.position = Managers.Tile.GetCellCenterToWorld(Define.Tilemap.Ground, cellPos);
                break;

            case Define.BuildType.Building:
                Managers.Tile.SetTile(Define.Tilemap.Building, cellPos, tile);
                tile.gameObject = Managers.Resource.Instantiate($"{BUILDING_PATH}{originalTile.name}",
                    Managers.Tile.GetTilemap(Define.Tilemap.Building).transform);
                tile.gameObject.transform.position = Managers.Tile.GetCellCenterToWorld(Define.Tilemap.Building, cellPos);
                break;
        }

        if (buildType != Define.BuildType.Ground)
        {
            Release();
        }
    }

    public void Release()
    {
        if ((_tempTilemap == Define.Tilemap.None) ||
            (_target == null))
        {
            return;
        }

        Managers.Tile.SetTile(_tempTilemap, _prevPos, null);

        _target.color = Color.white;
        _target = null;
        _tempTilemap = Define.Tilemap.None;
        _prevPos = new Vector3Int(999, 999, 999);
        IsBuilding = false;
    }

    private static void init()
    {
        if (s_instance == null)
        {
            var go = GameObject.Find("@ObjectBuilder");
            if (go == null)
            {
                go = new GameObject { name = "@ObjectBuilder" };
                go.AddComponent<ObjectBuilder>();
            }

            s_instance = go.GetComponent<ObjectBuilder>();
        }
    }
}
