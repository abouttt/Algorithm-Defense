using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MonsterSpawnData
{
    public float time;
    public Define.Job job;
}

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private List<MonsterSpawnData> _first;
    [SerializeField]
    private List<MonsterSpawnData> _second;
    [SerializeField]
    private List<MonsterSpawnData> _third;

    private Vector3[] _spawnPos = new Vector3[3];

    private void Start()
    {
        _spawnPos[0] = new Vector3(Managers.Game.Setting.StartPosition.x + 1, 
            Managers.Game.Setting.RampartHeight + Managers.Game.Setting.BattleLineLength, 0);
        _spawnPos[1] = new Vector3(Managers.Game.Setting.StartPosition.x + 3, 
            Managers.Game.Setting.RampartHeight + Managers.Game.Setting.BattleLineLength, 0);
        _spawnPos[2] = new Vector3(Managers.Game.Setting.StartPosition.x + 5, 
            Managers.Game.Setting.RampartHeight + Managers.Game.Setting.BattleLineLength, 0);

        LoadingControl.GetInstance.LoadingCompleteAction += StartSpawn;
    }

    private void StartSpawn()
    {
        StartCoroutine(FirstSpawn());
        StartCoroutine(SecondSpawn());
        StartCoroutine(ThirdSpawn());
    }

    private IEnumerator FirstSpawn()
    {
        int index = 0;
        while (true)
        {
            MonsterSpawnData data = _first[index];

            yield return new WaitForSeconds(data.time);

            var go = Managers.Resource.Instantiate($"{Define.MONSTER_UNIT_PATH}Goblin_{data.job}");
            go.transform.position = _spawnPos[0] + new Vector3(0.5f, 0f, 0f);

            index++;
            if (_first.Count >= index)
            {
                index = 0;
            }
        }
    }

    private IEnumerator SecondSpawn()
    {
        int index = 0;
        while (true)
        {
            MonsterSpawnData data = _second[index];

            yield return new WaitForSeconds(data.time);

            var go = Managers.Resource.Instantiate($"{Define.MONSTER_UNIT_PATH}Goblin_{data.job}");
            go.transform.position = _spawnPos[1] + new Vector3(0.5f, 0f, 0f);

            index++;
            if (_second.Count >= index)
            {
                index = 0;
            }
        }
    }

    private IEnumerator ThirdSpawn()
    {
        int index = 0;
        while (true)
        {
            MonsterSpawnData data = _third[index];

            yield return new WaitForSeconds(data.time);

            var go = Managers.Resource.Instantiate($"{Define.MONSTER_UNIT_PATH}Goblin_{data.job}");
            go.transform.position = _spawnPos[2] + new Vector3(0.5f, 0f, 0f);

            index++;
            if (_third.Count >= index)
            {
                index = 0;
            }
        }
    }
}