using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEvent_1 : TutorialBaseEvent
{
    [SerializeField]
    private Vector3Int _warriorCenterPos;

    bool[,] _discovered;
    Queue<Vector3Int> _reservePos = new();

    private bool _isChecked = false;
    private bool _isConnectedToCastle = false;

    private void Awake()
    {
        RoadBuilder.GetInstance.ConnectedRoadDoneAction += CheckConnectedToCastle;
        _discovered = new bool[Managers.Game.Setting.RampartHeight, Managers.Game.Setting.RampartWidth];
    }

    public override void InitEvent()
    {
        base.InitEvent();
    }

    public override void StartEvent()
    {

    }

    public override void CheckEvent()
    {
        if (_isChecked)
        {
            if (_isConnectedToCastle)
            {
                IsSuccessEvent = true;
            }
            else
            {
                Clear();
                IsFailureEvent = true;
            }

            _isChecked = false;
        }
    }

    private void CheckConnectedToCastle()
    {
        _isConnectedToCastle = (IsConnectedToCastle() && IsConnectedToStartRoad()) ? true : false;
        _isChecked = true;
    }

    private bool IsConnectedToCastle()
    {
        Clear();

        _discovered[_warriorCenterPos.y, _warriorCenterPos.x] = true;
        _reservePos.Enqueue(_warriorCenterPos);

        while (_reservePos.Count > 0)
        {
            Vector3Int pos = _reservePos.Dequeue();
            for (int i = 0; i < 4; i++)
            {
                int ny = pos.y + Define.DY[i];
                int nx = pos.x + Define.DX[i];

                if (ny < Managers.Game.Setting.StartPosition.y ||
                    nx < Managers.Game.Setting.StartPosition.x ||
                    ny > Managers.Game.Setting.StartPosition.y + Managers.Game.Setting.RampartHeight - 1 ||
                    nx > Managers.Game.Setting.StartPosition.x + Managers.Game.Setting.RampartWidth - 1)
                {
                    continue;
                }

                if (_discovered[ny, nx])
                {
                    continue;
                }

                var nextPos = new Vector3Int(nx, ny, 0);

                if (TileManager.GetInstance.GetTile(Define.Tilemap.Rampart, nextPos))
                {
                    continue;
                }

                if (Util.GetBuilding<CastleGate>(nextPos))
                {
                    return true;
                }

                if (TileManager.GetInstance.GetTile(Define.Tilemap.Road, nextPos))
                {
                    _discovered[ny, nx] = true;
                    _reservePos.Enqueue(nextPos);
                }
            }
        }

        return false;
    }

    private bool IsConnectedToStartRoad()
    {
        foreach (var item in RoadBuilder.GetInstance.RoadGroupDic)
        {
            if ((item.Value[0] == (Managers.Game.Setting.SpawnCellPos + Vector3Int.up)) &&
                (item.Value[item.Value.Count - 1] == _warriorCenterPos))
            {
                return true;
            }
            else if ((item.Value[item.Value.Count - 1] == (Managers.Game.Setting.SpawnCellPos + Vector3Int.up)) &&
                     (item.Value[0] == _warriorCenterPos))
            {
                return true;
            }
        }

        return false;
    }

    private void Clear()
    {
        for (int i = 0; i < _discovered.GetLength(0); i++)
        {
            for (int j = 0; j < _discovered.GetLength(1); j++)
            {
                _discovered[i, j] = false;
            }
        }

        _reservePos.Clear();
    }
}
