using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

public class JobCenter : BaseBuilding
{
    [SerializeField]
    private Define.Job _jobType = Define.Job.None;
    private Define.Move[] _moveTypes;
    private int _moveTypesIndex = 0;

    public void ChangeMoveType()
    {
        _moveTypesIndex = (_moveTypesIndex + 1) >= 4 ? 0 : _moveTypesIndex + 1;
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

            if (!HasNeighborRoad())
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

            if (HasRoadNextPosition(_moveTypes[_moveTypesIndex]))
            {
                citizen.Data.MoveType = _moveTypes[_moveTypesIndex];
            }
            else
            {
                citizen.SetReverseMoveType();
            }

            SetCitizenPosition(citizen);
            citizen.SetNextDestination(transform.position);
        }
    }

    protected override void Init()
    {
        _moveTypes = new Define.Move[]
        {
            Define.Move.Up,
            Define.Move.Right,
            Define.Move.Down,
            Define.Move.Left
        };

        HasUI = false;
    }
}
