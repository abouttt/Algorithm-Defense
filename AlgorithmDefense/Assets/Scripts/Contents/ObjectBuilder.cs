using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectBuilder : MonoBehaviour
{
    private static ObjectBuilder s_instance = null;
    public static ObjectBuilder GetInstance { get { init(); return s_instance; } }

    public bool IsBuilding { get; private set; } = false;

    private Tile _target = null;
    private Camera _camera = null;
    private Grid _grid = null;
    private Define.Tilemap _tempTilemap = Define.Tilemap.None;

    private Vector3Int _prevPos;
    private Color _validColor = new Color(1, 1, 1, 0.5f);
    private Color _unvalidColor = new Color(1, 0, 0, 0.5f);
    private bool _canBuild = false;

    private void Awake()
    {
        _camera = Camera.main;
        _grid = FindObjectOfType<Grid>();
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
        switch (buildType)
        {
            case Define.BuildType.Ground:
                _tempTilemap = Define.Tilemap.GroundTemp;
                break;
            case Define.BuildType.Building:
                _tempTilemap = Define.Tilemap.BuildingTemp;
                break;
        }

        _target = Managers.Resource.Load<Tile>($"Tiles/{tileObject.ToString()}");
    }

    public void CheckCanBuild(Vector3Int cellPos)
    {
        if (_prevPos == cellPos)
        {
            return;
        }

        Managers.Tile.SetTile(_tempTilemap, _prevPos, null);
        _prevPos = cellPos;

        var tile = Managers.Tile.GetTile(Define.Tilemap.Ground, cellPos) as Tile;
        if ((Managers.Tile.GetTile(Define.Tilemap.Ground, cellPos) == null) ||
            (tile.gameObject != null) ||
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

        Managers.Tile.SetTile(_tempTilemap, cellPos, _target);
    }

    public void Build(Tile originalTile, Define.BuildType buildType, Vector3Int cellPos)
    {
        Tile tile = Instantiate(originalTile);
        tile.gameObject = Managers.Resource.Load<GameObject>($"Prefabs/Tiles/{originalTile.name}");
        tile.gameObject.transform.position = _grid.GetCellCenterWorld(cellPos);

        tile.color = Color.white;

        switch (buildType)
        {
            case Define.BuildType.Ground:
                Managers.Tile.SetTile(Define.Tilemap.Ground, cellPos, tile);
                break;
            case Define.BuildType.Building:
                Managers.Tile.SetTile(Define.Tilemap.Building, cellPos, tile);
                break;
        }

        Release();
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
