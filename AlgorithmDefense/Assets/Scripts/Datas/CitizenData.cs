using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CitizenData
{
    public Define.Citizen CitizenType = Define.Citizen.None;
    public Define.Job JobType = Define.Job.None;
    public Define.Move MoveType = Define.Move.None;
    public float MoveSpeed;
    [HideInInspector]
    public Vector3 Destination;

    public void CopyTo(CitizenData other)
    {
        other.CitizenType = CitizenType;
        other.JobType = JobType;
        other.MoveType = MoveType;
        other.MoveSpeed = MoveSpeed;
        other.Destination = Destination;
    }
}
