using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CitizenController : BaseController
{
    [field: SerializeField]
    public Define.Citizen CitizenType { get; private set; } = Define.Citizen.None;
    public Define.MoveType MoveType { get; set; } = Define.MoveType.None;
    [field: SerializeField]
    public bool IsExit { get; set; } = false;

    [SerializeField]
    private float _moveSpeed = 0.0f;

    private Tilemap _globalTilemap = null;
    private Tilemap _roadTilemap = null;
    private Tilemap _buildingTilemap = null;

    private Vector3Int _prevPos;
    private bool _isChangeRoad = true;
    private const float TURN_GAP = 0.01f;

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Citizen;
        _state = Define.State.Moving;
        _globalTilemap = Managers.Tile.GetTilemap(Define.Tilemap.Global);
        _roadTilemap = Managers.Tile.GetTilemap(Define.Tilemap.Road);
        _buildingTilemap = Managers.Tile.GetTilemap(Define.Tilemap.Building);
    }

    protected override void UpdateMoving()
    {
        var cellPos = _globalTilemap.WorldToCell(transform.position);
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
                break;
            case Define.MoveType.Left:
                cellPos.x--;
                break;
        }

        transform.position = Vector3.MoveTowards(transform.position, _globalTilemap.GetCellCenterWorld(cellPos), (_moveSpeed * Time.deltaTime));

        checkOnBuilding(transform.position);

        if (IsExit)
        {
            checkOnRoad(transform.position);
        }
    }

    private void checkOnBuilding(Vector3 currentPos)
    {
        var cellPos = _buildingTilemap.WorldToCell(currentPos);
        var tile = _buildingTilemap.GetTile(cellPos) as Tile;

        if (tile == null)
        {
            IsExit = true;
            return;
        }

        if (!IsExit)
        {
            return;
        }

        if (tile.gameObject.name.Equals(Define.TileObject.Gateway.ToString()) ||
            tile.gameObject.name.Equals(Define.TileObject.StartGateway.ToString()))
        {
            tile.gameObject.GetComponent<Gateway>().EnterCitizen(this);
        }
        else if (tile.gameObject.name.Equals(Define.TileObject.EndGateway.ToString()))
        {
            tile.gameObject.GetComponent<EndGateway>().EnterCitizen(this);
        }
    }

    private void checkOnRoad(Vector3 currentPos)
    {
        var cellPos = _globalTilemap.WorldToCell(currentPos);
        if (_prevPos != cellPos)
        {
            _isChangeRoad = true;
            _prevPos = cellPos;
        }

        var go = _roadTilemap.GetInstantiatedObject(cellPos);
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

            var centerPos = _globalTilemap.GetCellCenterWorld(cellPos);
            var road = go.GetComponent<Road>();
            switch (road.RoadType)
            {
                case Define.RoadType.TW_U:
                case Define.RoadType.TW_D:
                case Define.RoadType.TW_L:
                case Define.RoadType.TW_R:
                    if (Vector3.Distance(transform.position, centerPos) <= TURN_GAP)
                    {
                        turnAround();
                        _isChangeRoad = false;
                    }
                    break;

                case Define.RoadType.Corner_U:
                    if (isMoveTypeUD())
                    {
                        if (Vector3.Distance(transform.position, centerPos) <= TURN_GAP)
                        {
                            MoveType = Define.MoveType.Left;
                            _isChangeRoad = false;
                        }
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, centerPos) <= TURN_GAP)
                        {
                            MoveType = Define.MoveType.Down;
                            _isChangeRoad = false;
                        }
                    }
                    break;
                case Define.RoadType.Corner_D:
                    if (isMoveTypeUD())
                    {
                        if (Vector3.Distance(transform.position, centerPos) <= TURN_GAP)
                        {
                            MoveType = Define.MoveType.Right;
                            _isChangeRoad = false;
                        }
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, centerPos) <= TURN_GAP)
                        {
                            MoveType = Define.MoveType.Up;
                            _isChangeRoad = false;
                        }
                    }
                    break;
                case Define.RoadType.Corner_L:
                    if (isMoveTypeUD())
                    {
                        if (Vector3.Distance(transform.position, centerPos) <= TURN_GAP)
                        {
                            MoveType = Define.MoveType.Right;
                            _isChangeRoad = false;
                        }
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, centerPos) <= TURN_GAP)
                        {
                            MoveType = Define.MoveType.Down;
                            _isChangeRoad = false;
                        }
                    }
                    break;
                case Define.RoadType.Corner_R:
                    if (isMoveTypeUD())
                    {
                        if (Vector3.Distance(transform.position, centerPos) <= TURN_GAP)
                        {
                            MoveType = Define.MoveType.Left;
                            _isChangeRoad = false;
                        }
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, centerPos) <= TURN_GAP)
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

    private bool isMoveTypeLR()
    {
        if ((MoveType == Define.MoveType.Left) || (MoveType == Define.MoveType.Right))
        {
            return true;
        }

        return false;
    }
}
