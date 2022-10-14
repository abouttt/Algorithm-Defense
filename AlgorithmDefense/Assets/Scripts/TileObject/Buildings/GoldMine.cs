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
                Managers.Game.Gold += GoldIncrease;
                Debug.Log(Managers.Game.Gold);

                if (HasRoadNextPosition(MoveType))
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
