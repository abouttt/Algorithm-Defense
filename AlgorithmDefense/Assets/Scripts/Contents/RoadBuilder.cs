using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;

public class RoadBuilder : MonoBehaviour
{
    private static RoadBuilder s_instance;
    public static RoadBuilder GetInstance { get { Init(); return s_instance; } }

    public bool IsBuilding = false;

    private RuleTile _roadRuleTile;
    private Tile _roadTile;
    private Vector3Int _prevCellPos;
    private bool _canBuild;

    private void Start()
    {
        _roadRuleTile = Managers.Resource.Load<RuleTile>($"{Define.RULE_TILE_PATH}RoadRuleTile");
        _roadTile = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_B");
    }

    private void Update()
    {
        if (IsBuilding)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Clear();
                return;
            }

            var mouseCellPos = MouseController.GetInstance.MouseCellPos;

            CheckCanBuild(mouseCellPos);

            if (_canBuild && Input.GetMouseButton(0))
            {
                Build(mouseCellPos);
            }
        }
    }

    public void Build(Vector3Int cellPos) => Managers.Tile.SetTile(Define.Tilemap.Road, cellPos, _roadRuleTile);

    public void CheckCanBuild(Vector3Int cellPos)
    {
        if (_prevCellPos == cellPos)
        {
            return;
        }

        Managers.Tile.SetTile(Define.Tilemap.Temp, _prevCellPos, null);

        if (Managers.Tile.GetTile(Define.Tilemap.Building, cellPos))
        {
            SetCanBuildFalse();
        }
        else
        {
            SetCanBuildTrue();
        }

        Managers.Tile.SetTile(Define.Tilemap.Temp, cellPos, _roadTile);

        _prevCellPos = cellPos;
    }

    private void SetCanBuildTrue()
    {
        _roadTile.color = Color.white;
        _canBuild = true;
    }

    private void SetCanBuildFalse()
    {
        _roadTile.color = Color.red;
        _canBuild = false;
    }

    public void Clear()
    {
        IsBuilding = false;
        _roadTile.color = Color.white;
        _canBuild = false;
        Managers.Tile.SetTile(Define.Tilemap.Temp, _prevCellPos, null);
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
