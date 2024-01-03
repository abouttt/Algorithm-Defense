using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CitizenUnitData
{
    public Define.Citizen CitizenType;
    public Define.Job JobType;
    public Define.Move MoveType;
    public float MoveSpeed;
    [HideInInspector]
    public Vector3 Destination;
}
