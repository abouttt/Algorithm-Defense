using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : BaseBuilding
{
    public override void EnterTheBuilding(UnitController citizen)
    {
        
    }

    protected override void Init()
    {
        HasUI = false;
    }
}
