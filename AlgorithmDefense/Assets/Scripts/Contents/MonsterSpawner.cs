using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public List<Vector3> _gatePos = new();
    private WaveSystem currentWave;
    private int waveValue;
    private float timeBtwnSpawn;

    public WaveSystem[] waves;
    public Transform[] SpawnPoint;

    private void Awake()
    {
        currentWave = waves[waveValue];
        timeBtwnSpawn = currentWave.TimeBeforeThisWave;

    }

    private void Start()
    {
        LoadingControl.GetInstance.LoadingCompleteAction += StartSpawn;

        for (int x = 1; x <= 5; x += 2)
        {
            _gatePos.Add(TileManager.GetInstance.GetCellToWorld(Define.Tilemap.Building, new Vector3Int(
                    Managers.Game.Setting.StartPosition.x + x,
                    Managers.Game.Setting.RampartHeight + Managers.Game.Setting.BattleLineLength, 0)));

        }

    }

    private void StartSpawn()
    {
        StartCoroutine(Spawn());
        IncWave();
    }

    private void IncWave()
    {
        if (waveValue + 1 < waves.Length)
        {
            waveValue++;
            currentWave = waves[waveValue];
        }
    }

    public IEnumerator Spawn()
    {
        for (int i = 0; i < currentWave.NumberToSpawn; i++)
        {
            int a = Random.Range(0, 3);
            if (a == 1)
            {
                int num = Random.Range(0, currentWave.monsterPrefab.Length);
                int num2 = Random.Range(0, _gatePos.Count);
                var go = Managers.Resource.Instantiate($"Prefabs/Units/MonsterUnits/{currentWave.monsterPrefab[num].name}", _gatePos[num2]);
                go.transform.position = _gatePos[num2] + (Vector3.right * 0.5f);
            }
            else if (a == 2)
            {
                for (int x = 0; x < 2; x++)
                {
                    int num = Random.Range(0, currentWave.monsterPrefab.Length);
                    int num2 = Random.Range(0, _gatePos.Count);
                    var go = Managers.Resource.Instantiate($"Prefabs/Units/MonsterUnits/{currentWave.monsterPrefab[num].name}", _gatePos[num2]);
                    go.transform.position = _gatePos[num2] + (Vector3.right * 0.5f);
                }
            }
            else if (a == 3)
            {
                for (int y = 0; y < 3; y++)
                {
                    int num = Random.Range(0, currentWave.monsterPrefab.Length);
                    var go = Managers.Resource.Instantiate($"Prefabs/Units/MonsterUnits/{currentWave.monsterPrefab[num].name}", _gatePos[y]);
                    go.transform.position = _gatePos[y] + (Vector3.right * 0.5f);
                }
            }
            yield return new WaitForSeconds(4f);
            //Instantiate(currentWave.monsterPrefab[num], SpawnPoint[num2].position, SpawnPoint[num2].rotation);
        }
    }
}