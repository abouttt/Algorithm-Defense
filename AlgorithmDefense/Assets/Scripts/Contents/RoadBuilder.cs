using System.Collections;
using System.Collections.Generic;
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
    private Stack<Vector3Int> _willRoadPosStack = new();
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
            // 설치 취소.
            if (Input.GetMouseButtonDown(1))
            {
                Clear();
                _startRoadPos = null;
                return;
            }

            // 설치할 위치 지정.
            if (Input.GetMouseButton(0))
            {
                BuildWillRoads(MouseController.GetInstance.MouseCellPos);
            }

            // 설치.
            if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonUp(0))
            {
                BuildRoads();
                Clear();
            }
        }
    }

    public void RemoveRoads(int groupNumber)
    {
        if (!RoadGroupDic.ContainsKey(groupNumber))
        {
            return;
        }

        foreach (var pos in RoadGroupDic[groupNumber])
        {
            if (_startRoadPos.HasValue && (_startRoadPos.Value == pos))
            {
                var tile = Managers.Resource.Load<TileBase>($"{Define.ROAD_TILE_PATH}Road_BD");
                Managers.Tile.SetTile(Define.Tilemap.Road, pos, tile);
                var go = Managers.Tile.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(pos);
                go.GetComponent<Road>().IsStartRoad = true;
                _startRoadPos = null;
            }
            else
            {
                Managers.Tile.SetTile(Define.Tilemap.Road, pos, null);
            }
        }

        RoadGroupDic.Remove(groupNumber);
    }

    private void BuildWillRoads(Vector3Int pos)
    {
        if (_prevPos == pos)
        {
            return;
        }

        _prevPos = pos;

        // 범위 체크.
        if (!IsInOfRange(pos))
        {
            return;
        }

        // 이전에 예약한 위치 옆이 아닐 경우 진행하지 않는다.
        if (!IsNextToPrevPos(pos))
        {
            return;
        }

        // 시작길이 아니며 길이 있다면 진행하지 않는다.
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

        // 성벽이 있다면 진행하지 않는다.
        if (Managers.Tile.GetTile(Define.Tilemap.Rampart, pos))
        {
            return;
        }

        // 예약한 위치라면 취소한다.
        if (Managers.Tile.GetTile(Define.Tilemap.WillRoad, pos))
        {
            var peekPos = _willRoadPosStack.Pop();
            if ((_willRoadPosStack.Count >= 1) && (pos == _willRoadPosStack.Peek()))
            {
                Managers.Tile.SetTile(Define.Tilemap.WillRoad, peekPos, null);
                RoadGroupDic[_groupCount].RemoveAt(RoadGroupDic[_groupCount].Count - 1);

                var prevGo = Managers.Tile.GetTilemap(Define.Tilemap.WillRoad).GetInstantiatedObject(pos);
                if (prevGo)
                {
                    prevGo.GetComponent<Road>().Refresh(pos);
                }
            }
            else
            {
                _willRoadPosStack.Push(peekPos);
            }

            return;
        }

        _willRoadPosStack.Push(pos);
        Managers.Tile.SetTile(Define.Tilemap.WillRoad, pos, _roadTile);
        var go = Managers.Tile.GetTilemap(Define.Tilemap.WillRoad).GetInstantiatedObject(pos);
        if (go)
        {
            if (!RoadGroupDic.ContainsKey(_groupCount))
            {
                RoadGroupDic.Add(_groupCount, new());
            }

            RoadGroupDic[_groupCount].Add(pos);
            var road = go.GetComponent<Road>();
            road.GroupNumber = _groupCount;
            road.Index = RoadGroupDic[_groupCount].Count - 1;
            if (_startRoadPos.HasValue && (_startRoadPos.Value == pos))
            {
                road.IsStartRoad = true;
            }
            road.Refresh(pos);
        }

        if (_willRoadPosStack.Count == 1)
        {
            _firstPos = pos;
        }
    }

    private void BuildRoads()
    {
        if (_willRoadPosStack.Count == 0)
        {
            return;
        }

        _lastPos = _willRoadPosStack.Peek();

        if (IsConnectBuilding())
        {
            TileBase roadTile = null;
            Road willRoad = null;
            Road road = null;

            while (_willRoadPosStack.Count > 0)
            {
                var pos = _willRoadPosStack.Pop();

                var name = Managers.Tile.GetTile(Define.Tilemap.WillRoad, pos).name;
                roadTile = Managers.Resource.Load<TileBase>($"{Define.ROAD_TILE_PATH}{name}");

                willRoad = Managers.Tile.GetTilemap(Define.Tilemap.WillRoad).GetInstantiatedObject(pos).GetComponent<Road>();
                int willRoadGroupNumber = willRoad.GroupNumber;
                int willRoadIndex = willRoad.Index;
                bool isStartRoad = willRoad.IsStartRoad;

                Managers.Tile.SetTile(Define.Tilemap.WillRoad, pos, null);
                Managers.Tile.SetTile(Define.Tilemap.Road, pos, roadTile);
                road = Managers.Tile.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(pos).GetComponent<Road>();
                road.GroupNumber = willRoadGroupNumber;
                road.Index = willRoadIndex;
                road.IsStartRoad = isStartRoad;
            }

            RemoveFirstAndLastRoad();

            _groupCount++;
        }
        else
        {
            while (_willRoadPosStack.Count > 0)
            {
                var pos = _willRoadPosStack.Pop();
                Managers.Tile.SetTile(Define.Tilemap.WillRoad, pos, null);
            }
            RoadGroupDic[_groupCount].Clear();
            _startRoadPos = null;
        }
    }

    private bool IsNextToPrevPos(Vector3Int pos)
    {
        if (_willRoadPosStack.Count == 0)
        {
            return true;
        }

        for (int i = 0; i < 4; i++)
        {
            var prevPos = _willRoadPosStack.Peek();
            int nx = prevPos.x + Define.DX[i];
            int ny = prevPos.y + Define.DY[i];

            if (pos.x == nx && pos.y == ny)
            {
                return true;
            }
        }

        return false;
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

    private bool IsConnectBuilding()
    {
        var a = Managers.Tile.GetTilemap(Define.Tilemap.Building).GetInstantiatedObject(_firstPos);
        var b = Managers.Tile.GetTilemap(Define.Tilemap.Building).GetInstantiatedObject(_lastPos);

        if (a && b)
        {
            return true;
        }

        if (a && _startRoadPos.HasValue && (_startRoadPos.Value == _lastPos))
        {
            return true;
        }

        if (b && _startRoadPos.HasValue && (_startRoadPos.Value == _lastPos))
        {
            return true;
        }

        return false;
    }

    private void RemoveFirstAndLastRoad()
    {
        var a = Managers.Tile.GetTile(Define.Tilemap.Building, _firstPos);
        var b = Managers.Tile.GetTile(Define.Tilemap.Building, _lastPos);

        if (a && a.name.Equals(Define.Building.Gateway.ToString()))
        {
            Managers.Tile.SetTile(Define.Tilemap.Road, _firstPos, null);
        }

        if (b && b.name.Equals(Define.Building.Gateway.ToString()))
        {
            Managers.Tile.SetTile(Define.Tilemap.Road, _firstPos, null);
        }
    }

    private bool IsStartRoad(Vector3Int pos)
    {
        var go = Managers.Tile.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(pos);
        if (go)
        {
            var road = go.GetComponent<Road>();
            return road.IsStartRoad;
        }

        return false;
    }

    private void Clear()
    {
        IsBuilding = false;
        _willRoadPosStack.Clear();
    }

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
