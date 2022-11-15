using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEvent_0 : TutorialBaseEvent
{
    [SerializeField]
    private Vector3Int _warriorCenterPos;

    bool[,] _discovered;
    Queue<Vector3Int> _reservePos = new();

    private bool _isChecked = false;
    private bool _isCanConnectRoad = false;

    private void Awake()
    {
        RoadBuilder.GetInstance.ConnectedRoadDoneAction += CheckConnectRoad;
        _discovered = new bool[Managers.Game.Setting.RampartHeight, Managers.Game.Setting.RampartWidth];
    }

    public override void InitEvent()
    {
        TileManager.GetInstance.SetTile(Define.Tilemap.Building, _warriorCenterPos, Define.Building.WarriorCenter);
    }

    public override void StartEvent()
    {

    }

    public override void CheckEvent()
    {
        if (_isChecked)
        {
            if (_isCanConnectRoad)
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

    private void CheckConnectRoad()
    {
        _isCanConnectRoad = IsCanConnectToCastle() ? true : false;
        _isChecked = true;
    }

    private bool IsCanConnectToCastle()
    {
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

                if (TileManager.GetInstance.GetTile(Define.Tilemap.Road, nextPos))
                {
                    continue;
                }

                if (Util.GetBuilding<CastleGate>(nextPos))
                {
                    return true;
                }

                _discovered[ny, nx] = true;
                _reservePos.Enqueue(nextPos);
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
