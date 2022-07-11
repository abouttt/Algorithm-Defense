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
        while (true)
        {
            if (_citizenOrderQueue.Count == 0)
            {
                _isReleasing = false;
                yield break;
            }

            yield return new WaitForSeconds(_stayTime);

            var citizen = DequeueCitizen();
            citizen.TurnAround();
            citizen.SetDest();
            SetPosition(citizen);
        }
    }

    protected override void Init()
    {
        CanSelect = false;
    }
}