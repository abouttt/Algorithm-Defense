using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : BaseBuilding
{
    public override void EnterTheBuilding(CitizenController citizen)
    {
        if (citizen.Data.JobType == Define.Job.None)
        {
            EnqueueCitizen(citizen);
            return;
        }

        Managers.Resource.Destroy(citizen.gameObject);
        CreateBattleUnit(citizen);
    }

    public override void CreateSaveData()
    {
        string data = JsonUtility.ToJson(this, true);
        string q = JsonUtility.ToJson(new SerializationQueue<CitizenOrderQueueData>(_citizenOrderQueue), true);
        Managers.Data.CampSaveDatas.Enqueue(JsonUtility.ToJson(new CampSaveData(data, q), true));
    }

    public override void LoadSaveData()
    {
        var saveData = JsonUtility.FromJson<CampSaveData>(Managers.Data.CampSaveDatas.Dequeue());

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

            citizen.SetReverseMoveType();
            SetCitizenPosition(citizen);
            citizen.SetNextDestination();
        }
    }

    protected override void Init()
    {
        HasUI = false;
    }

    private void CreateBattleUnit(CitizenController citizen)
    {
        Managers.Data.BattleUnitCounts[(int)citizen.Data.JobType - 1]++;
        Debug.Log($"{citizen.Data.JobType.ToString()} : {Managers.Data.BattleUnitCounts[(int)citizen.Data.JobType - 1]}");
    }
}
