using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialJobCenter : BaseBuilding
{
    [SerializeField]
    private Define.Job _jobType = Define.Job.None;

    private Queue<CitizenUnitData> _citizenOrderQueue = new();

    public override void EnterTheBuilding(CitizenUnitController citizen)
    {
        _citizenOrderQueue.Enqueue(citizen.Data);
        Managers.Resource.Destroy(citizen.gameObject);
    }

    public void Release()
    {
        var unitData = _citizenOrderQueue.Dequeue();

        var go = Managers.Resource.Instantiate($"{Define.CITIZEN_PREFAB_PATH}{unitData.CitizenType}Citizen_{_jobType}");

        var citizen = go.GetComponent<CitizenUnitController>();
        citizen.Data.MoveType = Define.Move.Up;
        citizen.Data.JobType = _jobType;
        SetUnitPosition(citizen, citizen.Data.MoveType);
        citizen.SetNextDestination(transform.position);
    }

    public int GetCitizenDataCount()
    {
        return _citizenOrderQueue.Count;
    }

    public Define.Citizen GetCitizenType()
    {
        return _citizenOrderQueue.Peek().CitizenType;
    }

    public void Clear()
    {
        _citizenOrderQueue.Clear();
    }

    protected override void Init()
    {
        HasUI = false;
    }
}
