using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleGate : BaseBuilding
{
    public int HP;

    public override void EnterTheBuilding(CitizenController citizen)
    {
        EnqueueCitizen(citizen);
    }

    public override void CreateSaveData()
    {
        
    }

    public override void LoadSaveData()
    {
        
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

            // �̰����� �������� ���� ������Ʈ�� �����Ѵ�.
            if (citizen.Data.JobType != Define.Job.None)
            {
                citizen.Data.MoveType = Define.Move.Up;
            }
            else
            {
                citizen.SetReverseMoveType();
            }

            SetCitizenPosition(citizen);
            citizen.SetNextDestination(transform.position);
        }
    }

    protected override void Init()
    {
        HasUI = false;
    }
}
