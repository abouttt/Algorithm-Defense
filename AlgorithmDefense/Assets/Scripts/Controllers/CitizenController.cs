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
    private Define.RoadType _prevRoadType;

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Citizen;
        _state = Define.State.Moving;
        _groundTilemap = Managers.Tile.GetTilemap(Define.Tilemap.Ground);
        _buildingTilemap = Managers.Tile.GetTilemap(Define.Tilemap.Building);
    }

    protected override void UpdateMoving()
    {
        var nextPos = new Vector3();

        switch (MoveType)
        {
            case Define.MoveType.Down:
                nextPos = (Vector3.right * 2) + Vector3.down;
                break;
            case Define.MoveType.Up:
                nextPos = (Vector3.left * 2) + Vector3.up;
                break;
            case Define.MoveType.Right:
                nextPos = Vector3.up + (Vector3.right * 2);
                break;
            case Define.MoveType.Left:
                nextPos = Vector3.down + (Vector3.left * 2);
                break;
        }

        transform.position += nextPos * (_moveSpeed * Time.deltaTime);

        checkOnBuilding(transform.position);

        if (IsExit)
        {
            checkOnRoad(transform.position);
        }
    }

    private void checkOnRoad(Vector3 currentPos)
    {
        var cellPos = _groundTilemap.WorldToCell(currentPos);
        var tile = _groundTilemap.GetTile(cellPos) as Tile;

        if (tile.gameObject == null)
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
}
