using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEvent_5 : TutorialBaseEvent
{
    [SerializeField]
    private int _monsterHp;

    [SerializeField]
    private Vector3Int _gatewayPos;
    [SerializeField]
    private Vector3Int _warriorCenterPos;
    [SerializeField]
    private Vector3Int _archerCenterPos;
    [SerializeField]
    private Vector3Int _wizardCenterPos;

    private TutorialJobCenter _warriorCenter;
    private TutorialJobCenter _archerCenter;
    private TutorialJobCenter _wizardCenter;

    private GameObject _monster1;
    private GameObject _monster2;
    private GameObject _monster3;

    private void Awake()
    {
        transform.Find("Canvas").GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public override void InitEvent()
    {
        base.InitEvent();
        _warriorCenter = Util.GetBuilding<TutorialJobCenter>(_warriorCenterPos);
        _archerCenter = Util.GetBuilding<TutorialJobCenter>(_archerCenterPos);
        _wizardCenter = Util.GetBuilding<TutorialJobCenter>(_wizardCenterPos);
    }

    public override void StartEvent()
    {
        _warriorCenter.Release();
        _archerCenter.Release();
        _wizardCenter.Release();
    }

    public override void CheckEvent()
    {
        if (!_monster1 || !_monster2 || !_monster3)
        {
            return;
        }

        if (!_monster1.activeSelf &&
            !_monster2.activeSelf &&
            !_monster3.activeSelf)
        {
            StartCoroutine(EndEvent());
        }
    }

    private void SpawnMonster()
    {
        _monster1 = CreateMonster(1, 50);
        _monster2 = CreateMonster(3, 50);
        _monster3 = CreateMonster(5, 50);
    }

    private IEnumerator EndEvent()
    {
        yield return YieldCache.WaitForSeconds(1f);

        IsSuccessEvent = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SpawnMonster();
    }
}
