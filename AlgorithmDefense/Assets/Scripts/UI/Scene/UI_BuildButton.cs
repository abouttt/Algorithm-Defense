using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UI_BuildButton : MonoBehaviour
{
    [field: SerializeField]
    public Define.BuildType BuildType { get; set; }

    [field: SerializeField]
    public Define.TileObject TileName { get; set; }
}
