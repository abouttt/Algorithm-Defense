using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BuildingMenager : MonoBehaviour
{
    private static UI_BuildingMenager s_instance;
    public static UI_BuildingMenager GetInstance { get { Init(); return s_instance; } }

    public GameObject GatewayUIController;
    public GameObject GatewayCountUIController;
    public GameObject JobTrainingUIController;
    public GameObject MagicFactoryUIController;

    //현재 띄워져 있는 UI
    private GameObject _currentShowUIController;

    private void Awake()
    {
        Init();
    }


    //건물UI 켜기
    public void ShowUIController(Define.Building building, GameObject obj)
    {
        if (_currentShowUIController)
        {
            return;
        }
    
        //게이트웨이
        if (building == Define.Building.Gateway)
        {
            //UI 키기
            GatewayUIController.SetActive(true);
            //UI와 연결된 gateway 오브젝트 전달
            obj.GetComponent<Gateway>().GateWayInformationTransfer(obj);
            _currentShowUIController = GatewayUIController;
        }

        //게이트웨이 카운트
        else if(building == Define.Building.GatewayWithCount)
        {
            GatewayCountUIController.SetActive(true);
            obj.GetComponent<GatewayWithCount>().GatewayWithCountInformationTransfer(obj);
            _currentShowUIController = GatewayCountUIController;
        }

        //직업 훈련소
        else if (building == Define.Building.JobTrainingCenter)
        {
            JobTrainingUIController.SetActive(true);
            obj.GetComponent<JobTrainingCenter>().JobTrainingInformationTransfer(obj);
            _currentShowUIController = JobTrainingUIController;
        }

        //마법 생성소
        else if (building == Define.Building.MagicFactory)
        {
            MagicFactoryUIController.SetActive(true);
            obj.GetComponent<MagicFactory>().MagicFactoryInformationTransfer(obj);
            _currentShowUIController = MagicFactoryUIController;
        }


    }

    //건물UI 끄기
    public void CloseUIController(Define.Building building)
    {
        if (!_currentShowUIController)
        {
            Debug.Log("빌딩 메니저 리턴");
            return;
        }

      
        //if (building == Define.Building.Gateway)
        //{
        //    _currentShowUIController.SetActive(false);
        //}

        _currentShowUIController.SetActive(false);

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

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<UI_BuildingMenager>();
        }
    }
}

