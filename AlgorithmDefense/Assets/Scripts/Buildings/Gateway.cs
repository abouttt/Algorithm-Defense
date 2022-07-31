using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gateway : BaseBuilding
{
    public Dictionary<Define.Citizen, Define.Move> DirectionCondition;

    // 테스트 변수.
    public Define.Move Red = Define.Move.None;
    public Define.Move Green = Define.Move.None;
    public Define.Move Blue = Define.Move.None;
    public Define.Move Yellow = Define.Move.None;
    public bool IsChanged = false;

    // 테스트 업데이트.
    private void Update()
    {
        if (IsChanged)
        {
            DirectionCondition[Define.Citizen.Red] = Red;
            DirectionCondition[Define.Citizen.Green] = Green;
            DirectionCondition[Define.Citizen.Blue] = Blue;
            DirectionCondition[Define.Citizen.Yellow] = Yellow;
            IsChanged = false;

            Debug.Log($"{DirectionCondition[Define.Citizen.Red]}");
            Debug.Log($"{DirectionCondition[Define.Citizen.Green]}");
            Debug.Log($"{DirectionCondition[Define.Citizen.Blue]}");
            Debug.Log($"{DirectionCondition[Define.Citizen.Yellow]}");
        }
    }

    public override void EnterTheBuilding(CitizenController citizen)
    {
        EnqueueCitizen(citizen);
    }

    public override string GetSaveData()
    {
        string data = JsonUtility.ToJson(this);
        string q = JsonUtility.ToJson(new SerializationQueue<OrderQueueData>(_citizenOrderQueue));
        string dic = JsonUtility.ToJson(new SerializationDictionary<Define.Citizen, Define.Move>(DirectionCondition));
        return JsonUtility.ToJson(new GatewaySaveData(data, q, dic));
    }

    public override void LoadSaveData(string saveData)
    {
        var data = JsonUtility.FromJson<GatewaySaveData>(saveData);

        JsonUtility.FromJsonOverwrite(data.Data, this);
        _citizenOrderQueue =
            JsonUtility.FromJson<SerializationQueue<OrderQueueData>>(data.OrderQueue).ToQueue();
        DirectionCondition =
            JsonUtility.FromJson<SerializationDictionary<Define.Citizen, Define.Move>>(data.DirectionCondition).ToDictionary();

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
            SetNextDestination(citizen);
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
                { Define.Citizen.Yellow, Define.Move.None },
            };
        }

        HasUI = true;
    }
}
