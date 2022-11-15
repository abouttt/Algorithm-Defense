using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI_BaseBuildingController : MonoBehaviour
{
    public BaseBuilding CurrentBuilding;

    public abstract void Clear();
    public void DestructionButtonClick()
    {
        var cellPos = TileManager.GetInstance.GetGrid().WorldToCell(CurrentBuilding.transform.position);

        //건물 삭제
        TileManager.GetInstance.SetTile(Define.Tilemap.Building, cellPos, null);
        TileManager.GetInstance.SetTile(Define.Tilemap.Road, cellPos, null);

        //UI 닫기
        UI_BuildingMenager.GetInstance.CloseUIController();
    }
}
