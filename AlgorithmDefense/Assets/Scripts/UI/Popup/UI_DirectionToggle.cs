using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DirectionToggle : MonoBehaviour
{
    [field: SerializeField]
    public Define.Citizen CitizenType { get; private set; }
    [field: SerializeField]
    public Define.Move MoveType { get; private set; }
}
