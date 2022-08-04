using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFactory : BaseBuilding
{
    [field: SerializeField]
    public int CurrentCount { get; private set; }

    [SerializeField]
    private Define.Magic _magicType = Define.Magic.None;
    private MagicFactoryCommonData _commonData;

    public void SetMagicType(Define.Magic magic)
    {
        _magicType = magic;
    }

    public override void EnterTheBuilding(CitizenController citizen)
    {
        if ((_magicType == Define.Magic.None) ||
            (citizen.Data.JobType != Define.Job.None))
        {
            EnqueueCitizen(citizen);
            return;
        }

        Managers.Resource.Destroy(citizen.gameObject);
        CurrentCount++;

        CreateMagic();
    }

    public override void CreateSaveData()
    {
        string data = JsonUtility.ToJson(this, true);
        string q = JsonUtility.ToJson(new SerializationQueue<CitizenOrderQueueData>(_citizenOrderQueue), true);
        Managers.Data.JobTrainingCenterSaveDatas.Enqueue(JsonUtility.ToJson(new MagicFactorySaveData(data, q), true));
    }

    public override void LoadSaveData()
    {
        var saveData = JsonUtility.FromJson<MagicFactorySaveData>(Managers.Data.JobTrainingCenterSaveDatas.Dequeue());

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
        _commonData = Managers.Resource.Load<MagicFactoryCommonData>("Datas/MagicFactoryCommonData");
        HasUI = true;
        CreateMagic();
    }

    private void CreateMagic()
    {
        if (_magicType == Define.Magic.None)
        {
            return;
        }

        if (CurrentCount >= _commonData.MagicCountData[(int)_magicType - 1])
        {
            CurrentCount -= _commonData.MagicCountData[(int)_magicType - 1];
            Managers.Game.MagicCounts[(int)_magicType - 1]++;
            Debug.Log($"{_magicType.ToString()} : {Managers.Game.MagicCounts[(int)_magicType - 1]}");
        }
    }
}
