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

    private Tilemap _groundTilemap = null;
    private Tilemap _buildingTilemap = null;

    private Vector3Int _prevPos;
    private bool _isChangeRoad = false;
    private const float TURN_GAP = 0.3f;

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Citizen;
        _state = Define.State.Moving;
        _groundTilemap = Managers.Tile.GetTilemap(Define.Tilemap.Ground);
        _buildingTilemap = Managers.Tile.GetTilemap(Define.Tilemap.Building);
    }

    protected override void UpdateMoving()
    {
        var cellPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Ground, transform.position);
        var nextPos = new Vector3();

        switch (MoveType)
        {
            case Define.MoveType.Down:
                //nextPos = (Vector3.right * 2) + Vector3.down;
                cellPos.y--;
                break;
            case Define.MoveType.Up:
                //nextPos = (Vector3.left * 2) + Vector3.up;
                cellPos.y++;
                break;
            case Define.MoveType.Right:
                //nextPos = Vector3.up + (Vector3.right * 2);
                cellPos.x++;
                break;
            case Define.MoveType.Left:
                //nextPos = Vector3.down + (Vector3.left * 2);
                cellPos.x--;
                break;
        }

        //transform.position += nextPos * (_moveSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(
            transform.position, 
            Managers.Tile.GetCellToWorld(Define.Tilemap.Ground, cellPos) + new Vector3(0, 0.25f, 0), 
            (_moveSpeed * Time.deltaTime));

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

        if (tile == null || tile.gameObject == null)
        {
            IsExit = true;
            return;
        }

        if (tile.gameObject.name.Equals(Enum.GetName(typeof(Define.TileObject), Define.TileObject.Gateway)) ||
            tile.gameObject.name.Equals(Enum.GetName(typeof(Define.TileObject), Define.TileObject.StartGateway)))
        {
            tile.gameObject.GetComponent<Gateway>().EnterCitizen(this);
        }
        else if (tile.gameObject.name.Equals(Enum.GetName(typeof(Define.TileObject), Define.TileObject.EndGateway)))
        {
            tile.gameObject.GetComponent<EndGateway>().EnterCitizen(this);
        }
    }

    private void checkOnRoad(Vector3 currentPos)
    {
        var cellPos = _groundTilemap.WorldToCell(currentPos);
        if (_prevPos == cellPos)
        {
            return;
        }

        _prevPos = cellPos;
        _isChangeRoad = true;

        var go = _groundTilemap.GetInstantiatedObject(cellPos);
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

            var centerPos = Managers.Tile.GetCellCenterToWorld(Define.Tilemap.Ground, cellPos);
            centerPos += new Vector3(0, 0.25f, 1);
            var road = go.GetComponent<Road>();
            switch (road.RoadType)
            {
                case Define.RoadType.Road_UD:
                    break;
                case Define.RoadType.Road_LR:
                    break;
                case Define.RoadType.TWRoad_U:
                    break;
                case Define.RoadType.TWRoad_D:
                    break;
                case Define.RoadType.TWRoad_L:
                    break;
                case Define.RoadType.TWRoad_R:
                    break;
                case Define.RoadType.CornerRoad_U:
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
                case Define.RoadType.CornerRoad_D:
                    Debug.Log(centerPos);
                    Debug.Log(Vector3.Distance(transform.position, centerPos));
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
                case Define.RoadType.CornerRoad_L:
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
                case Define.RoadType.CornerRoad_R:
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
                case Define.RoadType.CrossRoad:
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
