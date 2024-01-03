using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobCenter : BaseBuilding
{
    [SerializeField]
    private Define.Job _jobType = Define.Job.None;

    private Queue<CitizenUnitData> _citizenOrderQueue = new();
    private bool _isReleasing;

    private int _outputDir = 1;

    public void ChangeOutputDir()
    {
        _outputDir = (_outputDir + 1) > 4 ? 1 : _outputDir + 1;
        transform.Rotate(new Vector3(0f, 0f, -90.0f));
        Managers.Sound.Play("UI/Complete_The_Gateway_Setup", Define.Sound.Effect);
    }

    public override void EnterTheBuilding(CitizenUnitController citizen)
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
            yield return YieldCache.WaitForSeconds(_releaseTime);

            if (!HasRoadNextPosition((Define.Move)_outputDir))
            {
                continue;
            }

            var unitData = _citizenOrderQueue.Dequeue();

            var go = Managers.Resource.Instantiate($"{Define.CITIZEN_PREFAB_PATH}{unitData.CitizenType}Citizen_{_jobType}");

            var citizen = go.GetComponent<CitizenUnitController>();
            citizen.Data.MoveType = (Define.Move)_outputDir;
            citizen.Data.JobType = _jobType;
            SetUnitPosition(citizen, citizen.Data.MoveType);
            citizen.SetNextDestination(transform.position);
        }

        _isReleasing = false;
    }

    protected override void Init()
    {
        HasUI = false;
    }
}
