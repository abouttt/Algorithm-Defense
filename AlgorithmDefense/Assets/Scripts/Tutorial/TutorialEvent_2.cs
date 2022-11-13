using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEvent_2 : TutorialBaseEvent
{
    [SerializeField]
    private int _monsterHp;

    private GameObject _monster;
    private GameObject _battleUnit;

    public override void InitEvent()
    {

    }

    public override void StartEvent()
    {
        var pos = TileManager.GetInstance.GetCellCenterToWorld(Define.Tilemap.Ground, Managers.Game.Setting.SpawnCellPos);
        var go = Managers.Resource.Instantiate($"{Define.CITIZEN_PREFAB_PATH}RedCitizen", pos);
        var citizen = go.GetComponent<CitizenUnitController>();
        citizen.Data.MoveType = Define.Move.Up;
        citizen.SetNextDestination(citizen.transform.position);
    }

    public override void CheckEvent()
    {
        if (_monster && !_monster.activeSelf)
        {
            Managers.Resource.Destroy(_battleUnit);
            IsSuccessEvent = true;
        }
    }

    private void SpawnMonster()
    {
        var dungeonPos = new Vector3(
            Managers.Game.Setting.StartPosition.x + 3,
            Managers.Game.Setting.RampartHeight + Managers.Game.Setting.BattleLineLength,
            0);
        var pos = TileManager.GetInstance.GetWorldToCellCenterToWorld(Define.Tilemap.Ground, dungeonPos);
        pos -= new Vector3(0, 0.5f, 0);
        _monster = Managers.Resource.Instantiate($"{Define.MONSTER_UNIT_PREFAB_PATH}Goblin_Warrior", pos);
        var monster = _monster.GetComponent<BattleUnitController>();
        monster.transform.position = pos;
        monster.Data.CurrentHp = _monsterHp;
        monster.Data.MoveType = Define.Move.Down;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_monster)
        {
            _battleUnit = collision.gameObject;
            SpawnMonster();
        }
    }
}
