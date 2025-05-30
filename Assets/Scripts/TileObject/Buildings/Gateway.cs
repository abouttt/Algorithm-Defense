using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gateway : BaseBuilding
{
    public Dictionary<Define.Citizen, Define.Move> DirectionCondition { get; private set; }

    private Queue<CitizenUnitData> _redOrderQueue = new();
    private Queue<CitizenUnitData> _greenOrderQueue = new();
    private Queue<CitizenUnitData> _blueOrderQueue = new();

    private bool _isRedReleasing;
    private bool _isGreenReleasing;
    private bool _isBlueReleasing;

    public override void EnterTheBuilding(CitizenUnitController citizen)
    {
        switch (citizen.Data.CitizenType)
        {
            case Define.Citizen.Red:
                _redOrderQueue.Enqueue(citizen.Data);
                if (!_isRedReleasing)
                {
                    _isRedReleasing = true;
                    StartCoroutine(ReleaseRedCitizen());
                }
                break;
            case Define.Citizen.Green:
                _greenOrderQueue.Enqueue(citizen.Data);
                if (!_isGreenReleasing)
                {
                    _isGreenReleasing = true;
                    StartCoroutine(ReleaseGreenCitizen());
                }
                break;
            case Define.Citizen.Blue:
                _blueOrderQueue.Enqueue(citizen.Data);
                if (!_isBlueReleasing)
                {
                    _isBlueReleasing = true;
                    StartCoroutine(ReleaseBlueCitizen());
                }
                break;
        }

        Managers.Resource.Destroy(citizen.gameObject);
    }

    public bool HasCitizenData()
    {
        if (_redOrderQueue.Count > 0)
        {
            return true;
        }
        else if (_greenOrderQueue.Count > 0)
        {
            return true;
        }
        else if (_blueOrderQueue.Count > 0)
        {
            return true;
        }

        return false;
    }

    public void Clear()
    {
        _redOrderQueue.Clear();
        _greenOrderQueue.Clear();
        _blueOrderQueue.Clear();
    }

    private IEnumerator ReleaseRedCitizen()
    {
        while (_redOrderQueue.Count > 0)
        {
            yield return YieldCache.WaitForSeconds(_releaseTime);

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
            yield return YieldCache.WaitForSeconds(_releaseTime);

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
            yield return YieldCache.WaitForSeconds(_releaseTime);

            if (!HasRoadNextPosition(DirectionCondition[Define.Citizen.Blue]))
            {
                continue;
            }

            Release(_blueOrderQueue);
        }

        _isBlueReleasing = false;
    }


    private void Release(Queue<CitizenUnitData> unitOrderQueue)
    {
        var citizen = DequeueCitizen(unitOrderQueue);
        citizen.Data.MoveType = DirectionCondition[citizen.Data.CitizenType];
        SetUnitPosition(citizen, citizen.Data.MoveType);
        citizen.SetNextDestination(transform.position);
    }

    private CitizenUnitController DequeueCitizen(Queue<CitizenUnitData> citizenOrderQueue)
    {
        var data = citizenOrderQueue.Dequeue();

        GameObject go = null;
        if (data.JobType == Define.Job.None)
        {
            go = Managers.Resource.Instantiate($"{Define.CITIZEN_PREFAB_PATH}{data.CitizenType}Citizen");
        }
        else
        {
            go = Managers.Resource.Instantiate(
                $"{Define.CITIZEN_PREFAB_PATH}" +
                $"{data.CitizenType}Citizen_{data.JobType}");
        }

        var citizen = go.GetComponent<CitizenUnitController>();
        citizen.Data = data;

        return citizen;
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
