using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseBuildingData
{
    public float ReleaseTime;
    public Queue<CitizenController> CitizenOrderQueue = new Queue<CitizenController>();
    [HideInInspector]
    public bool HasUI;
    [HideInInspector]
    public bool IsReleasing;

    public virtual void CopyTo(BaseBuildingData other)
    {
        other.HasUI = HasUI;
        other.ReleaseTime = ReleaseTime;
        other.CitizenOrderQueue = CitizenOrderQueue;
        other.IsReleasing = IsReleasing;
    }
}
