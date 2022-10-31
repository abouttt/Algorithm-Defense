using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleGate : BaseBuilding
{
    private Queue<UnitData> _citizenOrderQueue = new();
    private Queue<UnitData> _battleUnitOrderQueue = new();
    private bool _isCitizenReleasing;
    private bool _isBattleUnitReleasing;

    public override void EnterTheBuilding(UnitController unit)
    {
        if (unit.Data.JobType == Define.Job.None)
        {
            _citizenOrderQueue.Enqueue(unit.Data);
            if (!_isCitizenReleasing)
            {
                _isCitizenReleasing = true;
                StartCoroutine(ReleaseCitizen());
            }
        }
        else
        {
            _battleUnitOrderQueue.Enqueue(unit.Data);
            if (!_isBattleUnitReleasing)
            {
                _isBattleUnitReleasing = true;
                StartCoroutine(ReleaseBattleUnit());
            }
        }

        Managers.Resource.Destroy(unit.gameObject);
    }

    private IEnumerator ReleaseCitizen()
    {
        while (_citizenOrderQueue.Count > 0)
        {
            yield return new WaitForSeconds(_releaseTime);

            if (!HasRoadNextPosition(Define.Move.Down))
            {
                continue;
            }

            var data = _citizenOrderQueue.Dequeue();

            var go = Managers.Resource.Instantiate($"{Define.CITIZEN_PREFAB_PATH}{data.CitizenType}Citizen");

            var unit = go.GetComponent<UnitController>();
            unit.Data.MoveType = Define.Move.Down;
            SetUnitPosition(unit, unit.Data.MoveType);
            unit.SetNextDestination(transform.position);
            unit.Move();
        }

        _isCitizenReleasing = false;
    }

    private IEnumerator ReleaseBattleUnit()
    {
        while (_battleUnitOrderQueue.Count > 0)
        {
            yield return new WaitForSeconds(_releaseTime);

            var data = _battleUnitOrderQueue.Dequeue();

            var go = Managers.Resource.Instantiate($"{Define.BATTILE_UNIT_PREFAB_PATH}{data.CitizenType}_{data.JobType}");
            go.transform.position = transform.position;

            var unit = go.GetComponent<UnitController>();
            unit.Data.MoveType = Define.Move.Up;
            unit.Data.CurrentHp = unit.Data.MaxHp;
            SetUnitPosition(unit, unit.Data.MoveType);
            unit.Move();
        }

        _isBattleUnitReleasing = false;
    }

    protected override void Init()
    {
        HasUI = false;
    }
}
