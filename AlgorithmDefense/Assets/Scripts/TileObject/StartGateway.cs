using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGateway : BaseBuilding
{
    public override void EnterTheBuilding(CitizenController citizen)
    {
        _citizenOrderQueue.Enqueue(citizen);
        citizen.gameObject.SetActive(false);
        StartCoroutine(LeaveTheBuilding());
    }

    protected override void Init()
    {
        CanSelect = false;
        _isDirectionOpposite = true;
    }
}