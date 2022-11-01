using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BattleUnitData
{
    public Define.Move MoveType;
    public int MaxHp;
    public int CurrentHp;
    public int Damage;
    public float Range;
    public float MoveSpeed;
    public LayerMask TargetLayer;
}
