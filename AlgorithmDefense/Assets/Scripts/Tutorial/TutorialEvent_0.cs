using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEvent_0 : TutorialBaseEvent
{
    [SerializeField]
    private Vector3Int _warriorCenterPos;

    private bool _isChecked = false;
    private bool _isCanConnectRoad = false;

    private void Awake()
    {
        RoadBuilder.GetInstance.ConnectedRoadDoneAction += CheckConnectRoad;
    }

    public override void InitEvent()
    {
        TileManager.GetInstance.SetTile(Define.Tilemap.Building, _warriorCenterPos, Define.Building.WarriorCenter);
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
                IsFailureEvent = true;
            }

            _isChecked = false;
        }
    }

    private void CheckConnectRoad()
    {
        _isCanConnectRoad = IsCanConnectCastle() ? true : false;
        _isChecked = true;
    }

    private bool IsCanConnectCastle()
    {
        bool[,] visited = new bool[Managers.Game.Setting.RampartHeight, Managers.Game.Setting.RampartWidth];
        Queue<Vector3Int> q = new();
        visited[_warriorCenterPos.y, _warriorCenterPos.x] = true;
        q.Enqueue(_warriorCenterPos);
        while (q.Count > 0)
        {
            Vector3Int pos = q.Dequeue();
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

                if (visited[ny, nx])
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

                visited[ny, nx] = true;
                q.Enqueue(nextPos);
            }
        }

        return false;
    }
}
