using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassGiveCenter : BaseBuilding
{
    [SerializeField]
    private Define.Class _classTemp = Define.Class.None;

    protected override void Init()
    {
        CanSelect = false;
        _isDirectionOpposite = false;
    }

    public override void EnterTheBuilding(CitizenController citizen)
    {
        citizen.ClassTemp = _classTemp;

        _citizenOrderQueue.Enqueue(citizen);
        citizen.gameObject.SetActive(false);
        StartCoroutine(LeaveTheBuilding());
    }

    public override void ShowUIController() 
    { 
        // TODO
    }
}
