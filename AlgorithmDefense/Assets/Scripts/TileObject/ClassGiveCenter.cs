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

        EnqueueCitizen(citizen);
    }

    public override void ShowUIController() 
    { 
        // TODO
    }
}
