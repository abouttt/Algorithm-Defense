using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MonsterSpawnData
{
    public float time;
    public Define.Job job;
}

[System.Serializable]
public struct StageSpawnData
{
    public List<MonsterSpawnData> First;
    public List<MonsterSpawnData> Second;
    public List<MonsterSpawnData> Third;
}

public class MonsterSpawner : MonoBehaviour
{
    public List<StageSpawnData> StageDataList;

    private Vector3[] _spawnPos = new Vector3[3];
    private int _stageNumber;

    private void Start()
    {
        _spawnPos[0] = new Vector3(Managers.Game.Setting.StartPosition.x + 1,
            Managers.Game.Setting.RampartHeight + Managers.Game.Setting.BattleLineLength, 0);
        _spawnPos[1] = new Vector3(Managers.Game.Setting.StartPosition.x + 3,
            Managers.Game.Setting.RampartHeight + Managers.Game.Setting.BattleLineLength, 0);
        _spawnPos[2] = new Vector3(Managers.Game.Setting.StartPosition.x + 5,
            Managers.Game.Setting.RampartHeight + Managers.Game.Setting.BattleLineLength, 0);

        _stageNumber = PlayerPrefs.GetInt("StageNum") - 1;

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
            MonsterSpawnData data = StageDataList[_stageNumber].First[index];

            yield return new WaitForSeconds(data.time);

            var go = Managers.Resource.Instantiate($"{Define.MONSTER_UNIT_PATH}Goblin_{data.job}");
            go.transform.position = _spawnPos[0] + new Vector3(0.5f, 0f, 0f);

            SetHP(go.GetComponent<UnitManager>());

            index++;
            if (StageDataList[_stageNumber].First.Count <= index)
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
            MonsterSpawnData data = StageDataList[_stageNumber].Second[index];

            yield return new WaitForSeconds(data.time);

            var go = Managers.Resource.Instantiate($"{Define.MONSTER_UNIT_PATH}Goblin_{data.job}");
            go.transform.position = _spawnPos[1] + new Vector3(0.5f, 0f, 0f);

            SetHP(go.GetComponent<UnitManager>());

            index++;
            if (StageDataList[_stageNumber].Second.Count <= index)
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
            MonsterSpawnData data = StageDataList[_stageNumber].Third[index];

            yield return new WaitForSeconds(data.time);

            var go = Managers.Resource.Instantiate($"{Define.MONSTER_UNIT_PATH}Goblin_{data.job}");
            go.transform.position = _spawnPos[2] + new Vector3(0.5f, 0f, 0f);

            SetHP(go.GetComponent<UnitManager>());

            index++;
            if (StageDataList[_stageNumber].Third.Count <= index)
            {
                index = 0;
            }
        }
    }

    private void SetHP(UnitManager um)
    {
        um.CurrentHP = um.MaxHP;
    }
}