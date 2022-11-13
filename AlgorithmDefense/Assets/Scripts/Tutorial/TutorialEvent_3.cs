using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEvent_3 : TutorialBaseEvent
{
    [SerializeField]
    private Vector3Int _gatewayPos;
    [SerializeField]
    private Vector3Int _warriorCenterPos;
    [SerializeField]
    private Vector3Int _archerCenterPos;
    [SerializeField]
    private Vector3Int _wizardCenterPos;

    private UI_BaseBuildingController _gatewayUI;

    private void Awake()
    {
        transform.Find("Canvas").GetComponent<Canvas>().worldCamera = Camera.main;
        Managers.Pool.Clear();
    }

    public override void InitEvent()
    {
        Clear();
        InitCastleAndDungeon();
        InitBattleLine();
        InitBuilding();
        InitRoad();

        _gatewayUI = UI_BuildingMenager.GetInstance.GatewayUIController;
    }

    public override void StartEvent()
    {
        
    }

    public override void CheckEvent()
    {
        if (_gatewayUI.gameObject.activeSelf)
        {
            IsSuccessEvent = true;
        }
    }

    private void InitCastleAndDungeon()
    {
        Managers.Game.Setting.SetCastleAndDungeon(1);
        Managers.Game.Setting.SetCastleAndDungeon(3);
        Managers.Game.Setting.SetCastleAndDungeon(5);
    }

    private void InitBattleLine()
    {
        Managers.Game.Setting.SetBattleLine(1);
        Managers.Game.Setting.SetBattleLine(3);
        Managers.Game.Setting.SetBattleLine(5);
    }

    private void InitBuilding()
    {
        TileManager.GetInstance.SetTile(Define.Tilemap.Building, _gatewayPos, Define.Building.Gateway);
        TileManager.GetInstance.SetTile(Define.Tilemap.Building, _warriorCenterPos, Define.Building.TutorialWarriorCenter);
        TileManager.GetInstance.SetTile(Define.Tilemap.Building, _archerCenterPos, Define.Building.TutorialArcherCenter);
        TileManager.GetInstance.SetTile(Define.Tilemap.Building, _wizardCenterPos, Define.Building.TutorialWizardCenter);
    }

    private void InitRoad()
    {
        // 시작길.
        {
            RoadBuilder.GetInstance.BuildWillRoads(Managers.Game.Setting.SpawnCellPos + Vector3Int.up);
            RoadBuilder.GetInstance.BuildWillRoads(Managers.Game.Setting.SpawnCellPos + Vector3Int.up + Vector3Int.up);

            RoadBuilder.GetInstance.BuildRoads();
        }

        var castlePos = Managers.Game.Setting.StartPosition + new Vector3Int(0, Managers.Game.Setting.RampartHeight, 0);

        // 전사 부여소.
        {
            for (int y = _gatewayPos.y; y <= _warriorCenterPos.y; y++)
            {
                RoadBuilder.GetInstance.BuildWillRoads(new Vector3Int(_gatewayPos.x, y, 0));
            }

            RoadBuilder.GetInstance.BuildRoads();

            for (int y = _warriorCenterPos.y; y < castlePos.y; y++)
            {
                RoadBuilder.GetInstance.BuildWillRoads(new Vector3Int(_warriorCenterPos.x, y, 0));
            }

            RoadBuilder.GetInstance.BuildRoads();
        }

        // 궁수 부여소.
        {
            for (int x = _gatewayPos.x; x >= _archerCenterPos.x; x--)
            {
                RoadBuilder.GetInstance.BuildWillRoads(new Vector3Int(x, _gatewayPos.y, 0));
            }
            for (int y = _gatewayPos.y + 1; y <= _archerCenterPos.y; y++)
            {
                RoadBuilder.GetInstance.BuildWillRoads(new Vector3Int(_archerCenterPos.x, y, 0));
            }

            RoadBuilder.GetInstance.BuildRoads();

            for (int y = _archerCenterPos.y; y < castlePos.y; y++)
            {
                RoadBuilder.GetInstance.BuildWillRoads(new Vector3Int(_archerCenterPos.x, y, 0));
            }

            RoadBuilder.GetInstance.BuildRoads();
        }

        // 마법사 부여소.
        {
            for (int x = _gatewayPos.x; x <= _wizardCenterPos.x; x++)
            {
                RoadBuilder.GetInstance.BuildWillRoads(new Vector3Int(x, _gatewayPos.y, 0));
            }
            for (int y = _gatewayPos.y + 1; y <= _wizardCenterPos.y; y++)
            {
                RoadBuilder.GetInstance.BuildWillRoads(new Vector3Int(_wizardCenterPos.x, y, 0));
            }

            RoadBuilder.GetInstance.BuildRoads();

            for (int y = _wizardCenterPos.y; y < castlePos.y; y++)
            {
                RoadBuilder.GetInstance.BuildWillRoads(new Vector3Int(_wizardCenterPos.x, y, 0));
            }

            RoadBuilder.GetInstance.BuildRoads();
        }
    }

    private void Clear()
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
