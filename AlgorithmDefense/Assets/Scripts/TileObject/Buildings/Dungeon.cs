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
    private int _damage;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private float _attackDelay;
    [SerializeField]
    private RaycastHit2D[] _targetList = new RaycastHit2D[10];

    private List<int> _randomMonsterList = new();

    private float timer = 0f;
    private int _archerCurrentCount = 0;
    private int _wizardCurrentCount = 0;

    private System.Random _random = new();

    private void Awake()
    {
        LoadingControl.GetInstance.LoadingCompleteAction += StartSpawn;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > _attackDelay)
        {
            int hit = Physics2D.RaycastNonAlloc(transform.position, Vector2.down, _targetList, _attackRange, LayerMask.GetMask("Human"));

            Debug.Log(hit);

            if (hit >= 5)
            {
                //Debug.Log("H");

                for (int a = 0; a < 4; a++)
                {
                    if(a != 0)
                    {
                        var go = Managers.Resource.Instantiate($"{Define.SKILL_PREFAB_PATH}Skill_Mob");
                        go.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f * a);
                        //°´Ã¼ ¼ÒÈ¯ µô·¹ÀÌ 0.5f ¿äÃ»
                    }

                }

                //var projectile = go.GetComponent<ProjectileController>();
                //projectile.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                //projectile.transform.position = transform.position;
                //projectile.Damage = _damage;
                //projectile.Target = _targetList[0].collider.gameObject;
            }

            timer = 0f;
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
