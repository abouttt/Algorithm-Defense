using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEvent_3 : TutorialBaseEvent
{
    public override void InitEvent()
    {
        Clear();
        InitCastleAndDungeon();
        InitBattleLine();
    }

    public override void StartEvent()
    {

    }

    public override void CheckEvent()
    {

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

                TileManager.GetInstance.SetTile(
                    Define.Tilemap.Road,
                    new Vector3Int(
                        Managers.Game.Setting.StartPosition.x + x + 1,
                        Managers.Game.Setting.StartPosition.y + y + 1, 0),
                        null);
            }
        }

        TileManager.GetInstance.SetTile(Define.Tilemap.Road, Managers.Game.Setting.SpawnCellPos + Vector3Int.up, Define.Road.BD);
    }
}
