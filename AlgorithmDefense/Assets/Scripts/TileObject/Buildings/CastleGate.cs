using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleGate : BaseBuilding
{
    private Queue<CitizenData> _citizenOrderQueue = new();
    private Queue<CitizenData> _battleUnitOrderQueue = new();
    private bool _isCitizenReleasing;
    private bool _isBattleUnitReleasing;

    public override void EnterTheBuilding(CitizenController citizen)
    {
        if (citizen.Data.JobType == Define.Job.None)
        {
            _citizenOrderQueue.Enqueue(citizen.Data);
            if (!_isCitizenReleasing)
            {
                _isCitizenReleasing = true;
                StartCoroutine(ReleaseCitizen());
            }
        }
        else
        {
            _battleUnitOrderQueue.Enqueue(citizen.Data);
            if (!_isBattleUnitReleasing)
            {
                _isBattleUnitReleasing = true;
                StartCoroutine(ReleaseBattleUnit());
            }
        }

        Managers.Resource.Destroy(citizen.gameObject);
    }

    private IEnumerator ReleaseCitizen()
    {
        while (_citizenOrderQueue.Count > 0)
        {
            yield return new WaitForSeconds(_releaseTime);

            var data = _citizenOrderQueue.Dequeue();

            if (!HasRoadNextPosition(Define.Move.Down))
            {
                _citizenOrderQueue.Enqueue(data);
                continue;
            }

            var go = Managers.Resource.Instantiate($"{Define.CITIZEN_PREFAB_PATH}{data.CitizenType}Citizen");

            var citizen = go.GetComponent<CitizenController>();
            citizen.Data.MoveType = Define.Move.Down;
            SetUnitPosition(go, citizen.Data.MoveType);
            citizen.SetNextDestination(transform.position);
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

            var um = go.GetComponent<UnitManager>();
            um.CurrentHP = um.MaxHP;

            SetUnitPosition(go, Define.Move.Up);
        }

        _isBattleUnitReleasing = false;
    }

    protected override void Init()
    {
        HasUI = false;
    }
}
