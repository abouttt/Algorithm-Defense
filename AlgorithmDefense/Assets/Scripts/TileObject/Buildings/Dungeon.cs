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

    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private float _attackCount;
    [SerializeField]
    private int _attackUnitCount;
    [SerializeField]
    private float _attackDelay;

    private RaycastHit2D[] _targetList = new RaycastHit2D[10];
    private List<int> _randomMonsterList = new();

    private float _timer = 0f;
    private int _archerCurrentCount = 0;
    private int _wizardCurrentCount = 0;

    private System.Random _random = new();

    private void Awake()
    {
        LoadingControl.GetInstance.LoadingCompleteAction += StartSpawn;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _attackDelay)
        {
            int hit = Physics2D.RaycastNonAlloc(transform.position, Vector2.down, _targetList, _attackRange, LayerMask.GetMask("Human"));

            if (hit >= _attackUnitCount)
            {
                var go = Managers.Resource.Instantiate($"{Define.SKILL_PREFAB_PATH}Skill_Mob");
                go.transform.position = new Vector3(transform.position.x, transform.position.y - 1.5f);
                go.transform.rotation = Quaternion.Euler(0f, 0f, -180f);
            }

            _timer = 0f;
        }
    }

    public override void EnterTheBuilding(CitizenUnitController citizen)
    {

    }

    private void StartSpawn()
    {
        if (Managers.Game.Setting.IsTutorialScene)
        {
            return;
        }

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

            var monster = Util.CreateMonster((Define.Job)randMonster, transform.position);
            SetUnitPosition(monster, Define.Move.Down);

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

    protected override void Init()
    {
        HasUI = false;
    }
}
