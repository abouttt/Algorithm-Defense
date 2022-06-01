using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassGiveCenter : CitizenDirectionCenter
{
    [SerializeField]
    private Define.Class _class = Define.Class.None;

    protected override void Init()
    {
        base.Init();

        _isApply = false;
        CanSelect = false;
    }

    protected override void AddBuildingFun()
    {
        BuildingFuncAction += GiveClassTemp;
    }

    private void GiveClassTemp(CitizenController citizen) => citizen.ClassTemp = _class;
}
