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
        while (true)
        {
            if (_citizenOrderQueue.Count == 0)
            {
                _isReleasing = false;
                yield break;
            }

            yield return new WaitForSeconds(_releaseTime);

            bool hasOutputRoad = HasRoadNextPosition(_outputDirs[_outputDirIndex]);

            if (!hasOutputRoad)
            {
                continue;
            }

            var citizen = DequeueCitizen(_citizenOrderQueue);

            var go = Managers.Resource.Instantiate($"{Define.BATTILE_UNIT_PATH}{citizen.Data.CitizenType}_{_jobType}");
            go.transform.position = transform.position;

            var newCitizen = go.GetOrAddComponent<CitizenController>();
            newCitizen.Data = citizen.Data;
            newCitizen.Data.MoveType = _outputDirs[_outputDirIndex];
            newCitizen.Data.JobType = _jobType;

            Managers.Resource.Destroy(citizen.gameObject);
            citizen = newCitizen;
            citizen.GetComponent<CitizenController>().enabled = true;
            citizen.GetComponent<UnitManager>().enabled = false;
            citizen.GetComponent<UnitAI>().enabled = false;

            SetCitizenPosition(citizen);
            citizen.SetNextDestination(transform.position);
        }
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
