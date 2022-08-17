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

    //���� ����� �ִ� UI
    private GameObject _currentShowUIController;

    private void Awake()
    {
        Init();
    }


    //�ǹ�UI �ѱ�
    public void ShowUIController(Define.Building building, GameObject obj)
    {
        if (_currentShowUIController)
        {
            return;
        }
    
        //����Ʈ����
        if (building == Define.Building.Gateway)
        {
            //UI Ű��
            GatewayUIController.SetActive(true);
            //UI�� ����� gateway ������Ʈ ����
            obj.GetComponent<Gateway>().GateWayInformationTransfer(obj);
            _currentShowUIController = GatewayUIController;
        }

        //����Ʈ���� ī��Ʈ
        else if(building == Define.Building.GatewayWithCount)
        {
            GatewayCountUIController.SetActive(true);
            obj.GetComponent<GatewayWithCount>().GatewayWithCountInformationTransfer(obj);
            _currentShowUIController = GatewayCountUIController;
        }

        //���� �Ʒü�
        else if (building == Define.Building.JobTrainingCenter)
        {
            JobTrainingUIController.SetActive(true);
            obj.GetComponent<JobTrainingCenter>().JobTrainingInformationTransfer(obj);
            _currentShowUIController = JobTrainingUIController;
        }

        //���� ������
        else if (building == Define.Building.MagicFactory)
        {
            MagicFactoryUIController.SetActive(true);
            obj.GetComponent<MagicFactory>().MagicFactoryInformationTransfer(obj);
            _currentShowUIController = MagicFactoryUIController;
        }


    }

    //�ǹ�UI ����
    public void CloseUIController(Define.Building building)
    {
        if (!_currentShowUIController)
        {
            Debug.Log("���� �޴��� ����");
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

