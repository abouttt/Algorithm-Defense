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

        citizen.Clear();
        Managers.Resource.Destroy(citizen.gameObject);
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
