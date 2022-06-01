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
    public Define.Class ClassTemp { get; set; } = Define.Class.None;
    [field: SerializeField]
    public uint ClassTrainingCount { get; set; } = 0;

    public bool IsExit { get; set; } = false;
    public Vector3Int PrevPos { get; set; }

    [SerializeField]
    private float _moveSpeed = 0.0f;

    private bool _isChangeRoad = true;
    private static readonly float TURN_GAP = 0.01f;

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Citizen;
        _state = Define.State.Moving;
    }

    protected override void UpdateMoving()
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

        var targetPos = Managers.Tile.GetCellCenterToWorld(Define.Tilemap.Ground, cellPos);
        transform.position = Vector2.MoveTowards(transform.position, targetPos, (_moveSpeed * Time.deltaTime));
        cellPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Ground, transform.position);

        if (IsExit)
        {
            if (checkOnBuilding(cellPos))
            {
                return;
            }

            checkOnRoad(cellPos);
        }

        if (PrevPos != cellPos)
        {
            PrevPos = cellPos;
            IsExit = true;
        }
    }

    private bool checkOnBuilding(Vector3Int currentPos)
    {
        var tile = Managers.Tile.GetTile(Define.Tilemap.Building, currentPos) as Tile;
        if (tile == null)
        {
            return false;
        }

        tile.gameObject.GetComponent<BaseBuilding>().EnterTheBuilding(this);
        return true;
    }

    private void checkOnRoad(Vector3Int currentPos)
    {
        if (PrevPos != currentPos)
        {
            _isChangeRoad = true;
        }

        var go = Managers.Tile.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(currentPos);
        if (go == null)
        {
            turnAround();
        }
        else
        {
            if (!_isChangeRoad)
            {
                return;
            }

            Vector2 targetPos = Managers.Tile.GetCellCenterToWorld(Define.Tilemap.Ground, currentPos);
            var road = go.GetComponent<Road>();
            switch (road.RoadType)
            {
                case Define.RoadType.TU:
                case Define.RoadType.TD:
                    if (isMoveTypeUD())
                    {
                        if (Vector3.Distance(transform.position, targetPos) <= TURN_GAP)
                        {
                            turnAround();
                            _isChangeRoad = false;
                        }
                    }
                    break;
                case Define.RoadType.TL:
                case Define.RoadType.TR:
                    if (!isMoveTypeUD())
                    {
                        if (Vector3.Distance(transform.position, targetPos) <= TURN_GAP)
                        {
                            turnAround();
                            _isChangeRoad = false;
                        }
                    }
                    break;

                case Define.RoadType.CUL:
                    if (isMoveTypeUD())
                    {
                        if (Vector2.Distance(transform.position, targetPos) <= TURN_GAP)
                        {
                            MoveType = Define.MoveType.Left;
                            _isChangeRoad = false;
                        }
                    }
                    else
                    {
                        if (Vector2.Distance(transform.position, targetPos) <= TURN_GAP)
                        {
                            MoveType = Define.MoveType.Down;
                            _isChangeRoad = false;
                        }
                    }
                    break;
                case Define.RoadType.CUR:
                    if (isMoveTypeUD())
                    {
                        if (Vector2.Distance(transform.position, targetPos) <= TURN_GAP)
                        {
                            MoveType = Define.MoveType.Right;
                            _isChangeRoad = false;
                        }
                    }
                    else
                    {
                        if (Vector2.Distance(transform.position, targetPos) <= TURN_GAP)
                        {
                            MoveType = Define.MoveType.Down;
                            _isChangeRoad = false;
                        }
                    }
                    break;
                case Define.RoadType.CDR:
                    if (isMoveTypeUD())
                    {
                        if (Vector2.Distance(transform.position, targetPos) <= TURN_GAP)
                        {
                            MoveType = Define.MoveType.Right;
                            _isChangeRoad = false;
                        }
                    }
                    else
                    {
                        if (Vector2.Distance(transform.position, targetPos) <= TURN_GAP)
                        {
                            MoveType = Define.MoveType.Up;
                            _isChangeRoad = false;
                        }
                    }
                    break;
                case Define.RoadType.CDL:
                    if (isMoveTypeUD())
                    {
                        if (Vector2.Distance(transform.position, targetPos) <= TURN_GAP)
                        {
                            MoveType = Define.MoveType.Left;
                            _isChangeRoad = false;
                        }
                    }
                    else
                    {
                        if (Vector2.Distance(transform.position, targetPos) <= TURN_GAP)
                        {
                            MoveType = Define.MoveType.Up;
                            _isChangeRoad = false;
                        }
                    }
                    break;
            }
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
