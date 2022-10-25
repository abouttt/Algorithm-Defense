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

        //�ǹ� ����
        TileManager.GetInstance.SetTile(Define.Tilemap.Building, cellPos, null);
        TileManager.GetInstance.SetTile(Define.Tilemap.Road, cellPos, null);

        //UI �ݱ�
        UI_BuildingMenager.GetInstance.CloseUIController();
    }
}
