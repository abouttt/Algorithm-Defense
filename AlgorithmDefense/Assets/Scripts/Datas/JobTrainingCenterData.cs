using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JobTrainingCenterData
{
    public Define.Job JobType = Define.Job.None;
    public Define.Move MoveType = Define.Move.None;
    public int CurrentCount;
    [HideInInspector]
    public Define.Citizen CitizenType = Define.Citizen.None;

    public void CopyTo(JobTrainingCenterData other)
    {
        other.JobType = JobType;
        other.MoveType = MoveType;
        other.CurrentCount = CurrentCount;
        other.CitizenType = CitizenType;
    }
}
