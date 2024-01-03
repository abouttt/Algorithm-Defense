using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenUnitController : BaseUnitController
{
    public CitizenUnitData Data = new();

    private void Update()
    {
        Move();
    }

    public void SetNextDestination(Vector3 startPos)
    {
        var cellPos = TileManager.GetInstance.GetWorldToCell(Define.Tilemap.Ground, startPos);
        switch (Data.MoveType)
        {
            case Define.Move.Down:
                cellPos.y--;
                break;
            case Define.Move.Up:
                cellPos.y++;
                break;
            case Define.Move.Right:
                cellPos.x++;
                break;
            case Define.Move.Left:
                cellPos.x--;
                break;
        }

        Data.Destination = TileManager.GetInstance.GetCellCenterToWorld(Define.Tilemap.Ground, cellPos);
        SetMoveAnimation(Data.MoveType);
    }

    public void SetReverseMoveType()
    {
        switch (Data.MoveType)
        {
            case Define.Move.Right:
                Data.MoveType = Define.Move.Left;
                break;
            case Define.Move.Left:
                Data.MoveType = Define.Move.Right;
                break;
            case Define.Move.Up:
                Data.MoveType = Define.Move.Down;
                break;
            case Define.Move.Down:
                Data.MoveType = Define.Move.Up;
                break;
        }
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, Data.Destination, (Data.MoveSpeed * Time.deltaTime));

        var cellPos = TileManager.GetInstance.GetWorldToCell(Define.Tilemap.Ground, transform.position);
        if (Vector2.Distance(transform.position, Data.Destination) <= 0.01f)
        {
            CheckRoad(cellPos);
            SetNextDestination(transform.position);
        }

        CheckBuilding(cellPos);
    }

    private void CheckBuilding(Vector3Int pos)
    {
        var building = Util.GetBuilding<BaseBuilding>(pos);
        if (building)
        {
            building.EnterTheBuilding(this);
        }
    }

    private void CheckRoad(Vector3Int pos)
    {
        var road = Util.GetRoad(Define.Tilemap.Road, pos);
        if (!road)
        {
            return;
        }

        switch (road.RoadType)
        {
            case Define.Road.B:
            case Define.Road.BU:
            case Define.Road.BD:
            case Define.Road.BL:
            case Define.Road.BR:
                SetReverseMoveType();
                break;
            case Define.Road.TU:
            case Define.Road.TD:
                if (IsMoveTypeUD())
                {
                    SetReverseMoveType();
                }
                break;
            case Define.Road.TL:
            case Define.Road.TR:
                if (!IsMoveTypeUD())
                {
                    SetReverseMoveType();
                }
                break;
            case Define.Road.CUL:
                Data.MoveType = IsMoveTypeUD() ? Define.Move.Left : Define.Move.Down;
                break;
            case Define.Road.CUR:
                Data.MoveType = IsMoveTypeUD() ? Define.Move.Right : Define.Move.Down;
                break;
            case Define.Road.CDR:
                Data.MoveType = IsMoveTypeUD() ? Define.Move.Right : Define.Move.Up;
                break;
            case Define.Road.CDL:
                Data.MoveType = IsMoveTypeUD() ? Define.Move.Left : Define.Move.Up;
                break;
        }
    }

    private bool IsMoveTypeUD() => (Data.MoveType == Define.Move.Up) || (Data.MoveType == Define.Move.Down);
}
