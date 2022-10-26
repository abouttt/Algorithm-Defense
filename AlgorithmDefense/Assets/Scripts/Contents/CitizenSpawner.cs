using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        LoadingControl.GetInstance.LoadingCompleteAction += StartSpawn;

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

    public void StartSpawn()
    {
        _spawn = true;
    }

    public void Setup(Vector3Int spawnPos, float spawnTime)
    {
        _spawnCellPos = spawnPos;
        _spawnTime = spawnTime;

        TileManager.GetInstance.SetTile(Define.Tilemap.Road, spawnPos, Define.Road.BU);
        TileManager.GetInstance.SetTile(Define.Tilemap.Road, spawnPos + Vector3Int.up, Define.Road.BD);

        var go = TileManager.GetInstance.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(spawnPos + Vector3Int.up);
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

            var pos = TileManager.GetInstance.GetCellCenterToWorld(Define.Tilemap.Ground, _spawnCellPos);
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