using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStateContext
{
    public IUnitState CurrentState { get; set; }

    private readonly UnitController _unitController;

    public UnitStateContext(UnitController unitController)
    {
        _unitController = unitController;
    }

    public void Transition()
    {
        CurrentState.Handle(_unitController);
    }

    public void Transition(IUnitState state)
    {
        CurrentState = state;
        CurrentState.Handle(_unitController);
    }
}
