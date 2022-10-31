using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UnitData
{
    public Define.Citizen CitizenType;
    public Define.Job JobType;
    public Define.Move MoveType;
    public bool IsCitizen;
    public bool IsMonster;
    public float MoveSpeed;
    public int MaxHp;
    public int Damage;
    public float Range;
    [HideInInspector]
    public int CurrentHp;
    [HideInInspector]
    public Vector3 Destination;

    public UnitData(bool isCitizen, bool isMonster, float moveSpeed, int maxHp, int damage, float range)
    {
        CitizenType = Define.Citizen.None;
        JobType = Define.Job.None;
        MoveType = Define.Move.None;
        IsCitizen = isCitizen;
        IsMonster = isMonster;
        MoveSpeed = moveSpeed;
        MaxHp = maxHp;
        CurrentHp = MaxHp;
        Damage = damage;
        Range = range;
        Destination = new Vector3();
    }
}
