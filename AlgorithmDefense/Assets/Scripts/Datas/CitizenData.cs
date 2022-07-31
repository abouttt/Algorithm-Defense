using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CitizenData
{
    public Define.Citizen CitizenType;
    public Define.Job JobType;
    public Define.Move MoveType;
    public float MoveSpeed;
    [HideInInspector]
    public Vector3 Destination;

    public CitizenData(float moveSpeed)
    {
        CitizenType = Define.Citizen.None;
        JobType = Define.Job.None;
        MoveType = Define.Move.None;
        MoveSpeed = moveSpeed;
        Destination = new Vector3();
    }
}
