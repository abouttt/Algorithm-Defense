using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CitizenData
{
    [field: SerializeField]
    public Define.Citizen CitizenType { get; private set; }
    public Define.Move MoveType = Define.Move.None;
    public float MoveSpeed;
    public Vector3 Destination;
}
