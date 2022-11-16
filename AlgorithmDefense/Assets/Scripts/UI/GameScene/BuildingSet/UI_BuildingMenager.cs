using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BuildingMenager : MonoBehaviour
{
    private static UI_BuildingMenager s_instance;
    public static UI_BuildingMenager GetInstance { get { Init(); return s_instance; } }

    public UI_BaseBuildingController GatewayUIController;
    //public UI_BaseBuildingController GatewayCountUIController;
    //public UI_BaseBuildingController JobTrainingUIController;
    //public UI_BaseBuildingController MagicFactoryUIController;

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
                Managers.Sound.Play("UI/Complete_The_Gateway_Setup", Define.Sound.UI);
                break;

        }

        //좌표값 수정중

        //GameObject obj = Instantiate(_currentShowUIController.gameObject, go.transform);

        //var pos = Camera.main.WorldToScreenPoint(go.transform.localPosition);
        //obj.transform.GetChild(0).localPosition = pos;

        //Debug.Log(pos);

        //obj.GetComponent<UI_BaseBuildingController>().CurrentBuilding = go;
        //obj.SetActive(true);

        _currentShowUIController.gameObject.SetActive(true);

        var pos = TileManager.GetInstance.GetWorldToCellCenterToWorld(Define.Tilemap.Building, go.transform.position);
        _currentShowUIController.transform.GetChild(0).position = pos;


        _currentShowUIController.CurrentBuilding = go;
        _currentShowUIController.GetComponent<UI_CitizenDirectionController>().OnUI();


    }

    //건물UI 끄기
    public void CloseUIController()
    {
        if (!_currentShowUIController)
        {
            return;
        }

        Managers.Sound.Play("UI/Complete_The_Gateway_Setup", Define.Sound.UI);
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

