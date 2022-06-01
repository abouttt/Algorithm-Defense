using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx
{
    public List<Define.Citizen> CitizenSpawnOrderList { get; private set; } = null;

    public void Init()
    {
        CitizenSpawnOrderList = new List<Define.Citizen>(UI_CitizenSpawnController.BUTTON_NUM);
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
                citizen.IsExit = false;
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
