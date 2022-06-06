using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx
{
    public (Define.Citizen, bool)[] CitizenSpawnList { get; private set; }

    public void Init()
    {
        CitizenSpawnList = new (Define.Citizen, bool)[UI_CitizenSpawnController.BUTTON_NUM]
        {
            (Define.Citizen.Red, false),
            (Define.Citizen.Blue, false),
            (Define.Citizen.Green, false),
            (Define.Citizen.Yellow, false),
        };
    }

    public GameObject Spawn(Define.WorldObject type, string path, Vector3? position, Transform parent = null)
    {
        if (!position.HasValue)
        {
            position = Vector3.zero;
        }

        var go = Managers.Resource.Instantiate(path, position.Value, parent);

        switch (type)
        {
            case Define.WorldObject.Citizen:
                var citizen = go.GetComponent<CitizenController>();
                citizen.PrevPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Ground, position.Value);
                citizen.MoveType = Define.MoveType.Right;
                citizen.SetDest();
                break;
        }

        return go;
    }

    public void Despawn(GameObject go)
    {
        var type = GetWorldObjectType(go);

        switch (type)
        {
            case Define.WorldObject.Citizen:
                var citizen = go.GetComponent<CitizenController>();
                citizen.MoveType = Define.MoveType.None;
                citizen.Class = Define.Class.None;
                citizen.ClassTemp = Define.Class.None;
                citizen.ClassTrainingCount = 0;
                break;
        }

        Managers.Resource.Destroy(go);
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        var bc = go.GetComponent<BaseController>();
        if (bc == null)
        {
            return Define.WorldObject.Unknown;
        }

        return bc.WorldObjectType;
    }
}
