using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CitizenController : MonoBehaviour
{
    public CitizenData Data = new();

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateMoving();
    }

    public void SetNextDestination(Vector3 startPosition)
    {
        var cellPos = TileManager.GetInstance.GetWorldToCell(Define.Tilemap.Ground, startPosition);
        switch (Data.MoveType)
        {
            case Define.Move.Down:
                cellPos.y--;
                _animator.SetFloat("Hor", 0);
                _animator.SetFloat("Ver", -1);
                break;
            case Define.Move.Up:
                cellPos.y++;
                _animator.SetFloat("Hor", 0);
                _animator.SetFloat("Ver", 1);
                break;
            case Define.Move.Right:
                cellPos.x++;
                _animator.SetFloat("Hor", 1);
                _animator.SetFloat("Ver", 0);
                break;
            case Define.Move.Left:
                cellPos.x--;
                _animator.SetFloat("Hor", -1);
                _animator.SetFloat("Ver", 0);
                break;
        }

        Data.Destination = TileManager.GetInstance.GetCellCenterToWorld(Define.Tilemap.Ground, cellPos);
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

    private void UpdateMoving()
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
        var go = TileManager.GetInstance.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(pos);
        if (!go)
        {
            return;
        }

        var road = go.GetComponent<Road>();
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
