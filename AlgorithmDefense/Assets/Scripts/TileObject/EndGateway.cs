using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGateway : BaseBuilding
{
    public override void EnterTheBuilding(CitizenController citizen)
    {
        if (!citizen)
        {
            return;
        }

        CitizenSpawner.GetInstance.Despawn(citizen);
    }

    protected override void Init()
    {
        CanSelect = false;
    }

    protected override IEnumerator LeaveTheBuilding()
    {
        yield break;
    }
}
