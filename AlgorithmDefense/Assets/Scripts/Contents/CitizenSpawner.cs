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
    public int OnNum { get; set; } = 0;

    [SerializeField]
    private Vector3Int _spawnCellPos;
    [SerializeField]
    private Vector3Int _endCellPos;
    [SerializeField]
    private float _spawnTime = 5.0f;

    private Define.Citizen _spawnTarget;
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
            if (OnNum == 0)
            {
                _spawnIdx = 0;
                IsSpawning = false;
                yield break;
            }

            while (true)
            {
                if (Managers.Game.CitizenSpawnList[_spawnIdx].Item2)
                {
                    _spawnTarget = Managers.Game.CitizenSpawnList[_spawnIdx].Item1;
                    break;
                }
                else
                {
                    _spawnIdx = ++_spawnIdx < Managers.Game.CitizenSpawnList.Length ? _spawnIdx : 0;
                }
            }

            var pos = Managers.Tile.GetCellCenterToWorld(Define.Tilemap.Ground, _spawnCellPos) + new Vector3(0.5f, 0, 0);
            Managers.Game.Spawn(Define.WorldObject.Citizen, $"{CITIZEN_PATH}{_spawnTarget.ToString()}Citizen", pos);

            yield return new WaitForSeconds(_spawnTime);

            _spawnIdx = ++_spawnIdx < Managers.Game.CitizenSpawnList.Length ? _spawnIdx : 0;
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

        var roadTile = Managers.Resource.Load<TileBase>($"{RoadBuilder.ROAD_RULETILE_PATH}Road_RuleTile");
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
