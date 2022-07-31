using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CitizenSaveData
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
    public CitizenData Data;
}
