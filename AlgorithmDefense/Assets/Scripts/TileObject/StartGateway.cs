using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGateway : BaseBuilding
{
    public override void EnterTheBuilding(CitizenController citizen)
    {
        EnqueueCitizen(citizen);
    }

    protected override void Init()
    {
        CanSelect = false;
        _isDirectionOpposite = true;
    }
}