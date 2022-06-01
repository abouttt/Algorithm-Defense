using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassTrainingCenter : CitizenDirectionCenter
{
    [SerializeField]
    private int _count;

    protected override void Init()
    {
        base.Init();

        _isApply = true;
        CanSelect = true;
    }

    protected override void AddBuildingFun()
    {
        BuildingFuncAction += CountCitizenClassTemp;
    }

    private void CountCitizenClassTemp(CitizenController citizen)
    {
        citizen.ClassTrainingCount++;
        if (citizen.ClassTrainingCount >= _count)
        {
            citizen.ClassTrainingCount = 0;
            citizen.Class = citizen.ClassTemp;
        }
    }
}
