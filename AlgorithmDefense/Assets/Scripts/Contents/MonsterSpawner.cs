using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private float _spawnTime;

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

        _stageNumber = PlayerPrefs.GetInt("StageNum") - 1;

        LoadingControl.GetInstance.LoadingCompleteAction += StartSpawn;
    }

    private void StartSpawn()
    {

    }

    private IEnumerator SpawnMonster()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnTime);

            int randNum = Random.Range(0, 3);

        }
    }

    //private void SpawnMonster(MonsterSpawnData data, Vector3 spawnPos)
    //{
    //    var go = Managers.Resource.Instantiate($"{Define.MONSTER_UNIT_PREFAB_PATH}Goblin_{data.job}");
    //    go.transform.position = spawnPos + new Vector3(0.5f, 0f, 0f);
    //    var battleUnit = go.GetComponent<BattleUnitController>();
    //    battleUnit.Data.MoveType = Define.Move.Down;
    //    battleUnit.Data.CurrentHp = battleUnit.Data.MaxHp;
    //}
}