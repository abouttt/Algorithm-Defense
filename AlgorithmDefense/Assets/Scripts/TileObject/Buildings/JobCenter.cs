using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobCenter : BaseBuilding
{
    [SerializeField]
    private Define.Job _jobType = Define.Job.None;

    private Queue<CitizenData> _citizenOrderQueue = new();
    private bool _isReleasing;

    private Define.Move[] _outputDirs;
    private int _outputDirIndex = 0;

    public void ChangeOutputDir()
    {
        _outputDirIndex = (_outputDirIndex + 1) >= 4 ? 0 : _outputDirIndex + 1;
        transform.Rotate(new Vector3(0f, 0f, -90.0f));
    }

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

            bool hasOutputRoad = HasRoadNextPosition(_outputDirs[_outputDirIndex]);

            if (!hasOutputRoad)
            {
                continue;
            }

            var citizenData = _citizenOrderQueue.Dequeue();

            var go = Managers.Resource.Instantiate($"{Define.CITIZEN_PATH}{citizenData.CitizenType}Citizen_{_jobType}");

            var citizen = go.GetComponent<CitizenController>();
            citizen.transform.position = transform.position;
            citizen.Data.MoveType = _outputDirs[_outputDirIndex];
            citizen.Data.JobType = _jobType;

            SetUnitPosition(go, citizen.Data.MoveType);
            citizen.SetNextDestination(transform.position);
        }

        _isReleasing = false;
    }

    protected override void Init()
    {
        _outputDirs = new Define.Move[]
        {
            Define.Move.Up,
            Define.Move.Right,
            Define.Move.Down,
            Define.Move.Left
        };

        HasUI = false;
    }
}
