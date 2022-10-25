using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallSkill : MonoBehaviour
{
    [SerializeField]
    private bool _spawn1, _spawn2, _spawn3 = false;
    private bool _isSpawning1, _isSpawning2, _isSpawning3 = false;

    public GameObject[] SkillPref;
    public float Cooldown1, Cooldown2, Cooldown3 = 5f;

    private void Update()
    {

        skill1();
        skill2();
        skill3();

    }

    public void skill1()
    {
        if (_spawn1 && !_isSpawning1)
        {
            StartCoroutine(SpawnSkill1(SkillPref[0].name));
            _isSpawning1 = true;
        }

    }

    public void skill2()
    {
        if (_spawn2 && !_isSpawning2)
        {
            StartCoroutine(SpawnSkill2(SkillPref[1].name));
            _isSpawning2 = true;
        }
    }

    public void skill3()
    {
        if (_spawn3 && !_isSpawning3)
        {
            StartCoroutine(SpawnSkill3(SkillPref[2].name));
            _isSpawning3 = true;
        }
    }

    public IEnumerator SpawnSkill1(string ID)
    {
        while (true)
        {
            if (!_spawn1)
            {
                _isSpawning1 = false;
                yield break;
            }
            Managers.Resource.Instantiate($"Prefabs/Skill/{ID}");

            yield return new WaitForSeconds(Cooldown1);
            _spawn1 = false;
        }
    }

    public IEnumerator SpawnSkill2(string ID)
    {
        while (true)
        {
            if (!_spawn2)
            {
                _isSpawning2 = false;
                yield break;
            }
            Managers.Resource.Instantiate($"Prefabs/Skill/{ID}");

            yield return new WaitForSeconds(Cooldown2);
            _spawn2 = false;
        }
    }

    public IEnumerator SpawnSkill3(string ID)
    {
        while (true)
        {
            if (!_spawn3)
            {
                _isSpawning3 = false;
                yield break;
            }
            Managers.Resource.Instantiate($"Prefabs/Skill/{ID}");

            yield return new WaitForSeconds(Cooldown3);
            _spawn3 = false;
        }
    }
}
