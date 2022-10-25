using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTest : MonoBehaviour
{
    public GameObject SkillPref;
    [SerializeField]
    private bool _spawn = false;
    private bool _isSpawning = false;

    private void Update()
    {
        if (_spawn && !_isSpawning)
        {
            StartCoroutine(SpawnSkill());
            _isSpawning = true;
        }
    }

    public IEnumerator SpawnSkill()
    {
        while (true)
        {
            if (!_spawn)
            {
                _isSpawning = false;
                yield break;
            }

            Managers.Resource.Instantiate($"Prefabs/Skill/{SkillPref.name}");

            yield return new WaitForSeconds(5f);
        }
    }
}
