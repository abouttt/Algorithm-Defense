using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class BaseBuilder : MonoBehaviour
{
    public static bool IsBuilding { get; protected set; } = false;

    protected Camera _camera = null;
    protected TileBase _target = null;
    protected Tile _targetTile = null;
    protected Tilemap _originalTilemap = null;
    protected Tilemap _tempTilemap = null;

    protected Vector3Int _prevPos;
    protected Color _validColor = new Color(1, 1, 1, 0.5f);
    protected Color _unvalidColor = new Color(1, 0, 0, 0.5f);
    protected bool _canBuild = false;

    private void Start()
    {
        Init();
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
            Vector3Int cellPos = _tempTilemap.WorldToCell(worldPoint);

            if ((_tempTilemap == Managers.Tile.GetTilemap(Define.Tilemap.RoadTemp)) &&
                (Input.GetKeyDown(KeyCode.R)))
            {
                if (_target.name.Equals(Define.TileObject.Road_UD_RuleTile.ToString()))
                {
                    SetTarget(Define.TileObject.Road_LR_RuleTile);
                }
                else
                {
                    SetTarget(Define.TileObject.Road_UD_RuleTile);
                }
            }

            CheckCanBuild(cellPos);

            if (_canBuild && Input.GetMouseButton(0))
            {
                Build(_target, cellPos);
            }
        }
    }

    protected virtual void Init()
    {
        _camera = Camera.main;
    }

    public abstract void SetTarget(Define.TileObject tileObject);

    public abstract void CheckCanBuild(Vector3Int cellPos);

    public abstract void Build(TileBase tileBase, Vector3Int cellPos);

    public abstract void Release();
}
