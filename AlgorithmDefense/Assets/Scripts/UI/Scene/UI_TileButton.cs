using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TileButton : MonoBehaviour
{
    [field: SerializeField]
    public Define.BuildType BuildType { get; private set; }

    [field: SerializeField]
    public Define.TileObject TileObject { get; private set; }
}
