using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CitizenMove : SerializableDictionary<Define.Citizen, Define.Move> { }

public class Gateway : BaseBuilding
{
    [SerializeField]
    private CitizenMove _directionCondition;

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

            var citizen = DequeueCitizen();

            var directionConditionMoveType = _directionCondition[citizen.CitizenType];
            if (directionConditionMoveType == Define.Move.None)
            {
                citizen.SetOppositeMoveType();
            }
            else
            {
                if (IsRoadNextPosition(directionConditionMoveType))
                {
                    citizen.MoveType = directionConditionMoveType;
                }
                else
                {
                    citizen.SetOppositeMoveType();
                }
            }

            citizen.SetNextDestination();
            SetCitizenPosition(citizen);
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
        _directionCondition = new CitizenMove();
        _directionCondition.Add(Define.Citizen.Red, Define.Move.None);
        _directionCondition.Add(Define.Citizen.Green, Define.Move.None);
        _directionCondition.Add(Define.Citizen.Blue, Define.Move.None);
        _directionCondition.Add(Define.Citizen.Yellow, Define.Move.None);

        HasUI = true;
    }
}
