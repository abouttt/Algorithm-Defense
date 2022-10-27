using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleGate : BaseBuilding
{
    private Queue<CitizenData> _citizenOrderQueue = new();
    private bool _isReleasing;

    public override void EnterTheBuilding(CitizenController citizen)
    {
        _citizenOrderQueue.Enqueue(citizen.Data);

        Managers.Resource.Destroy(citizen.gameObject);

        if (!_isReleasing)
        {
            _isReleasing = true;
            StartCoroutine(ReleaseCitizen());
        }
    }

    protected IEnumerator ReleaseCitizen()
    {
        while (_citizenOrderQueue.Count > 0)
        {
            yield return new WaitForSeconds(_releaseTime);

            var citizenData = _citizenOrderQueue.Dequeue();

            if (citizenData.JobType != Define.Job.None)
            {
                var go = Managers.Resource.Instantiate($"{Define.BATTILE_UNIT_PATH}{citizenData.CitizenType}_{citizenData.JobType}");
                var unitManager = go.GetComponent<UnitManager>();
                unitManager.CurrentHP = unitManager.MaxHP;
                SetUnitPosition(go, Define.Move.Up);
            }
            else
            {
                var go = Managers.Resource.Instantiate($"{Define.CITIZEN_PATH}{citizenData.CitizenType}Citizen");
                var citizen = go.GetComponent<CitizenController>();
                citizen.SetReverseMoveType();
                SetUnitPosition(go, citizen.Data.MoveType);
                citizen.SetNextDestination(transform.position);
            }
        }

        _isReleasing = false;
    }

    protected override void Init()
    {
        HasUI = false;
    }
}
