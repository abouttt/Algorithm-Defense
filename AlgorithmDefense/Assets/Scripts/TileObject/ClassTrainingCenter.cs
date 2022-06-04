using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassTrainingCenter : DirectionApplicableBuilding
{
    [SerializeField]
    private int _count;

    protected override void AddBuildingFun()
    {
        _buildingFuncAction += countCitizenClassTemp;
    }

    private void countCitizenClassTemp(CitizenController citizen)
    {
        citizen.ClassTrainingCount++;
        if (citizen.ClassTrainingCount >= _count)
        {
            citizen.ClassTrainingCount = 0;
            citizen.Class = citizen.ClassTemp;
        }
    }
}
