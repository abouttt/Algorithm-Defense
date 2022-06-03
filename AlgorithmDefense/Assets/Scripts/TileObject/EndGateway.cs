using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGateway : BaseBuilding
{
    public override void EnterTheBuilding(CitizenController citizen)
    {
        if (citizen != null)
        {
            Managers.Game.Despawn(citizen.gameObject);
        }
    }

    public override void ShowUIController()
    {
        
    }

    protected override void Init()
    {
        CanSelect = false;
    }
}