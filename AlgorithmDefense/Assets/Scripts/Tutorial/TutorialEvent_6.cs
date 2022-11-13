using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEvent_6 : TutorialBaseEvent
{
    private void Awake()
    {
        Managers.Pool.Clear();
    }

    public override void InitEvent()
    {
        Clear();
    }

    public override void StartEvent()
    {
        
    }

    public override void CheckEvent()
    {
        
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
