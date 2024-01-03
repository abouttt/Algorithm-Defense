using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenSpawner : MonoBehaviour
{
    private static CitizenSpawner s_instance;
    public static CitizenSpawner GetInstance { get { Init(); return s_instance; } }

    public float SpawnTime;
    public Define.Citizen[] CitizenSpawnList { get; private set; }

    private Vector3Int _spawnCellPos;

    private int _spawnIndex = 1;

    private void Awake()
    {
        LoadingControl.GetInstance.LoadingCompleteAction += StartSpawn;
    }

    public void StartSpawn()
    {
        if (Managers.Game.Setting.IsTutorialScene)
        {
            return;
        }

        StartCoroutine(SpawnCitizen());
    }

    public void Setup(Vector3Int spawnPos, float spawnTime)
    {
        _spawnCellPos = spawnPos;
        SpawnTime = spawnTime;

        TileManager.GetInstance.SetTile(Define.Tilemap.Road, spawnPos, Define.Road.BU);
        TileManager.GetInstance.SetTile(Define.Tilemap.Road, spawnPos + Vector3Int.up, Define.Road.BD);

        Util.GetRoad(Define.Tilemap.Road, spawnPos + Vector3Int.up).IsStartRoad = true;
    }

    public void SetSpawnTime(float spawnTime)
    {
        SpawnTime = spawnTime;
    }

    public IEnumerator SpawnCitizen()
    {
        while (true)
        {
            var pos = TileManager.GetInstance.GetCellCenterToWorld(Define.Tilemap.Ground, _spawnCellPos);

            var go = Managers.Resource.Instantiate($"{Define.CITIZEN_PREFAB_PATH}{Enum.GetName(typeof(Define.Citizen), _spawnIndex)}Citizen", pos);

            var citizen = go.GetComponent<CitizenUnitController>();
            citizen.Data.CitizenType = (Define.Citizen)_spawnIndex;
            citizen.Data.MoveType = Define.Move.Up;
            citizen.SetNextDestination(citizen.transform.position);

            yield return new WaitForSeconds(SpawnTime);

            _spawnIndex = (_spawnIndex + 1) < 4 ? _spawnIndex + 1 : 1;
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