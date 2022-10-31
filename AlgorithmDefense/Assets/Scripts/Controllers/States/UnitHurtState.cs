using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHurtState : MonoBehaviour, IUnitState
{
    private UnitController _unit;

    public void Handle(UnitController controller)
    {
        if (!_unit)
        {
            _unit = controller;
        }

        _unit.CurrentSpeed = 0f;
    }
}
