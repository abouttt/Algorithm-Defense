using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CitizenSpawner : MonoBehaviour
{
    private static CitizenSpawner s_instance;
    public static CitizenSpawner GetInstance { get { init(); return s_instance; } }

    public static readonly string CITIZEN_PATH = "Prefabs/WorldObject/Citizen/";

    public bool IsSpawning { get; private set; } = false;

    private Define.Citizen _spawnTarget;

    [SerializeField]
    private Vector3Int _spawnCellPos;
    [SerializeField]
    private Vector3Int _endCellPos;
    [SerializeField]
    private float _spawnTime = 5.0f;
    private int _spawnIdx = 0;

    private void Start()
    {
        init();
        setup();
    }

    public IEnumerator SpawnCitizen()
    {
        if (IsSpawning)
        {
            yield break;
        }
        else
        {
            IsSpawning = true;
        }

        while (true)
        {
            if (Managers.Game.CitizenSpawnOrderList.Count == 0)
            {
                _spawnIdx = 0;
                IsSpawning = false;
                yield break;
            }

            _spawnTarget = Managers.Game.CitizenSpawnOrderList[_spawnIdx];

            while (_spawnIdx >= 0)
            {
                if (!Managers.Game.CitizenSpawnOrderList.Contains(_spawnTarget))
                {
                    _spawnIdx--;
                    _spawnTarget = Managers.Game.CitizenSpawnOrderList[_spawnIdx];
                }
                else
                {
                    break;
                }
            }

            var name = _spawnTarget.ToString();
            var pos = Managers.Tile.GetCellCenterToWorld(Define.Tilemap.Ground, _spawnCellPos);
            Managers.Game.Spawn(Define.WorldObject.Citizen, $"{CITIZEN_PATH}{name}Citizen", pos);

            yield return new WaitForSeconds(_spawnTime);

            _spawnIdx = ++_spawnIdx < Managers.Game.CitizenSpawnOrderList.Count ? _spawnIdx : 0;
        }
    }

    private void setup()
    {
        var tile = Managers.Resource.Load<Tile>($"{BuildingBuilder.BUILDING_TILE_PATH}StartGateway");
        BuildingBuilder.GetInstance.Build(tile, _spawnCellPos);

        tile = Managers.Resource.Load<Tile>($"{BuildingBuilder.BUILDING_TILE_PATH}EndGateway");
        BuildingBuilder.GetInstance.Build(tile, _endCellPos);

        _spawnCellPos.z = 1;
        _endCellPos.z = 1;

        var roadTile = Managers.Resource.Load<TileBase>($"{RoadBuilder.ROAD_RULETILE_PATH}Road_LR_RuleTile");
        Vector3Int roadCellPos = new Vector3Int(0, 0, 0);
        for (int x = -5; x < 5; x++)
        {
            if (-4 < x && x < 3)
            {
                continue;
            }

            roadCellPos.x = x;
            RoadBuilder.GetInstance.Build(roadTile, roadCellPos);
        }
    }

    private static void init()
    {
        if (s_instance == null)
        {
            var go = GameObject.Find("@CitizenSpawner");
            if (go == null)
            {
                go = new GameObject { name = "@CitizenSpawner" };
                go.AddComponent<CitizenSpawner>();
            }

            s_instance = go.GetComponent<CitizenSpawner>();
        }
    }
}
