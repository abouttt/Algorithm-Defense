using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class BaseBuilder : MonoBehaviour
{
    public static bool IsBuilding { get; protected set; } = false;

    protected TileBase _target;
    protected Tile _targetTile;
    protected Tilemap _tempTilemap;

    protected Vector3Int _prevPos;
    protected bool _canBuild = false;

    protected readonly Color _validColor = new Color(1, 1, 1, 0.5f);
    protected readonly Color _unvalidColor = new Color(1, 0, 0, 0.5f);

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

            CheckCanBuild(TileSelector.GetInstance.CurrentMouseCellPos);

            if (_canBuild && Input.GetMouseButton(0))
            {
                Build(_target, TileSelector.GetInstance.CurrentMouseCellPos);
            }
        }
    }

    protected abstract void Init();
    public abstract void SetTarget(Define.TileObject tileObject);
    public abstract void CheckCanBuild(Vector3Int cellPos);
    public abstract void Build(TileBase tileBase, Vector3Int cellPos);
    public virtual void Release()
    {
        if (_target == null)
        {
            return;
        }

        _tempTilemap.SetTile(_prevPos, null);
        _targetTile.color = Color.white;
        _targetTile = null;
        _target = null;
        IsBuilding = false;
    }
}
