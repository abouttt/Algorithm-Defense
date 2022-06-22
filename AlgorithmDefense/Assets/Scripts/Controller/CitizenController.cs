using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CitizenController : BaseController
{
    [field: SerializeField] 
    public Define.Citizen CitizenType { get; private set; }

    [field: SerializeField] 
    public Define.MoveType MoveType { get; set; } = Define.MoveType.None;

    [field: SerializeField]
    public Define.Class Class { get; set; } = Define.Class.None;

    [field: SerializeField]
    public Define.ClassTier Tier { get; set; } = 0;

    [field: SerializeField]
    public Define.Class TempClass { get; set; } = Define.Class.None;

    [field: SerializeField]
    public uint ClassTrainingCount { get; set; } = 0;

    public Vector3Int PrevPos { get; set; }
    public bool IsExit { get; set; }

    [SerializeField]
    private float _moveSpeed = 0.0f;

    private Vector3 _dest;

    public override void Init()
    {
        _state = Define.State.Moving;
    }

    protected override void UpdateMoving()
    {
        var cellPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Ground, transform.position);
        if (Vector2.Distance(transform.position, _dest) <= 0.01f)
        {
            checkRoad(cellPos);
            SetDest();
        }

        transform.position = Vector2.MoveTowards(transform.position, _dest, (_moveSpeed * Time.deltaTime));

        if (IsExit)
        {
            checkOnBuilding(cellPos);
        }

        if (!IsExit && PrevPos != cellPos)
        {
            IsExit = true;
            PrevPos = cellPos;
        }
    }

    public void SetDest()
    {
        var cellPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Ground, transform.position);
        switch (MoveType)
        {
            case Define.MoveType.Down:
                cellPos.y--;
                break;
            case Define.MoveType.Up:
                cellPos.y++;
                break;
            case Define.MoveType.Right:
                cellPos.x++;
                transform.localScale = new Vector3(-1, 1, 1);
                break;
            case Define.MoveType.Left:
                cellPos.x--;
                transform.localScale = new Vector3(1, 1, 1);
                break;
        }

        _dest = Managers.Tile.GetCellCenterToWorld(Define.Tilemap.Ground, cellPos);
    }

    private void checkOnBuilding(Vector3Int currentPos)
    {
        var tile = Managers.Tile.GetTile(Define.Tilemap.Building, currentPos) as Tile;
        if (!tile)
        {
            return;
        }

        tile.gameObject.GetComponent<BaseBuilding>().EnterTheBuilding(this);
    }

    private void checkRoad(Vector3Int currentPos)
    {
        var go = Managers.Tile.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(currentPos);
        if (!go)
        {
            return;
        }

        var road = go.GetComponent<Road>();
        switch (road.RoadType)
        {
            case Define.RoadType.B:
            case Define.RoadType.BU:
            case Define.RoadType.BD:
            case Define.RoadType.BL:
            case Define.RoadType.BR:
                turnAround();
                break;
            case Define.RoadType.TU:
            case Define.RoadType.TD:
                if (isMoveTypeUD())
                {
                    turnAround();
                }
                break;
            case Define.RoadType.TL:
            case Define.RoadType.TR:
                if (!isMoveTypeUD())
                {
                    turnAround();
                }
                break;
            case Define.RoadType.CUL:
                MoveType = isMoveTypeUD() ? Define.MoveType.Left : Define.MoveType.Down;
                break;
            case Define.RoadType.CUR:
                MoveType = isMoveTypeUD() ? Define.MoveType.Right : Define.MoveType.Down;
                break;
            case Define.RoadType.CDR:
                MoveType = isMoveTypeUD() ? Define.MoveType.Right : Define.MoveType.Up;
                break;
            case Define.RoadType.CDL:
                MoveType = isMoveTypeUD() ? Define.MoveType.Left : Define.MoveType.Up;
                break;
        }
    }

    private void turnAround()
    {
        switch (MoveType)
        {
            case Define.MoveType.Right:
                MoveType = Define.MoveType.Left;
                break;
            case Define.MoveType.Left:
                MoveType = Define.MoveType.Right;
                break;
            case Define.MoveType.Up:
                MoveType = Define.MoveType.Down;
                break;
            case Define.MoveType.Down:
                MoveType = Define.MoveType.Up;
                break;
        }
    }

    private bool isMoveTypeUD()
    {
        if ((MoveType == Define.MoveType.Up) || (MoveType == Define.MoveType.Down))
        {
            return true;
        }

        return false;
    }
}
