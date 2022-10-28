using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGate : BaseBuilding
{
    public override void EnterTheBuilding(CitizenController citizen)
    {
        
    }

    protected override void Init()
    {
        HasUI = false;
    }
}
