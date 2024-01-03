using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEvent_2 : TutorialBaseEvent
{
    [SerializeField]
    private Vector3Int _warriorCenterPos;
    [SerializeField]
    private int _monsterHp;

    private GameObject _monster;
    private GameObject _battleUnit;

    public override void InitEvent()
    {
        base.InitEvent();
        TileManager.GetInstance.SetTile(Define.Tilemap.Building, _warriorCenterPos, null);
        TileManager.GetInstance.SetTile(Define.Tilemap.Building, _warriorCenterPos, Define.Building.WarriorCenter);
        Util.GetBuilding<JobCenter>(_warriorCenterPos).ChangeOutputDir();
        Util.GetBuilding<JobCenter>(_warriorCenterPos).ChangeOutputDir();
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
            StartCoroutine(EndEvent());
        }
    }

    private IEnumerator EndEvent()
    {
        yield return new WaitForSeconds(1f);
        IsSuccessEvent = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_monster)
        {
            _battleUnit = collision.gameObject;
            _monster = CreateMonster(3, 30);
        }
    }
}
