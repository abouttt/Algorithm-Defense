using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnitController : BaseUnitController
{
    public BattleUnitData Data = new();

    private BaseUnitController _targetUnit;
    private BaseBuilding _targetBuilding;

    private void Update()
    {
        _targetUnit = null;
        _targetBuilding = null;

        if (Data.CurrentHp <= 0)
        {

            return;
        }

        CheckTargetUnit();
        if (_targetUnit)
        {

            return;
        }

        CheckTargetBuilding();
        if (_targetBuilding)
        {

            return;
        }

        Move();
    }

    private void Move()
    {
        Vector2 dir = (Data.MoveType == Define.Move.Up) ? Vector2.up :
                      (Data.MoveType == Define.Move.Down) ? Vector2.down : Vector2.zero;

        transform.Translate(dir * (Data.MoveSpeed * Time.deltaTime));
    }

    private RaycastHit2D GetRayHit2DInfo(LayerMask layer)
    {
        Vector2 dir = (Data.MoveType == Define.Move.Up) ? Vector2.up : Vector2.down;
        return Physics2D.Raycast(transform.position, dir, Data.Range, layer);
    }

    private void CheckTargetUnit()
    {
        var hit = GetRayHit2DInfo(Data.TargetLayer);
        if (hit.collider != null)
        {
            var unit = hit.collider.gameObject.GetComponent<BattleUnitController>();
            if (unit.Data.CurrentHp > 0)
            {
                _targetUnit = unit;
            }
        }
    }

    private void CheckTargetBuilding()
    {
        LayerMask layer = (Data.TargetLayer == LayerMask.NameToLayer("Human")) ? LayerMask.GetMask("Dungeon") : LayerMask.GetMask("Castle");
        var hit = GetRayHit2DInfo(layer);
        if (hit.collider != null)
        {
            _targetBuilding = hit.collider.GetComponent<BaseBuilding>();
        }
    }
}
