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
            IsSuccessEvent = true;
        }
    }

    private void SpawnMonster()
    {
        _monster1 = CreateMonster(1);
        _monster2 = CreateMonster(3);
        _monster3 = CreateMonster(5);
    }

    private GameObject CreateMonster(int x)
    {
        var dungeonPos = new Vector3(
            Managers.Game.Setting.StartPosition.x + x,
            Managers.Game.Setting.RampartHeight + Managers.Game.Setting.BattleLineLength,
            0);

        var pos = TileManager.GetInstance.GetWorldToCellCenterToWorld(Define.Tilemap.Ground, dungeonPos);
        pos -= new Vector3(0, 0.5f, 0);

        var go = Managers.Resource.Instantiate($"{Define.MONSTER_UNIT_PREFAB_PATH}Goblin_Warrior", pos);
        var monster = go.GetComponent<BattleUnitController>();
        monster.transform.position = pos;
        monster.Data.CurrentHp = _monsterHp;
        monster.Data.MoveType = Define.Move.Down;
        return go;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SpawnMonster();
    }
}
