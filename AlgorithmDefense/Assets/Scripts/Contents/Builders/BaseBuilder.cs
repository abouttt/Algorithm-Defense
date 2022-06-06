using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class BaseBuilder : MonoBehaviour
{
    public static bool IsBuilding { get; protected set; } = false;

    protected TileBase _target;
    protected Tile _targetTile;
    protected Tilemap _tempTilemap;

    protected Vector3Int _prevCellPos;
    protected bool _canBuild = false;

    protected readonly Color _validColor = new Color(1, 1, 1, 0.5f);
    protected readonly Color _unvalidColor = new Color(1, 0, 0, 0.5f);

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (_target)
        {
            IsBuilding = true;

            if (Input.GetMouseButtonDown(1))
            {
                Release();
                return;
            }

            CheckCanBuild(TileSelector.GetInstance.CurrentMouseCellPos);

            if (_canBuild && Input.GetMouseButton(0))
            {
                Build(_target, TileSelector.GetInstance.CurrentMouseCellPos);
            }
        }
    }

    public virtual void CheckCanBuild(Vector3Int cellPos)
    {
        if (_prevCellPos == cellPos)
        {
            return;
        }

        _tempTilemap.SetTile(_prevCellPos, null);
        _prevCellPos = cellPos;

        if (!Managers.Tile.GetTile(Define.Tilemap.Ground, cellPos) ||
            (Managers.Tile.GetTile(Define.Tilemap.Building, cellPos)))
        {
            _targetTile.color = _unvalidColor;
            _canBuild = false;
        }
        else
        {
            _targetTile.color = _validColor;
            _canBuild = true;
        }

        _tempTilemap.SetTile(cellPos, _targetTile);
    }

    public virtual void Release()
    {
        if (!_target)
        {
            return;
        }

        _tempTilemap.SetTile(_prevCellPos, null);
        _targetTile.color = Color.white;
        _targetTile = null;
        _target = null;
        IsBuilding = false;
    }

    public abstract void SetTarget(Define.TileObject tileObject);
    public abstract void Build(TileBase tileBase, Vector3Int cellPos);
    protected abstract void Init();
}
