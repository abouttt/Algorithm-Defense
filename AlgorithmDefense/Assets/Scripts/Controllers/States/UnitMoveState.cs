using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UnitMoveState : MonoBehaviour, IUnitState
{
    private UnitController _unit;

    private UnitController _targetUnit;
    private BaseBuilding _targetBuilding;

    public void Handle(UnitController controller)
    {
        if (!_unit)
        {
            _unit = controller;
        }

        _unit.CurrentSpeed = _unit.Data.MoveSpeed;
    }

    private void Update()
    {
        if (!_unit)
        {
            return;
        }

        if (_unit.CurrentSpeed <= 0)
        {
            return;
        }

        _unit.SetMoveAnimation();

        if (_unit.Data.IsCitizen)
        {
            transform.position = Vector2.MoveTowards(transform.position, _unit.Data.Destination, (_unit.CurrentSpeed * Time.deltaTime));

            var cellPos = TileManager.GetInstance.GetWorldToCell(Define.Tilemap.Ground, transform.position);
            if (Vector2.Distance(transform.position, _unit.Data.Destination) <= 0.01f)
            {
                CheckRoad(cellPos);
                _unit.SetNextDestination(transform.position);
            }

            CheckBuilding(cellPos);
        }
        else
        {
            CheckTargetUnit();
            if (_targetUnit)
            {
                _unit.Attack();
                return;
            }

            CheckTargetBuilding();
            if (_targetBuilding)
            {
                _unit.Attack();
                return;
            }

            Vector2 dir = (_unit.Data.MoveType == Define.Move.Up) ? Vector2.up :
                            (_unit.Data.MoveType == Define.Move.Down) ? Vector2.down : Vector2.zero;

            transform.Translate(dir * (_unit.CurrentSpeed * Time.deltaTime));
        }
    }

    private void CheckBuilding(Vector3Int pos)
    {
        var building = Util.GetBuilding<BaseBuilding>(pos);
        if (building)
        {
            building.EnterTheBuilding(_unit);
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
                _unit.SetReverseMoveType();
                break;
            case Define.Road.TU:
            case Define.Road.TD:
                if (IsMoveTypeUD())
                {
                    _unit.SetReverseMoveType();
                }
                break;
            case Define.Road.TL:
            case Define.Road.TR:
                if (!IsMoveTypeUD())
                {
                    _unit.SetReverseMoveType();
                }
                break;
            case Define.Road.CUL:
                _unit.Data.MoveType = IsMoveTypeUD() ? Define.Move.Left : Define.Move.Down;
                break;
            case Define.Road.CUR:
                _unit.Data.MoveType = IsMoveTypeUD() ? Define.Move.Right : Define.Move.Down;
                break;
            case Define.Road.CDR:
                _unit.Data.MoveType = IsMoveTypeUD() ? Define.Move.Right : Define.Move.Up;
                break;
            case Define.Road.CDL:
                _unit.Data.MoveType = IsMoveTypeUD() ? Define.Move.Left : Define.Move.Up;
                break;
        }
    }

    private RaycastHit2D GetRayHit2DInfo(LayerMask layer)
    {
        Vector2 dir = (_unit.Data.MoveType == Define.Move.Up) ? Vector2.up : Vector2.down;
        return Physics2D.Raycast(transform.position, dir, _unit.Data.Range, layer);
    }

    private void CheckTargetUnit()
    {
        LayerMask layer = _unit.Data.IsMonster ? LayerMask.GetMask("Human") : LayerMask.GetMask("Monster");
        var hit = GetRayHit2DInfo(layer);
        if (hit.collider != null)
        {
            var unit = hit.collider.gameObject.GetComponent<UnitController>();
            if (unit.Data.CurrentHp > 0)
            {
                _targetUnit = unit;
            }
        }
    }

    private void CheckTargetBuilding()
    {
        LayerMask layer = _unit.Data.IsMonster ? LayerMask.GetMask("Castle") : LayerMask.GetMask("Dungeon");
        var hit = GetRayHit2DInfo(layer);
        if (hit.collider != null)
        {
            _targetBuilding = hit.collider.GetComponent<BaseBuilding>();
        }
    }

    private bool IsMoveTypeUD() => (_unit.Data.MoveType == Define.Move.Up) || (_unit.Data.MoveType == Define.Move.Down);
}
