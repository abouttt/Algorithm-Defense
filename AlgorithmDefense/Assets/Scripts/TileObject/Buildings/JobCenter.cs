using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobCenter : BaseBuilding
{
    [SerializeField]
    private Define.Job _jobType = Define.Job.None;
    private Define.Move _inputDir = Define.Move.None;
    private Define.Move[] _outputDirs;
    private int _outputDirIndex = 0;

    public void ChangeInputDir(Vector3Int pos)
    {
        var dir = pos - Managers.Tile.GetWorldToCell(Define.Tilemap.Building, transform.position);
        if (dir == Vector3Int.right)
        {
            _inputDir = Define.Move.Right;
        }
        else if (dir == Vector3Int.left)
        {
            _inputDir = Define.Move.Left;
        }
        else if (dir == Vector3Int.up)
        {
            _inputDir = Define.Move.Up;
        }
        else if (dir == Vector3Int.down)
        {
            _inputDir = Define.Move.Down;
        }
    }

    public void ChangeOutputDir()
    {
        _outputDirIndex = (_outputDirIndex + 1) >= 4 ? 0 : _outputDirIndex + 1;
        transform.Rotate(new Vector3(0f, 0f, -90.0f));
    }

    public override void EnterTheBuilding(CitizenController citizen)
    {
        EnqueueCitizen(citizen);
    }

    protected override IEnumerator ReleaseCitizen()
    {
        while (true)
        {
            if (_citizenOrderQueue.Count == 0)
            {
                _isReleasing = false;
                yield break;
            }

            yield return new WaitForSeconds(_releaseTime);

            bool hasInputRoad = HasRoadNextPosition(_inputDir);
            bool hasOutputRoad = HasRoadNextPosition(_outputDirs[_outputDirIndex]);

            if (!hasInputRoad && !hasOutputRoad)
            {
                continue;
            }

            var citizen = DequeueCitizen();

            if (citizen.Data.JobType == Define.Job.None)
            {
                var go = Managers.Resource.Instantiate($"{Define.BATTILE_UNIT_PATH}{citizen.Data.CitizenType}_{_jobType}");
                go.transform.position = transform.position;

                var newCitizen = go.GetOrAddComponent<CitizenController>();
                newCitizen.Data = citizen.Data;
                newCitizen.Data.JobType = _jobType;

                Managers.Resource.Destroy(citizen.gameObject);
                citizen = newCitizen;
                citizen.GetComponent<CitizenController>().enabled = true;
                citizen.GetComponent<UnitManager>().enabled = false;
                citizen.GetComponent<UnitAI>().enabled = false;
            }

            if (hasOutputRoad)
            {
                citizen.Data.MoveType = _outputDirs[_outputDirIndex];
            }
            else
            {
                citizen.Data.MoveType = _inputDir;
            }

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
