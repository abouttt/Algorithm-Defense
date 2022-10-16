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

            var nextMoveType = DirectionCondition[citizen.Data.CitizenType];
            if (nextMoveType == Define.Move.None)
            {
                citizen.SetReverseMoveType();
            }
            else
            {
                if (HasRoadNextPosition(nextMoveType))
                {
                    citizen.Data.MoveType = nextMoveType;
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
