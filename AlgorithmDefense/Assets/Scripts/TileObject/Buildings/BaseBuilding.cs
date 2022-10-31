using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuilding : MonoBehaviour
{
    public bool HasUI { get; protected set; }

    [SerializeField]
    protected float _releaseTime;

    private void Start()
    {
        Init();
    }

    public abstract void EnterTheBuilding(UnitController citizen);

    protected abstract void Init();

    public void SetUnitPosition(UnitController unit, Define.Move moveType)
    {
        var pos = TileManager.GetInstance.GetWorldToCellCenterToWorld(Define.Tilemap.Ground, transform.position);

        switch (moveType)
        {
            case Define.Move.Right:
                pos += new Vector3(0.51f, 0, 0);
                break;
            case Define.Move.Left:
                pos += new Vector3(-0.51f, 0, 0);
                break;
            case Define.Move.Up:
                pos += new Vector3(0, 0.51f, 0);
                break;
            case Define.Move.Down:
                pos += new Vector3(0, -0.51f, 0);
                break;
        }

        unit.transform.position = pos;
    }

    protected bool HasRoadNextPosition(Define.Move moveType)
    {
        var nextPos = TileManager.GetInstance.GetWorldToCell(Define.Tilemap.Ground, transform.position);

        switch (moveType)
        {
            case Define.Move.None:
                return false;
            case Define.Move.Up:
                nextPos += Vector3Int.up;
                break;
            case Define.Move.Right:
                nextPos += Vector3Int.right;
                break;
            case Define.Move.Down:
                nextPos += Vector3Int.down;
                break;
            case Define.Move.Left:
                nextPos += Vector3Int.left;
                break;
        }

        var road = Util.GetRoad(Define.Tilemap.Road, nextPos);
        if (road)
        {
            if (moveType == Define.Move.Up)
            {
                if (road.RoadType == Define.Road.BD ||
                   road.RoadType == Define.Road.CUL ||
                   road.RoadType == Define.Road.CUR ||
                   road.RoadType == Define.Road.UD)
                {
                    return true;
                }
            }
            else if (moveType == Define.Move.Right)
            {
                if (road.RoadType == Define.Road.BL ||
                   road.RoadType == Define.Road.CDL ||
                   road.RoadType == Define.Road.CUL ||
                   road.RoadType == Define.Road.LR)
                {
                    return true;
                }
            }
            else if (moveType == Define.Move.Down)
            {
                if (road.RoadType == Define.Road.BU ||
                   road.RoadType == Define.Road.CDL ||
                   road.RoadType == Define.Road.CDR ||
                   road.RoadType == Define.Road.UD)
                {
                    return true;
                }
            }
            else if (moveType == Define.Move.Left)
            {
                if (road.RoadType == Define.Road.BR ||
                   road.RoadType == Define.Road.CDR ||
                   road.RoadType == Define.Road.CUR ||
                   road.RoadType == Define.Road.LR)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
