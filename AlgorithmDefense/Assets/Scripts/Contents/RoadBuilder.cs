using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class RoadBuilder : MonoBehaviour
{
    private static RoadBuilder s_instance;
    public static RoadBuilder GetInstance { get { Init(); return s_instance; } }

    public float WrongTilePosRemoveTime;

    [HideInInspector]
    public bool IsBuilding;

    private Tile _roadTile;
    private Stack<Vector3Int> _willRoadPosStack = new();
    private Stack<Vector3Int> _willRoadPosStackTemp = new();
    private Queue<Vector3Int> _wrongTilePosQueue = new();
    private Vector3Int _prevPos;
    private bool _isWillBuildMode;
    private bool _hasMode;
    private bool _isRemoveWrongTile;

    private void Start()
    {
        _roadTile = Managers.Resource.Load<TileBase>($"{Define.WILLROAD_TILE_PATH}Road_B") as Tile;
    }

    private void Update()
    {
        if (IsBuilding)
        {
            // 취소.
            if (Input.GetMouseButtonDown(1))
            {
                Clear();
                return;
            }

            // 설치할 위치 지정.
            if (Input.GetMouseButton(0))
            {
                if (!_hasMode)
                {
                    _isWillBuildMode = Managers.Tile.GetTile(Define.Tilemap.Road, MouseController.GetInstance.MouseCellPos) ? false : true;
                    _hasMode = true;
                }

                CallMode(MouseController.GetInstance.MouseCellPos);
            }

            // 설치.
            if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonUp(0))
            {
                BuildRoads();
                Clear();
            }
        }
    }

    private void BuildRoads()
    {
        _roadTile.color = Color.white;

        TileBase willRoadTile = null;
        TileBase roadTile = null;
        bool isFirstTileRefresh = false;

        while (_willRoadPosStack.Count > 0)
        {
            var cellPos = _willRoadPosStack.Pop();

            willRoadTile = Managers.Tile.GetTile(Define.Tilemap.WillRoad, cellPos);
            roadTile = Managers.Resource.Load<TileBase>($"{Define.ROAD_TILE_PATH}{willRoadTile.name}");

            Managers.Tile.SetTile(Define.Tilemap.WillRoad, cellPos, null);
            Managers.Tile.SetTile(Define.Tilemap.Road, cellPos, roadTile);

            if (!isFirstTileRefresh || _willRoadPosStack.Count == 0)
            {
                _willRoadPosStackTemp.Push(cellPos);
                isFirstTileRefresh = true;
            }
        }

        while (_willRoadPosStackTemp.Count > 0)
        {
            var cellPos = _willRoadPosStackTemp.Pop();
            var go = Managers.Tile.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(cellPos);
            if (go)
            {
                go.GetComponent<Road>().RefreshTile(cellPos);
            }
        }
    }

    private void CallMode(Vector3Int cellPos)
    {
        if (_prevPos == cellPos)
        {
            return;
        }

        _prevPos = cellPos;

        // 설치 범위 확인.
        if (cellPos.x <= Managers.Game.Setting.StartPosition.x ||
            cellPos.y <= Managers.Game.Setting.StartPosition.y ||
            cellPos.x > Managers.Game.Setting.RampartWidth - 1 ||
            cellPos.y > Managers.Game.Setting.RampartHeight - 1)
        {
            return;
        }

        if (_isWillBuildMode)
        {
            BuildMode(cellPos);
        }
        else
        {
            RemoveMode(cellPos);
        }
    }

    private void BuildMode(Vector3Int cellPos)
    {
        // 위치가 예약 되어 있다면 제거.
        if (Managers.Tile.GetTile(Define.Tilemap.WillRoad, cellPos))
        {
            if (_willRoadPosStack.Count <= 1)
            {
                return;
            }

            var pos = _willRoadPosStack.Pop();
            if (_willRoadPosStack.Peek() == cellPos)
            {
                Managers.Tile.SetTile(Define.Tilemap.WillRoad, pos, null);
                WillRoad.WillRoadList.RemoveAt(WillRoad.WillRoadList.Count - 1);

                var prevPos = WillRoad.WillRoadList[WillRoad.WillRoadList.Count - 1];
                var go = Managers.Tile.GetTilemap(Define.Tilemap.WillRoad).GetInstantiatedObject(prevPos);
                if (go)
                {
                    go.GetComponent<WillRoad>().RefreshTile(prevPos);
                }
            }
            else
            {
                _willRoadPosStack.Push(pos);
            }
        }
        else
        {
            // 설치할려는 위치가 이전에 예약한 위치 옆이 아닐 경우 진행하지 않는다.
            if (!IsNextToPrevPos(cellPos))
            {
                return;
            }

            // 이미 길이 있다면 설치 불가.
            // 또는 주변에 막다른 길이 2개 이상이면 설치 불가.
            if (Managers.Tile.GetTile(Define.Tilemap.Road, cellPos) ||
                GetCountNeighborEndRoad(cellPos) >= 2)
            {
                if (Managers.Tile.GetTile(Define.Tilemap.Temp, cellPos))
                {
                    return;
                }

                _roadTile.color = new Color(1, 0, 0, 0.5f);
                _wrongTilePosQueue.Enqueue(cellPos);
                Managers.Tile.SetTile(Define.Tilemap.Temp, cellPos, _roadTile);
                if (!_isRemoveWrongTile)
                {
                    _isRemoveWrongTile = true;
                    StartCoroutine(RemoveWrongTile());
                }

                return;
            }

            _roadTile.color = new Color(0, 0, 1, 0.5f);
            _willRoadPosStack.Push(cellPos);
            Managers.Tile.SetTile(Define.Tilemap.WillRoad, cellPos, _roadTile);
            var go = Managers.Tile.GetTilemap(Define.Tilemap.WillRoad).GetInstantiatedObject(cellPos);
            if (go)
            {
                WillRoad.WillRoadList.Add(cellPos);
                go.GetComponent<WillRoad>().Index = WillRoad.WillRoadList.Count - 1;
                go.GetComponent<WillRoad>().RefreshTile(cellPos);
            }

            // 클릭한 곳이 건물인지.
            if (Managers.Tile.GetTile(Define.Tilemap.Building, cellPos))
            {
                Managers.Tile.SetTile(Define.Tilemap.WillRoad, cellPos, null);
                _willRoadPosStack.Pop();
            }
        }
    }

    bool IsNextToPrevPos(Vector3Int cellPos)
    {
        if (_willRoadPosStack.Count == 0)
        {
            return true;
        }

        for (int i = 0; i < 4; i++)
        {
            int nx = _willRoadPosStack.Peek().x + Define.DX[i];
            int ny = _willRoadPosStack.Peek().y + Define.DY[i];

            if (cellPos.x == nx && cellPos.y == ny)
            {
                return true;
            }
        }

        return false;
    }

    int GetCountNeighborEndRoad(Vector3Int cellPos)
    {
        int cnt = 0;

        for (int i = 0; i < 4; i++)
        {
            int nx = cellPos.x + Define.DX[i];
            int ny = cellPos.y + Define.DY[i];

            var neighborPos = new Vector3Int(nx, ny, 0);
            var go = Managers.Tile.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(neighborPos);
            if (go)
            {
                var road = go.GetComponent<Road>();
                if (road.RoadType == Define.Road.B  ||
                    road.RoadType == Define.Road.BU ||
                    road.RoadType == Define.Road.BD ||
                    road.RoadType == Define.Road.BL ||
                    road.RoadType == Define.Road.BR)
                {
                    cnt++;
                }
            }
        }

        return cnt;
    }

    private void RemoveMode(Vector3Int cellPos)
    {
        if (Managers.Tile.GetTile(Define.Tilemap.Road, cellPos))
        {
            Managers.Tile.SetTile(Define.Tilemap.Road, cellPos, null);
        }
    }

    private IEnumerator RemoveWrongTile()
    {
        while (true)
        {
            if (_wrongTilePosQueue.Count == 0)
            {
                _isRemoveWrongTile = false;
                yield break;
            }

            yield return new WaitForSeconds(WrongTilePosRemoveTime);

            var pos = _wrongTilePosQueue.Dequeue();
            Managers.Tile.SetTile(Define.Tilemap.Temp, pos, null);
        }
    }

    private void Clear()
    {
        WillRoad.WillRoadList.Clear();
        _roadTile.color = Color.white;
        _willRoadPosStack.Clear();
        _willRoadPosStackTemp.Clear();
        _hasMode = false;
        IsBuilding = false;
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
