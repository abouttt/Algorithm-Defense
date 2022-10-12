using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

public class JobCenter : BaseBuilding
{
    public Define.Move MoveType = Define.Move.None;
    [SerializeField]
    private Define.Job JobType = Define.Job.None;

    public override void EnterTheBuilding(CitizenController citizen)
    {
        EnqueueCitizen(citizen);
    }

    public override void CreateSaveData()
    {
        string data = JsonUtility.ToJson(this, true);
        string q = JsonUtility.ToJson(new SerializationQueue<CitizenOrderQueueData>(_citizenOrderQueue), true);
        Managers.Data.GatewaySaveDatas.Enqueue(JsonUtility.ToJson(new BuildingSaveData(data, q), true));
    }

    public override void LoadSaveData()
    {
        var saveData = JsonUtility.FromJson<BuildingSaveData>(Managers.Data.JobTrainingCenterSaveDatas.Dequeue());

        JsonUtility.FromJsonOverwrite(saveData.Data, this);
        _citizenOrderQueue = JsonUtility.FromJson<SerializationQueue<CitizenOrderQueueData>>(saveData.OrderQueue).ToQueue();

        if (!_isReleasing)
        {
            _isReleasing = true;
            StartCoroutine(ReleaseCitizen());
        }
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

            var citizen = DequeueCitizen();

            if (MoveType != Define.Move.None &&
                citizen.Data.JobType == Define.Job.None)
            {
                var go = Managers.Resource.Instantiate($"{Define.BATTILE_UNIT_PATH}{citizen.Data.CitizenType}_{JobType}");
                go.transform.position = transform.position;

                var newCitizen = go.GetOrAddComponent<CitizenController>();
                newCitizen.Data = citizen.Data;
                newCitizen.Data.JobType = JobType;

                Managers.Resource.Destroy(citizen.gameObject);
                citizen = newCitizen;
                citizen.GetComponent<CitizenController>().enabled = true;
                citizen.GetComponent<UnitManager>().enabled = false;
                citizen.GetComponent<UnitAI>().enabled = false;

                if (IsRoadNextPosition(MoveType))
                {
                    citizen.Data.MoveType = MoveType;
                }
                else
                {
                    citizen.SetReverseMoveType();
                }
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
        HasUI = true;
    }
}
