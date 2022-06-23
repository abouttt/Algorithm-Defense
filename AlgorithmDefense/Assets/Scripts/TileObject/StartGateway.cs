using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGateway : BaseBuilding
{
    public override void EnterTheBuilding(CitizenController citizen)
    {
        EnqueueCitizen(citizen);
    }

    protected override IEnumerator LeaveTheBuilding()
    {
        yield return new WaitForSeconds(_stayTime);

        if (_citizenOrderQueue.Count == 0)
        {
            yield break;
        }

        var citizen = DequeueCitizen();

        SetOpposite(citizen);

        if (!HasRoadNextPosition(citizen.MoveType))
        {
            SetOpposite(citizen);
        }

        citizen.SetDest();
        SetPosition(citizen);
    }

    protected override void Init()
    {
        CanSelect = false;
    }
}