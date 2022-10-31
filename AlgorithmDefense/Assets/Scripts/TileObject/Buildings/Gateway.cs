using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gateway : BaseBuilding
{
    public Dictionary<Define.Citizen, Define.Move> DirectionCondition { get; private set; }

    private Queue<UnitData> _redOrderQueue = new();
    private Queue<UnitData> _greenOrderQueue = new();
    private Queue<UnitData> _blueOrderQueue = new();

    private bool _isRedReleasing;
    private bool _isGreenReleasing;
    private bool _isBlueReleasing;

    public override void EnterTheBuilding(UnitController unit)
    {
        switch (unit.Data.CitizenType)
        {
            case Define.Citizen.Red:
                _redOrderQueue.Enqueue(unit.Data);
                if (!_isRedReleasing)
                {
                    _isRedReleasing = true;
                    StartCoroutine(ReleaseRedCitizen());
                }
                break;
            case Define.Citizen.Green:
                _greenOrderQueue.Enqueue(unit.Data);
                if (!_isGreenReleasing)
                {
                    _isGreenReleasing = true;
                    StartCoroutine(ReleaseGreenCitizen());
                }
                break;
            case Define.Citizen.Blue:
                _blueOrderQueue.Enqueue(unit.Data);
                if (!_isBlueReleasing)
                {
                    _isBlueReleasing = true;
                    StartCoroutine(ReleaseBlueCitizen());
                }
                break;
        }

        Managers.Resource.Destroy(unit.gameObject);
    }

    private IEnumerator ReleaseRedCitizen()
    {
        while (_redOrderQueue.Count > 0)
        {
            yield return new WaitForSeconds(_releaseTime);

            if (!HasRoadNextPosition(DirectionCondition[Define.Citizen.Red]))
            {
                continue;
            }

            Release(_redOrderQueue);
        }

        _isRedReleasing = false;
    }

    private IEnumerator ReleaseGreenCitizen()
    {
        while (_greenOrderQueue.Count > 0)
        {
            yield return new WaitForSeconds(_releaseTime);

            if (!HasRoadNextPosition(DirectionCondition[Define.Citizen.Green]))
            {
                continue;
            }

            Release(_greenOrderQueue);
        }

        _isGreenReleasing = false;
    }

    private IEnumerator ReleaseBlueCitizen()
    {
        while (_blueOrderQueue.Count > 0)
        {
            yield return new WaitForSeconds(_releaseTime);

            if (!HasRoadNextPosition(DirectionCondition[Define.Citizen.Blue]))
            {
                continue;
            }

            Release(_blueOrderQueue);
        }

        _isBlueReleasing = false;
    }


    private void Release(Queue<UnitData> unitOrderQueue)
    {
        var unit = DequeueCitizen(unitOrderQueue);
        unit.Data.MoveType = DirectionCondition[unit.Data.CitizenType];
        SetUnitPosition(unit.GetComponent<UnitController>(), unit.Data.MoveType);
        unit.SetNextDestination(transform.position);
    }

    private UnitController DequeueCitizen(Queue<UnitData> citizenOrderQueue)
    {
        UnitData unitData = citizenOrderQueue.Dequeue();

        GameObject go = null;
        if (unitData.JobType == Define.Job.None)
        {
            go = Managers.Resource.Instantiate($"{Define.CITIZEN_PREFAB_PATH}{unitData.CitizenType}Citizen");
        }
        else
        {
            go = Managers.Resource.Instantiate(
                $"{Define.CITIZEN_PREFAB_PATH}" +
                $"{unitData.CitizenType}Citizen_{unitData.JobType}");
        }

        var unit = go.GetComponent<UnitController>();
        unit.Data = unitData;

        return unit;
    }

    protected override void Init()
    {
        if (DirectionCondition == null)
        {
            DirectionCondition = new Dictionary<Define.Citizen, Define.Move>()
            {
                { Define.Citizen.Red, Define.Move.None },
                { Define.Citizen.Green, Define.Move.None },
                { Define.Citizen.Blue, Define.Move.None },
            };
        }

        HasUI = true;
    }
}
