using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGateway : BaseBuilding
{
    public Dictionary<Define.Citizen, Define.MoveType> GatewayPassCondition { get; private set; }

    public override void EnterTheBuilding(CitizenController citizen)
    {
        _citizenOrderQueue.Enqueue(citizen);
        citizen.gameObject.SetActive(false);
        StartCoroutine(ReleaseCitizen());
    }

    public override void ShowUIController()
    {

    }

    protected override void Init()
    {
        GatewayPassCondition = new Dictionary<Define.Citizen, Define.MoveType>()
        {
            { Define.Citizen.Red, Define.MoveType.None },
            { Define.Citizen.Blue, Define.MoveType.None },
            { Define.Citizen.Green, Define.MoveType.None },
            { Define.Citizen.Yellow, Define.MoveType.None },
        };

        CanSelect = false;
        _isDirectionOpposite = true;
    }
}