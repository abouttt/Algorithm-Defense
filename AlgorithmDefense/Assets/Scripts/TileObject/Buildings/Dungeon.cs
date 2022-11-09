using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : BaseBuilding
{
    [SerializeField]
    private RangeFloat _sapwnTime;
    [SerializeField]
    private int _archerMaxCount;
    [SerializeField]
    private int _wizardMaxCount;

    private List<int> _randomMonsterList = new();

    private int _archerCurrentCount = 0;
    private int _wizardCurrentCount = 0;

    private System.Random _random = new();

    private void Awake()
    {
        LoadingControl.GetInstance.LoadingCompleteAction += StartSpawn;
    }

    public override void EnterTheBuilding(CitizenUnitController citizen)
    {

    }

    private void StartSpawn()
    {
        if (PlayerPrefs.GetInt("Num") == 0)
        {
            return;
        }

        StartCoroutine(SpawnMonster());
    }

    private IEnumerator SpawnMonster()
    {
        while (true)
        {
            float waitTime = Random.Range(_sapwnTime.min, _sapwnTime.max);

            yield return new WaitForSeconds(waitTime);

            _randomMonsterList.Clear();
            _randomMonsterList.Add((int)Define.Job.Warrior);

            if (_archerCurrentCount > _archerMaxCount)
            {
                _randomMonsterList.Add((int)Define.Job.Archer);
            }

            if (_wizardCurrentCount > _wizardMaxCount)
            {
                _randomMonsterList.Add((int)Define.Job.Wizard);
            }

            int randIndex = _random.Next(_randomMonsterList.Count);
            int randMonster = _randomMonsterList[randIndex];

            CreateMonster((Define.Job)randMonster);

            if (randMonster == (int)Define.Job.Archer)
            {
                _archerCurrentCount = -1;
            }
            else if (randMonster == (int)Define.Job.Wizard)
            {
                _wizardCurrentCount = -1;
            }

            _archerCurrentCount++;
            _wizardCurrentCount++;
        }
    }

    private void CreateMonster(Define.Job job)
    {
        var go = Managers.Resource.Instantiate($"{Define.MONSTER_UNIT_PREFAB_PATH}Goblin_{job}");
        var monster = go.GetComponent<BattleUnitController>();
        monster.transform.position = transform.position;
        monster.Data.MoveType = Define.Move.Down;
        monster.Data.CurrentHp = monster.Data.MaxHp;
        SetUnitPosition(monster, Define.Move.Down);
    }

    protected override void Init()
    {
        HasUI = false;
    }
}
