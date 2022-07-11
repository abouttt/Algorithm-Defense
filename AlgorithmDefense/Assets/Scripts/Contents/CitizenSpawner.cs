using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CitizenSpawner : MonoBehaviour
{
    private static CitizenSpawner s_instance;
    public static CitizenSpawner GetInstance { get { Init(); return s_instance; } }

    public static readonly string CITIZEN_PATH = "Prefabs/WorldObject/Citizen/";

    public int OnCount;
    public (Define.Citizen, bool)[] CitizenSpawnList { get; private set; }
    public bool IsSpawning { get; private set; } = false;

    [SerializeField]
    private Vector3Int _spawnCellPos;
    [SerializeField]
    private Vector3Int _endCellPos;
    [SerializeField]
    private float _spawnTime = 5.0f;

    private Define.Citizen _spawnTarget;
    private int _spawnIndex = 0;

    private void Start()
    {
        Init();
        Setup();

        CitizenSpawnList = new (Define.Citizen, bool)[4]
        {
            (Define.Citizen.Red, false),
            (Define.Citizen.Blue, false),
            (Define.Citizen.Green, false),
            (Define.Citizen.Yellow, false),
        };
    }

    public void SetOnOff(Define.Citizen citizenType)
    {
        for (int i = 0; i < CitizenSpawnList.Length; i++)
        {
            if (CitizenSpawnList[i].Item1 == citizenType)
            {
                CitizenSpawnList[i].Item2 = !CitizenSpawnList[i].Item2;
                OnCount = CitizenSpawnList[i].Item2 ? OnCount + 1 : OnCount - 1;
            }
        }

        if (!IsSpawning && OnCount > 0)
        {
            IsSpawning = true;
            StartCoroutine(SpawnCitizen());
        }
    }

    public IEnumerator SpawnCitizen()
    {
        while (true)
        {
            if (OnCount == 0)
            {
                _spawnIndex = 0;
                IsSpawning = false;
                yield break;
            }

            while (true)
            {
                if (CitizenSpawnList[_spawnIndex].Item2)
                {
                    _spawnTarget = CitizenSpawnList[_spawnIndex].Item1;
                    break;
                }
                else
                {
                    _spawnIndex = ++_spawnIndex < CitizenSpawnList.Length ? _spawnIndex : 0;
                }
            }

            var pos = Managers.Tile.GetCellCenterToWorld(Define.Tilemap.Ground, _spawnCellPos) + new Vector3(0.5f, 0, 0);
            var go = Managers.Resource.Instantiate($"{CITIZEN_PATH}{_spawnTarget.ToString()}Citizen", pos);
            var citizen = go.GetComponent<CitizenController>();
            citizen.PrevPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Ground, pos);
            citizen.IsExit = false;
            citizen.MoveType = Define.Move.Right;
            citizen.SetDest();

            yield return new WaitForSeconds(_spawnTime);

            _spawnIndex = ++_spawnIndex < CitizenSpawnList.Length ? _spawnIndex : 0;
        }
    }

    private void Setup()
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

    private static void Init()
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
