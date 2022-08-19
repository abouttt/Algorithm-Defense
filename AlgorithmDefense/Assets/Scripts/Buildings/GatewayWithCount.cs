using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatewayWithCount : BaseBuilding
{
    [System.Serializable]
    public class DirectionConditionData
    {
        public Define.Move MoveType;
        public bool IsOn;
    }

    public int Count;           // È½¼ö
    public List<DirectionConditionData> DirectionCondition { get; private set; }

    [SerializeField]
    private int _currentCount;
    [SerializeField]
    private int _currentIndex;

    public override void EnterTheBuilding(CitizenController citizen)
    {
        EnqueueCitizen(citizen);
    }

    public override void CreateSaveData()
    {
        string data = JsonUtility.ToJson(this, true);
        string q = JsonUtility.ToJson(new SerializationQueue<CitizenOrderQueueData>(_citizenOrderQueue), true);
        string dc = JsonUtility.ToJson(new SerializationList<DirectionConditionData>(DirectionCondition), true);
        Managers.Data.GatewayWithCountSaveDatas.Enqueue(JsonUtility.ToJson(new GatewayWithCountSaveData(data, q, dc), true));
    }

    public override void LoadSaveData()
    {
        var saveData = JsonUtility.FromJson<GatewayWithCountSaveData>(Managers.Data.GatewayWithCountSaveDatas.Dequeue());

        JsonUtility.FromJsonOverwrite(saveData.Data, this);
        _citizenOrderQueue =
            JsonUtility.FromJson<SerializationQueue<CitizenOrderQueueData>>(saveData.OrderQueue).ToQueue();
        DirectionCondition =
            JsonUtility.FromJson<SerializationList<DirectionConditionData>>(saveData.DirectionCondition).ToList();

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

            if (_currentCount <= 0)
            {
                _currentCount = Count;

                if (_currentIndex >= DirectionCondition.Count - 1)
                {
                    _currentIndex = -1;
                }

                bool hasSetting = false;
                for (int i = _currentIndex + 1; i < DirectionCondition.Count; i++)
                {
                    if (DirectionCondition[i].IsOn)
                    {
                        _currentIndex = i;
                        hasSetting = true;
                        break;
                    }
                }

                if (!hasSetting)
                {
                    _currentIndex = -1;
                }
            }

            if (_currentIndex == -1)
            {
                citizen.SetReverseMoveType();
            }
            else
            {
                if (IsRoadNextPosition(DirectionCondition[_currentIndex].MoveType))
                {
                    citizen.Data.MoveType = DirectionCondition[_currentIndex].MoveType;
                }
                else
                {
                    citizen.SetReverseMoveType();
                }

                _currentCount--;
            }

            SetCitizenPosition(citizen);
            SetNextDestination(citizen);
        }
    }

    protected override void Init()
    {
        if (DirectionCondition == null)
        {
            DirectionCondition = new List<DirectionConditionData>();
            DirectionCondition.Add(new DirectionConditionData { MoveType = Define.Move.Left, IsOn = false });
            DirectionCondition.Add(new DirectionConditionData { MoveType = Define.Move.Up, IsOn = false });
            DirectionCondition.Add(new DirectionConditionData { MoveType = Define.Move.Right, IsOn = false });
            DirectionCondition.Add(new DirectionConditionData { MoveType = Define.Move.Down, IsOn = false });
        }

        HasUI = true;
    }
}
