using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class RoadBuilder : MonoBehaviour
{
    private static RoadBuilder s_instance;
    public static RoadBuilder GetInstance { get { Init(); return s_instance; } }

    [HideInInspector]
    public bool IsBuilding;
    public Dictionary<int, List<Vector3Int>> RoadGroupDic = new();

    private Tile _roadTile;
    private Vector3Int _prevPos;
    private int _groupCount = 1;

    private Vector3Int _firstPos;
    private Vector3Int _lastPos;
    private Vector3Int? _startRoadPos;

    private void Start()
    {
        _roadTile = Managers.Resource.Load<TileBase>($"{Define.ROAD_TILE_PATH}Road_B") as Tile;
    }

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

    public void RemoveRoads(int groupNumber)
    {
        if (!RoadGroupDic.ContainsKey(groupNumber))
        {
            Debug.Log($"RoadBuilder - RemoveRoads() : No contains group number({groupNumber}).");
            return;
        }

        foreach (var pos in RoadGroupDic[groupNumber])
        {
            Managers.Tile.SetTile(Define.Tilemap.Road, pos, null);

            if (_startRoadPos.HasValue && _startRoadPos.Value == pos)
            {
                Debug.Log(_startRoadPos.Value);
                var tile = Managers.Resource.Load<TileBase>($"{Define.ROAD_TILE_PATH}Road_BD");
                Managers.Tile.SetTile(Define.Tilemap.Road, _startRoadPos.Value, tile);
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
        if (Managers.Tile.GetTile(Define.Tilemap.Rampart, pos))
        {
            return;
        }

        // �׷��� ���ٸ� �����.
        if (!RoadGroupDic.ContainsKey(_groupCount))
        {
            RoadGroupDic.Add(_groupCount, new());
        }

        // ���۱��� �ƴϸ� ���� �ִٸ� �������� �ʴ´�.
        if (Managers.Tile.GetTile(Define.Tilemap.Road, pos))
        {
            if (!_startRoadPos.HasValue && IsStartRoad(pos))
            {
                _startRoadPos = new Vector3Int?(pos);
            }
            else
            {
                return;
            }
        }

        // ������ ������ ��ġ��� ����Ѵ�.
        if (Managers.Tile.GetTile(Define.Tilemap.WillRoad, pos))
        {
            var nextPos = GetLastPos();
            RoadGroupDic[_groupCount].RemoveAt(GetLastIndex());

            if ((RoadGroupDic[_groupCount].Count >= 1) && (pos == GetLastPos()))
            {
                Managers.Tile.SetTile(Define.Tilemap.WillRoad, nextPos, null);
                Util.GetRoad(Define.Tilemap.WillRoad, pos).Refresh(pos);

                if (_startRoadPos.HasValue && (_startRoadPos.Value == nextPos))
                {
                    _startRoadPos = null;
                }
            }
            else
            {
                RoadGroupDic[_groupCount].Add(nextPos);
            }

            return;
        }

        // ������ ������ ��ġ ���� �ƴ� ��� �������� �ʴ´�.
        if (!IsNextToPrevPos(pos))
        {
            return;
        }

        // ������ ������ ��ġ�� ���۱��̶�� �������� �ʴ´�.
        if (IsStartRoadPrevPos())
        {
            return;
        }

        RoadGroupDic[_groupCount].Add(pos);
        Managers.Tile.SetTile(Define.Tilemap.WillRoad, pos, _roadTile);

        var willRoad = Util.GetRoad(Define.Tilemap.WillRoad, pos);
        willRoad.GroupNumber = _groupCount;
        willRoad.Index = GetLastIndex();
        if (_startRoadPos.HasValue && (_startRoadPos.Value == pos))
        {
            willRoad.IsStartRoad = true;
        }
        willRoad.Refresh(pos);

        // ù��°�� ������ ��ġ�� ������ �ǹ� �Ǵ� ���۱����� �Ǻ��ϱ� ���� ����.
        if (RoadGroupDic[_groupCount].Count == 1)
        {
            _firstPos = pos;
        }
    }

    private void BuildRoads()
    {
        // �ƹ��͵� ����Ǿ� ���� �ʴٸ� �������� �ʴ´�.
        if (RoadGroupDic[_groupCount].Count == 0)
        {
            return;
        }

        // ���������� ������ ��ġ�� ������ �ǹ� �Ǵ� ���۱����� �Ǻ��ϱ� ���� ����.
        _lastPos = GetLastPos();

        if (IsConnectedBuilding())
        {
            TileBase roadTile = null;
            Road willRoad = null;
            Road road = null;

            foreach (var pos in RoadGroupDic[_groupCount])
            {
                var name = Managers.Tile.GetTile(Define.Tilemap.WillRoad, pos).name;
                roadTile = Managers.Resource.Load<TileBase>($"{Define.ROAD_TILE_PATH}{name}");

                willRoad = Util.GetRoad(Define.Tilemap.WillRoad, pos);
                int willRoadGroupNumber = willRoad.GroupNumber;
                int willRoadIndex = willRoad.Index;
                bool isStartRoad = willRoad.IsStartRoad;

                Managers.Tile.SetTile(Define.Tilemap.WillRoad, pos, null);
                Managers.Tile.SetTile(Define.Tilemap.Road, pos, roadTile);

                road = Util.GetRoad(Define.Tilemap.Road, pos);
                road.GroupNumber = willRoadGroupNumber;
                road.Index = willRoadIndex;
                road.IsStartRoad = isStartRoad;
            }

            RemoveFirstAndLastRoad();

            _groupCount++;
        }
        else
        {
            foreach (var pos in RoadGroupDic[_groupCount])
            {
                Managers.Tile.SetTile(Define.Tilemap.WillRoad, pos, null);
                if (_startRoadPos.HasValue && (_startRoadPos == pos))
                {
                    _startRoadPos = null;
                }
            }

            RoadGroupDic.Remove(_groupCount);
        }
    }

    private bool IsNextToPrevPos(Vector3Int pos)
    {
        if (RoadGroupDic[_groupCount].Count == 0)
        {
            return true;
        }

        var prevPos = GetLastPos();
        for (int i = 0; i < 4; i++)
        {
            int nx = prevPos.x + Define.DX[i];
            int ny = prevPos.y + Define.DY[i];

            if (pos.x == nx && pos.y == ny)
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

        return _startRoadPos.HasValue && (_startRoadPos.Value == GetLastPos());
    }

    private bool IsInOfRange(Vector3Int pos)
    {
        if (pos.x <= Managers.Game.Setting.StartPosition.x ||
            pos.y <= Managers.Game.Setting.StartPosition.y ||
            pos.x > Managers.Game.Setting.StartPosition.x + Managers.Game.Setting.RampartWidth - 1 ||
            pos.y > Managers.Game.Setting.StartPosition.y + Managers.Game.Setting.RampartHeight - 1)
        {
            return false;
        }

        return true;
    }

    private bool IsConnectedBuilding()
    {
        var firstBuilding = Managers.Tile.GetTilemap(Define.Tilemap.Building).GetInstantiatedObject(_firstPos);
        var secondBuilding = Managers.Tile.GetTilemap(Define.Tilemap.Building).GetInstantiatedObject(_lastPos);

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
        var firstBuilding = Managers.Tile.GetTile(Define.Tilemap.Building, _firstPos);
        var secondBuilding = Managers.Tile.GetTile(Define.Tilemap.Building, _lastPos);

        if (firstBuilding && _startRoadPos != _firstPos)
        {
            Managers.Tile.SetTile(Define.Tilemap.Road, _firstPos, null);
        }

        if (secondBuilding && _startRoadPos != _lastPos)
        {
            Managers.Tile.SetTile(Define.Tilemap.Road, _lastPos, null);
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

    private Vector3Int GetLastPos() => RoadGroupDic[_groupCount][RoadGroupDic[_groupCount].Count - 1];

    private int GetLastIndex() => RoadGroupDic[_groupCount].Count - 1;

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
