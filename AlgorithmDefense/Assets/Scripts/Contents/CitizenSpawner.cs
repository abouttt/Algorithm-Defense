using System;
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

    private int _spawnIndex = 1;

    [SerializeField]
    private bool _startSpawn = false;
    private bool _isSpawning = false;

    private void Start()
    {
        Init();

        LoadingControl.GetInstance.LoadingCompleteAction += StartSpawn;
    }

    private void Update()
    {
        if (!_startSpawn)
        {
            return;
        }

        if (!_isSpawning)
        {
            StartCoroutine(SpawnCitizen());
            _isSpawning = true;
        }
    }

    public void StartSpawn()
    {
        _startSpawn = true;
    }

    public void Setup(Vector3Int spawnPos, float spawnTime)
    {
        _spawnCellPos = spawnPos;
        _spawnTime = spawnTime;

        TileManager.GetInstance.SetTile(Define.Tilemap.Road, spawnPos, Define.Road.BU);
        TileManager.GetInstance.SetTile(Define.Tilemap.Road, spawnPos + Vector3Int.up, Define.Road.BD);

        Util.GetRoad(Define.Tilemap.Road, spawnPos + Vector3Int.up).IsStartRoad = true;
    }

    public IEnumerator SpawnCitizen()
    {
        while (_startSpawn)
        {
            var pos = TileManager.GetInstance.GetCellCenterToWorld(Define.Tilemap.Ground, _spawnCellPos);

            var go = Managers.Resource.Instantiate($"{Define.CITIZEN_PREFAB_PATH}{Enum.GetName(typeof(Define.Citizen), _spawnIndex)}Citizen", pos);

            var unit = go.GetComponent<UnitController>();
            unit.Data.CitizenType = (Define.Citizen)_spawnIndex;
            unit.Data.MoveType = Define.Move.Up;
            unit.SetNextDestination(unit.transform.position);
            unit.Move();

            yield return new WaitForSeconds(_spawnTime);

            _spawnIndex = (_spawnIndex + 1) < 4 ? _spawnIndex + 1 : 1;
        }

        _isSpawning = false;
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