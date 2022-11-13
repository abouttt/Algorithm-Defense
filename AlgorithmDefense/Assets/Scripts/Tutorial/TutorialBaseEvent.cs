using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialBaseEvent : MonoBehaviour
{
    public List<string> TextList;
    public List<string> FailedTextList;

    [HideInInspector]
    public bool IsSuccessEvent = false;
    [HideInInspector]
    public bool IsFailureEvent = false;

    private Transform _guideUI;

    private void Start()
    {
        _guideUI = transform.Find("GuideUI");
    }

    public abstract void InitEvent();
    public abstract void StartEvent();
    public abstract void CheckEvent();

    public void SetActiveGuideUI(bool isShow)
    {
        if (_guideUI)
        {
            _guideUI.gameObject.SetActive(isShow);
        }
    }

    protected GameObject CreateCitizen(Define.Citizen citizenType)
    {
        var pos = TileManager.GetInstance.GetCellCenterToWorld(Define.Tilemap.Ground, Managers.Game.Setting.SpawnCellPos);
        var go = Managers.Resource.Instantiate($"{Define.CITIZEN_PREFAB_PATH}{citizenType}Citizen", pos);
        var citizen = go.GetComponent<CitizenUnitController>();
        citizen.Data.MoveType = Define.Move.Up;
        citizen.SetNextDestination(citizen.transform.position);
        return go;
    }

    protected GameObject CreateMonster(int x, int hp)
    {
        var dungeonPos = new Vector3Int(
            Managers.Game.Setting.StartPosition.x + x,
            Managers.Game.Setting.RampartHeight + Managers.Game.Setting.BattleLineLength,
            0);

        var pos = TileManager.GetInstance.GetCellCenterToWorld(Define.Tilemap.Ground, dungeonPos);
        pos += new Vector3(0, -0.5f, 0);

        var go = Managers.Resource.Instantiate($"{Define.MONSTER_UNIT_PREFAB_PATH}Goblin_Warrior", pos);

        var monster = go.GetComponent<BattleUnitController>();
        monster.Data.CurrentHp = hp;
        monster.Data.MoveType = Define.Move.Down;

        return go;
    }

    protected void ClearBuildingAndRoad()
    {
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                TileManager.GetInstance.SetTile(
                    Define.Tilemap.Building,
                    new Vector3Int(
                        Managers.Game.Setting.StartPosition.x + x + 1,
                        Managers.Game.Setting.StartPosition.y + y + 1, 0),
                        null);
            }
        }

        for (int i = 0; i <= RoadBuilder.GetInstance.RoadGroupCount; i++)
        {
            RoadBuilder.GetInstance.RemoveRoads(i);
        }
    }
}
