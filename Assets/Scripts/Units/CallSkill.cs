using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallSkill : MonoBehaviour
{
    [SerializeField]
    private bool _spawn1, _spawn2, _spawn3 = false;
    public bool _isSpawning1, _isSpawning2, _isSpawning3 = false;

    public GameObject[] SkillPref;
    public float Cooldown1, Cooldown2, Cooldown3 = 5f;

    public void skill(int num)
    {
        switch (num)
        {
            case 0:
                _spawn1 = true;
                if (_spawn1 && !_isSpawning1)
                {
                    StartCoroutine(SpawnSkill1(SkillPref[0].name));
                    _isSpawning1 = true;
                   
                }
                return;
            case 1:
                _spawn2 = true;
                if (_spawn2 && !_isSpawning2)
                {
                    StartCoroutine(SpawnSkill2(SkillPref[1].name));
                    _isSpawning2 = true;
                }
                return;
            case 2:
                _spawn3 = true;
                if (_spawn3 && !_isSpawning3)
                {
                    StartCoroutine(SpawnSkill3(SkillPref[2].name));
                    _isSpawning3 = true;
                }
                return;
        }

    }



    public IEnumerator SpawnSkill1(string ID)
    {
        while (true)
        {
            if (!_spawn1)
            {
                _isSpawning1 = false;
                int firstGoid = Managers.Game.Gold;
                if (firstGoid >= UI_TileSpawnController.GetInstance.BuildButtons[0]._cost)
                {
                    UI_TileSpawnController.GetInstance.BuildButtons[0].ButtonObj.GetComponent<Image>().color = new Color(1f, 1f, 1f);
                }
                yield break;
            }

            Managers.Resource.Instantiate($"Prefabs/Skill/{ID}");
            Managers.Sound.Play($"Skill/skill_1");

            yield return YieldCache.WaitForSeconds(Cooldown1);
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
                int firstGoid = Managers.Game.Gold;
                if (firstGoid >= UI_TileSpawnController.GetInstance.BuildButtons[1]._cost)
                {
                    UI_TileSpawnController.GetInstance.BuildButtons[1].ButtonObj.GetComponent<Image>().color = new Color(1f, 1f, 1f);
                }
                yield break;
            }

            Managers.Resource.Instantiate($"Prefabs/Skill/{ID}");
            Managers.Sound.Play($"Skill/skill_2");

            yield return YieldCache.WaitForSeconds(Cooldown2);
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
                int firstGoid = Managers.Game.Gold;
                if (firstGoid >= UI_TileSpawnController.GetInstance.BuildButtons[2]._cost)
                {
                    UI_TileSpawnController.GetInstance.BuildButtons[2].ButtonObj.GetComponent<Image>().color = new Color(1f, 1f, 1f);
                }
                yield break;
            }

            Managers.Resource.Instantiate($"Prefabs/Skill/{ID}");
            Managers.Sound.Play($"Skill/skill_3");

            yield return YieldCache.WaitForSeconds(Cooldown3);
            _spawn3 = false;
        }
    }   
}
