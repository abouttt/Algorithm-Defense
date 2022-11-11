using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoadBuilder : MonoBehaviour
{
    private static RoadBuilder s_instance;
    public static RoadBuilder GetInstance { get { Init(); return s_instance; } }

    [HideInInspector]
    public bool IsBuilding;
    public Dictionary<int, List<Vector3Int>> RoadGroupDic = new();
    public int GroupCount { get { return _groupCount; } }
    public Action ConnectedRoadDoneAction;

    private int _groupCount = 1;

    private Vector3Int _prevPos;
    private Vector3Int _firstPos;
    private Vector3Int _lastPos;
    private Vector3Int? _startRoadPos;

    private void Update()
    {
        if (IsBuilding)
        {
            // ��ġ�� ��ġ ����.
            if (Input.GetMouseButton(0))
            {
                BuildWillRoads(MouseController.GetInstance.MouseCellPos);
            }

            // ��ġ.
            if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonUp(0))
            {
                BuildRoads();
                IsBuilding = false;
            }
        }
    }

    public bool IsWillRoadBuilding()
    {
        if (!RoadGroupDic.ContainsKey(_groupCount))
        {
            return true;
        }

        return RoadGroupDic[_groupCount].Count > 1;
    }

    public void RemoveRoads(int groupNumber)
    {
        if (!RoadGroupDic.ContainsKey(groupNumber))
        {
            return;
        }

        // �ù��� �ִ��� �˻�
        foreach (var pos in RoadGroupDic[groupNumber])
        {
            var road = Util.GetRoad(Define.Tilemap.Road, pos);
            if (road && road.IsOnCitizen)
            {
                return;
            }
        }

        // �� ����.
        foreach (var pos in RoadGroupDic[groupNumber])
        {
            TileManager.GetInstance.SetTile(Define.Tilemap.Road, pos, null);

            if (_startRoadPos.HasValue && (_startRoadPos.Value == pos))
            {
                TileManager.GetInstance.SetTile(Define.Tilemap.Road, _startRoadPos.Value, Define.Road.BD);
                Util.GetRoad(Define.Tilemap.Road, _startRoadPos.Value).IsStartRoad = true;
                _startRoadPos = null;
            }
        }

        RoadGroupDic.Remove(groupNumber);
    }

    private void BuildWillRoads(Vector3Int pos)
    {
        // ���� ��ġ��� �������� �ʴ´�.
        if (_prevPos == pos)
        {
            return;
        }

        _prevPos = pos;

        // ���� üũ.
        if (!IsInOfRange(pos))
        {
            return;
        }

        // ������ �ִٸ� �������� �ʴ´�.
        if (TileManager.GetInstance.GetTile(Define.Tilemap.Rampart, pos))
        {
            return;
        }

        // �׷��� ���ٸ� �����.
        if (!RoadGroupDic.ContainsKey(_groupCount))
        {
            RoadGroupDic.Add(_groupCount, new());
        }

        // ������ ������ ��ġ��� ����Ѵ�.
        if (TileManager.GetInstance.GetTile(Define.Tilemap.WillRoad, pos))
        {
            RevertWillRoad(pos);
            return;
        }

        // ������ ������ ��ġ ���� �ƴ� ��� �������� �ʴ´�.
        if (!IsNextToPrevPos(pos))
        {
            return;
        }

        // ���۱��� �ƴϸ� ���� �ִٸ� �������� �ʴ´�.
        if (TileManager.GetInstance.GetTile(Define.Tilemap.Road, pos))
        {
            if (!_startRoadPos.HasValue && IsStartRoad(pos))
            {
                _startRoadPos = pos;
            }
            else
            {
                return;
            }
        }

        // ������ ������ ��ġ�� ���۱��̶�� �������� �ʴ´�.
        if (IsStartRoadPrevPos())
        {
            return;
        }

        // ������ ��ġ�� �ǹ��� �ִٸ� �������� �ʴ´�.
        if ((_firstPos != _lastPos) && Util.GetBuilding<BaseBuilding>(_lastPos))
        {
            return;
        }

        RoadGroupDic[_groupCount].Add(pos);
        TileManager.GetInstance.SetTile(Define.Tilemap.WillRoad, pos, Define.Road.B);

        var willRoad = Util.GetRoad(Define.Tilemap.WillRoad, pos);
        willRoad.GroupNumber = _groupCount;
        willRoad.Index = GetGroupLastIndex();
        if (_startRoadPos.HasValue && (_startRoadPos.Value == pos))
        {
            willRoad.IsStartRoad = true;
        }
        willRoad.Refresh(pos);

        // ������ ��ġ�� ������ �ǹ� �Ǵ� ���۱����� �Ǻ��ϱ� ���� ����.
        if (RoadGroupDic[_groupCount].Count == 1)
        {
            _firstPos = pos;
        }
        else
        {
            _lastPos = GetGroupLastPos();
        }
    }

    public void BuildRoads()
    {
        // �ƹ��͵� ����Ǿ� ���� �ʴٸ� �������� �ʴ´�.
        if (!RoadGroupDic.ContainsKey(_groupCount) || (RoadGroupDic[_groupCount].Count == 0))
        {
            return;
        }

        if (IsConnectedBuilding())
        {
            Road road = null;
            Road willRoad = null;
            Define.Road roadType = Define.Road.B;

            foreach (var pos in RoadGroupDic[_groupCount])
            {
                willRoad = Util.GetRoad(Define.Tilemap.WillRoad, pos);

                roadType = willRoad.RoadType;
                int willRoadGroupNumber = willRoad.GroupNumber;
                int willRoadIndex = willRoad.Index;
                bool isStartRoad = willRoad.IsStartRoad;

                TileManager.GetInstance.SetTile(Define.Tilemap.WillRoad, pos, null);
                TileManager.GetInstance.SetTile(Define.Tilemap.Road, pos, roadType);

                road = Util.GetRoad(Define.Tilemap.Road, pos);
                road.GroupNumber = willRoadGroupNumber;
                road.Index = willRoadIndex;
                road.IsStartRoad = isStartRoad;
            }

            RemoveFirstAndLastRoad();
            _groupCount++;
            ConnectedRoadDoneAction?.Invoke();
        }
        else
        {
            foreach (var pos in RoadGroupDic[_groupCount])
            {
                TileManager.GetInstance.SetTile(Define.Tilemap.WillRoad, pos, null);
                if (_startRoadPos.HasValue && (_startRoadPos == pos))
                {
                    _startRoadPos = null;
                }
            }

            RoadGroupDic[_groupCount].Clear();
        }

        _prevPos = Vector3Int.zero;
        _firstPos = Vector3Int.zero;
        _lastPos = Vector3Int.zero;
    }

    private void RevertWillRoad(Vector3Int pos)
    {
        var nextPos = GetGroupLastPos();
        RoadGroupDic[_groupCount].RemoveAt(GetGroupLastIndex());

        if ((RoadGroupDic[_groupCount].Count >= 1) && (pos == GetGroupLastPos()))
        {
            TileManager.GetInstance.SetTile(Define.Tilemap.WillRoad, nextPos, null);
            Util.GetRoad(Define.Tilemap.WillRoad, pos).Refresh(pos);

            if (_startRoadPos.HasValue && (_startRoadPos.Value == nextPos))
            {
                _startRoadPos = null;
            }

            _lastPos = GetGroupLastPos();
        }
        else
        {
            RoadGroupDic[_groupCount].Add(nextPos);
        }
    }

    private bool IsNextToPrevPos(Vector3Int pos)
    {
        if (RoadGroupDic[_groupCount].Count == 0)
        {
            return true;
        }

        var prevPos = GetGroupLastPos();
        for (int i = 0; i < 4; i++)
        {
            int nx = prevPos.x + Define.DX[i];
            int ny = prevPos.y + Define.DY[i];

            if ((pos.x == nx) && (pos.y == ny))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsStartRoadPrevPos()
    {
        if (RoadGroupDic[_groupCount].Count == 0)
        {
            return false;
        }

        // ù��° ������ġ�� ���۱��̶�� ����.
        if (_startRoadPos.HasValue && (_startRoadPos.Value == _firstPos))
        {
            return false;
        }

        return _startRoadPos.HasValue && (_startRoadPos.Value == GetGroupLastPos());
    }

    private bool IsInOfRange(Vector3Int pos)
    {
        if ((pos.x <= Managers.Game.Setting.StartPosition.x) ||
            (pos.y <= Managers.Game.Setting.StartPosition.y) ||
            (pos.x > Managers.Game.Setting.StartPosition.x + Managers.Game.Setting.RampartWidth - 1) ||
            (pos.y > Managers.Game.Setting.StartPosition.y + Managers.Game.Setting.RampartHeight - 1))
        {
            return false;
        }

        return true;
    }

    private bool IsConnectedBuilding()
    {
        var firstBuilding = Util.GetBuilding<BaseBuilding>(_firstPos);
        var secondBuilding = Util.GetBuilding<BaseBuilding>(_lastPos);

        if (firstBuilding && secondBuilding)
        {
            return true;
        }

        if (firstBuilding && _startRoadPos.HasValue && (_startRoadPos.Value == _lastPos))
        {
            return true;
        }

        if (secondBuilding && _startRoadPos.HasValue && (_startRoadPos.Value == _firstPos))
        {
            return true;
        }

        return false;
    }

    private void RemoveFirstAndLastRoad()
    {
        var firstBuilding = TileManager.GetInstance.GetTile(Define.Tilemap.Building, _firstPos);
        var secondBuilding = TileManager.GetInstance.GetTile(Define.Tilemap.Building, _lastPos);

        if (firstBuilding && _startRoadPos != _firstPos)
        {
            TileManager.GetInstance.SetTile(Define.Tilemap.Road, _firstPos, null);
        }

        if (secondBuilding && _startRoadPos != _lastPos)
        {
            TileManager.GetInstance.SetTile(Define.Tilemap.Road, _lastPos, null);
        }
    }

    private bool IsStartRoad(Vector3Int pos)
    {
        var road = Util.GetRoad(Define.Tilemap.Road, pos);
        if (road)
        {
            return road.IsStartRoad;
        }

        return false;
    }

    private Vector3Int GetGroupLastPos() => RoadGroupDic[_groupCount][RoadGroupDic[_groupCount].Count - 1];

    private int GetGroupLastIndex() => RoadGroupDic[_groupCount].Count - 1;

    private static void Init()
    {
        if (s_instance == null)
        {
            var go = GameObject.Find("@RoadBuilder");
            if (!go)
            {
                go = Util.CreateGameObject<RoadBuilder>("@RoadBuilder");
            }

            s_instance = go.GetComponent<RoadBuilder>();
        }
    }
}