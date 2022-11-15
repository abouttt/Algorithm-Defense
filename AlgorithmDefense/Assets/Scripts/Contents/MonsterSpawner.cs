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

    private Vector3[] _spawnPosArr = new Vector3[3];
    private int _stageNumber;

    private void Start()
    {
        _spawnPosArr[0] = new Vector3(Managers.Game.Setting.StartPosition.x + 1,
            Managers.Game.Setting.RampartHeight + Managers.Game.Setting.BattleLineLength, 0);
        _spawnPosArr[1] = new Vector3(Managers.Game.Setting.StartPosition.x + 3,
            Managers.Game.Setting.RampartHeight + Managers.Game.Setting.BattleLineLength, 0);
        _spawnPosArr[2] = new Vector3(Managers.Game.Setting.StartPosition.x + 5,
            Managers.Game.Setting.RampartHeight + Managers.Game.Setting.BattleLineLength, 0);

        _stageNumber = PlayerPrefs.GetInt("Num") - 1;

        LoadingControl.GetInstance.LoadingCompleteAction += StartSpawn;
    }

    private void StartSpawn()
    {
        StartCoroutine(Spawn(StageDataList[_stageNumber].First, _spawnPosArr[0] + new Vector3(0.51f, 0f, 0f)));
        StartCoroutine(Spawn(StageDataList[_stageNumber].Second, _spawnPosArr[1] + new Vector3(0.51f, 0f, 0f)));
        StartCoroutine(Spawn(StageDataList[_stageNumber].Third, _spawnPosArr[2] + new Vector3(0.51f, 0f, 0f)));
    }

    private IEnumerator Spawn(List<MonsterSpawnData> monsterSpawnData, Vector3 spawnPos)
    {
        if (monsterSpawnData.Count == 0)
        {
            yield break;
        }

        int index = 0;

        while (true)
        {
            MonsterSpawnData data = monsterSpawnData[index];

            yield return new WaitForSeconds(data.time);

            Util.CreateMonster(data.job, spawnPos);

            index = (index + 1) >= monsterSpawnData.Count ? 0 : index + 1;
        }
    }
}