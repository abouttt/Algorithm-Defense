using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GatewayToggle : MonoBehaviour
{
    [field: SerializeField]
    public Define.Citizen CitizenType { get; private set; }
    [field: SerializeField]
    public Define.MoveType MoveType { get; private set; }
}
