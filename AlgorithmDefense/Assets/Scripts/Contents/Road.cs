using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [field: SerializeField]
    public Define.RoadType RoadType { get; private set; }
}
