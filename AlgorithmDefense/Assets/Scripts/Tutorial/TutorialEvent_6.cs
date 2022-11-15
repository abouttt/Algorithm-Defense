using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEvent_6 : TutorialBaseEvent
{
    [SerializeField]
    private Vector3Int _goldMinePos;
    [SerializeField]
    private float _spawnTime;

    private Coroutine _coroutine;

    private void Awake()
    {
        RoadBuilder.GetInstance.ConnectedRoadDoneAction += CheckConnectedToGoldMine;
    }

    public override void InitEvent()
    {
        Managers.Pool.Clear();
        ClearBuildingAndRoad();
        InitBuilding();
    }

    public override void StartEvent()
    {

    }

    public override void CheckEvent()
    {
        if (Managers.Game.Gold == 100)
        {
            IsSuccessEvent = true;
        }
    }

    private void CheckConnectedToGoldMine()
    {
        if (_coroutine == null)
        {
            if (IsConnectedToGoldMine())
            {
                _coroutine = StartCoroutine(SpawnCitizen());
            }
            else
            {
                IsFailureEvent = true;
            }
        }
    }

    private IEnumerator SpawnCitizen()
    {
        for (int i = 1; i <= 2; i++)
        {
            CreateCitizen((Define.Citizen)i);

            yield return new WaitForSeconds(2f);
        }
    }

    private bool IsConnectedToGoldMine()
    {
        foreach (var item in RoadBuilder.GetInstance.RoadGroupDic)
        {
            if ((item.Value[0] == (Managers.Game.Setting.SpawnCellPos + Vector3Int.up)) &&
                (item.Value[item.Value.Count - 1] == _goldMinePos))
            {
                return true;
            }
            else if ((item.Value[item.Value.Count - 1] == (Managers.Game.Setting.SpawnCellPos + Vector3Int.up)) &&
                     (item.Value[0] == _goldMinePos))
            {
                return true;
            }
        }

        return false;
    }

    private void InitBuilding()
    {
        TileManager.GetInstance.SetTile(Define.Tilemap.Building, _goldMinePos, Define.Building.GoldMine);
    }
}
