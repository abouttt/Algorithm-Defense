using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UI_BuildButton : MonoBehaviour
{
    [field: SerializeField]
    public Define.BuildType BuildType { get; private set; }

    [field: SerializeField]
    public Define.TileObject TileObject { get; private set; }
}
