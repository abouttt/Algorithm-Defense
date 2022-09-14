using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gateway : BaseBuilding
{
    public Dictionary<Define.Citizen, Define.Move> DirectionCondition { get; private set; }

    public override void EnterTheBuilding(CitizenController citizen)
    {
        EnqueueCitizen(citizen);
    }

    public override void CreateSaveData()
    {
        string data = JsonUtility.ToJson(this, true);
        string q = JsonUtility.ToJson(new SerializationQueue<CitizenOrderQueueData>(_citizenOrderQueue), true);
        string dc = JsonUtility.ToJson(new SerializationDictionary<Define.Citizen, Define.Move>(DirectionCondition), true);
        Managers.Data.GatewaySaveDatas.Enqueue(JsonUtility.ToJson(new BuildingSaveData(data, q, dc), true));
    }

    public override void LoadSaveData()
    {
        var saveData = JsonUtility.FromJson<BuildingSaveData>(Managers.Data.GatewaySaveDatas.Dequeue());

        JsonUtility.FromJsonOverwrite(saveData.Data, this);
        _citizenOrderQueue =
            JsonUtility.FromJson<SerializationQueue<CitizenOrderQueueData>>(saveData.OrderQueue).ToQueue();
        DirectionCondition =
            JsonUtility.FromJson<SerializationDictionary<Define.Citizen, Define.Move>>(saveData.DirectionCondition).ToDictionary();

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

            var directionConditionMoveType = DirectionCondition[citizen.Data.CitizenType];
            if (directionConditionMoveType == Define.Move.None)
            {
                citizen.SetReverseMoveType();
            }
            else
            {
                if (IsRoadNextPosition(directionConditionMoveType))
                {
                    citizen.Data.MoveType = directionConditionMoveType;
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
        if (DirectionCondition == null)
        {
            DirectionCondition = new Dictionary<Define.Citizen, Define.Move>()
            {
                { Define.Citizen.Red, Define.Move.None },
                { Define.Citizen.Green, Define.Move.None },
                { Define.Citizen.Blue, Define.Move.None },
            };
        }

        HasUI = true;
    }
}
