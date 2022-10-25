using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CitizenSpawner : MonoBehaviour
{
    private static CitizenSpawner s_instance;
    public static CitizenSpawner GetInstance { get { Init(); return s_instance; } }

    public Define.Citizen[] CitizenSpawnList { get; private set; }

    [SerializeField]
    private float _spawnTime;
    private Vector3Int _spawnCellPos;

    private Define.Citizen _spawnTarget;
    private int _spawnIndex = 0;

    // 테스트 전용 변수.
    [SerializeField]
    private bool _spawn = false;
    private bool _isSpawning = false;

    private void Start()
    {
        Init();

        CitizenSpawnList = new Define.Citizen[3]
        {
            Define.Citizen.Red,
            Define.Citizen.Green,
            Define.Citizen.Blue,
        };

        StartCoroutine(SpawnCitizen());
    }

    private void Update()
    {
        if (_spawn && !_isSpawning)
        {
            StartCoroutine(SpawnCitizen());
            _isSpawning = true;
        }
    }

    public void Setup(Vector3Int spawnPos, float spawnTime)
    {
        _spawnCellPos = spawnPos;
        _spawnTime = spawnTime;

        var bu = Managers.Resource.Load<TileBase>($"{Define.ROAD_TILE_PATH}Road_BU");
        var db = Managers.Resource.Load<TileBase>($"{Define.ROAD_TILE_PATH}Road_BD");
        Managers.Tile.SetTile(Define.Tilemap.Road, spawnPos, bu);
        Managers.Tile.SetTile(Define.Tilemap.Road, spawnPos + Vector3Int.up, db);

        var go = Managers.Tile.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(spawnPos + Vector3Int.up);
        go.GetComponent<Road>().IsStartRoad = true;
    }

    public IEnumerator SpawnCitizen()
    {
        while (true)
        {
            if (!_spawn)
            {
                _isSpawning = false;
                yield break;
            }

            _spawnTarget = CitizenSpawnList[_spawnIndex];
            _spawnIndex = ++_spawnIndex < CitizenSpawnList.Length ? _spawnIndex : 0;

            var pos = Managers.Tile.GetCellCenterToWorld(Define.Tilemap.Ground, _spawnCellPos);
            var go = Managers.Resource.Instantiate($"{Define.CITIZEN_PATH}{_spawnTarget}Citizen", pos);
            var citizen = go.GetOrAddComponent<CitizenController>();
            citizen.Data.CitizenType = _spawnTarget;
            citizen.Data.MoveType = Define.Move.Up;
            citizen.SetNextDestination(citizen.transform.position);

            yield return new WaitForSeconds(_spawnTime);

            _spawnIndex = ++_spawnIndex < CitizenSpawnList.Length ? _spawnIndex : 0;
        }
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            var go = GameObject.Find("@CitizenSpawner");
            if (go == null)
            {
                go = Util.CreateGameObject<CitizenSpawner>("@CitizenSpawner");
            }

            s_instance = go.GetComponent<CitizenSpawner>();
        }
    }
}
