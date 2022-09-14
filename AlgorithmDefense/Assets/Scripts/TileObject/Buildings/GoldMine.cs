using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMine : BaseBuilding
{
    public Define.Move MoveType = Define.Move.None;
    public int GoldIncrease;

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
        var saveData = JsonUtility.FromJson<BuildingSaveData>(Managers.Data.GatewaySaveDatas.Dequeue());

        JsonUtility.FromJsonOverwrite(saveData.Data, this);
        _citizenOrderQueue =
            JsonUtility.FromJson<SerializationQueue<CitizenOrderQueueData>>(saveData.OrderQueue).ToQueue();

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

            if ((MoveType == Define.Move.None) ||
                (citizen.Data.JobType != Define.Job.None))
            {
                citizen.SetReverseMoveType();
            }
            else
            {
                Managers.Data.RuntimeDatas.Gold += GoldIncrease;
                Debug.Log(Managers.Data.RuntimeDatas.Gold);

                if (IsRoadNextPosition(MoveType))
                {
                    citizen.Data.MoveType = MoveType;
                }
                else
                {
                    citizen.SetReverseMoveType();
                }
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
