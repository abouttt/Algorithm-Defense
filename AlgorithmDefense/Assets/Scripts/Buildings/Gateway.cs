using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CitizenMove : SerializableDictionary<Define.Citizen, Define.Move> { }

public class Gateway : BaseBuilding
{
    [field: SerializeField]
    public CitizenMove DirectionCondition { get; private set; }

    public override void EnterTheBuilding(CitizenController citizen)
    {
        EnqueueCitizen(citizen);
    }

    protected override IEnumerator ReleaseCitizen()
    {
        while (true)
        {
            if (_baseBuildingData.CitizenOrderQueue.Count == 0)
            {
                _baseBuildingData.IsReleasing = false;
                yield break;
            }

            yield return new WaitForSeconds(_baseBuildingData.ReleaseTime);

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
        //_directionCondition = new Dictionary<Define.Citizen, Define.Move>()
        //{
        //    { Define.Citizen.Red, Define.Move.None },
        //    { Define.Citizen.Green, Define.Move.None },
        //    { Define.Citizen.Blue, Define.Move.None },
        //    { Define.Citizen.Yellow, Define.Move.None },
        //};
        DirectionCondition = new CitizenMove();
        DirectionCondition.Add(Define.Citizen.Red, Define.Move.None);
        DirectionCondition.Add(Define.Citizen.Green, Define.Move.None);
        DirectionCondition.Add(Define.Citizen.Blue, Define.Move.None);
        DirectionCondition.Add(Define.Citizen.Yellow, Define.Move.None);

        _baseBuildingData.HasUI = true;
    }
}
