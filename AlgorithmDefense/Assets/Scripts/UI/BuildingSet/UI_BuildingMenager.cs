using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BuildingMenager : MonoBehaviour
{
    private static UI_BuildingMenager s_instance;
    public static UI_BuildingMenager GetInstance { get { Init(); return s_instance; } }

    public UI_BaseBuildingController GatewayUIController;
    public UI_BaseBuildingController GatewayCountUIController;
    public UI_BaseBuildingController JobTrainingUIController;
    public UI_BaseBuildingController MagicFactoryUIController;

    //현재 띄워져 있는 UI
    private UI_BaseBuildingController _currentShowUIController;

    private void Awake()
    {
        Init();
    }


    //건물UI 켜기
    public void ShowUIController(Define.Building building, BaseBuilding go)
    {
        if (_currentShowUIController)
        {
            return;
        }

        switch (building)
        {
            case Define.Building.Gateway:
                _currentShowUIController = GatewayUIController;
                break;
        }

        _currentShowUIController.CurrentBuilding = go;
        _currentShowUIController.gameObject.SetActive(true);
    }

    //건물UI 끄기
    public void CloseUIController()
    {
        if (!_currentShowUIController)
        {
            Debug.Log("빌딩 메니저 리턴");
            return;
        }

        _currentShowUIController.Clear();

        _currentShowUIController.gameObject.SetActive(false);
        _currentShowUIController = null;

        
    }



    private static void Init()
    {
        if (!s_instance)
        {
            var go = GameObject.Find("Building_set_menu");
            if (!go)
            {
                go = new GameObject { name = "Building_set_menu" };
                go.AddComponent<UI_BuildingMenager>();
            }

            s_instance = go.GetComponent<UI_BuildingMenager>();
        }
    }
}

