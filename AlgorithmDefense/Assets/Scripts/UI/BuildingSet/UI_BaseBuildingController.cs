using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI_BaseBuildingController : MonoBehaviour
{
    public BaseBuilding CurrentBuilding;

    public abstract void Clear();
    public void DestructionButtonClick()
    {
        var cellPos = Managers.Tile.GetGrid().WorldToCell(CurrentBuilding.transform.position);

        //�ǹ� ����
        Managers.Tile.SetTile(Define.Tilemap.Building, cellPos, null);
        Managers.Tile.SetTile(Define.Tilemap.Road, cellPos, null);

        //UI �ݱ�
        UI_BuildingMenager.GetInstance.CloseUIController();
    }
}
