using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEvent_7 : TutorialBaseEvent
{
    [SerializeField]
    private int _monsterHp;
    private GameObject[] _monsters = new GameObject[3];

    public override void InitEvent()
    {

    }

    public override void StartEvent()
    {
        _monsters[0] = CreateMonster(1, _monsterHp);
        _monsters[1] = CreateMonster(3, _monsterHp);
        _monsters[2] = CreateMonster(5, _monsterHp);
    }

    public override void CheckEvent()
    {
        bool flag = false;
        for (int i = 0; i < _monsters.Length; i++)
        {
            if (_monsters[i].activeSelf)
            {
                flag = true;
                break;
            }
        }

        if (!flag)
        {
            StartCoroutine(EndEvent());
        }
    }

    private IEnumerator EndEvent()
    {
        yield return new WaitForSeconds(1f);
        IsSuccessEvent = true;
    }
}
