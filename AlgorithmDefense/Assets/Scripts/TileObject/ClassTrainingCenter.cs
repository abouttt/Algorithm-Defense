using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassTrainingCenter : DirectionApplicableBuilding
{
    [SerializeField]
    private int _count;

    protected override void AddBuildingFun()
    {
        _buildingFuncAction += CountCitizenClassTemp;
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
