using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;


public class UI_JobTrainingController : UI_BaseBuildingController
{
    [SerializeField]
    private ToggleGroup _jobToggleGroups;
    [SerializeField]
    private ToggleGroup _moveToggleGroups;

    [SerializeField]
    private GameObject NomalClass;
    [SerializeField]
    private GameObject UniqueClass;

    public Define.Move JobMoveType;
    public Define.Job JobClassType;

    //���� ����� GateWay prtfab(Clone)
    private GameObject ThisJobTraining;

    public void SetDirection(GameObject obj)
    {
        //Ŭ���� ����
        if (JobClassType < Define.Job.Golem)
        {
            NomalClassButtonKlick();
        }
        else
        {
            UniqueClassButtonKlick();
        }


        //��� ��� �ݱ�
        AllOffToggles();

        //��� ����
        SetupGatewayInfo();


        //���� ����� GateWay ����
        ThisJobTraining = obj;

    }


    public void AllOffToggles()
    {
        //���� �����ִ� ��� ��� �ݱ�
        _jobToggleGroups.SetAllTogglesOff();
        _moveToggleGroups.SetAllTogglesOff();

    }

    private void SetupGatewayInfo()
    {

        if (JobClassType != Define.Job.None)
        {
            var toggle = findJobToggle(JobClassType);
            toggle.isOn = true;
        }

        if (JobMoveType != Define.Move.None)
        {
            var toggle = findMoveToggle(JobMoveType);
            toggle.isOn = true;
        }

    }

    private Toggle findJobToggle(Define.Job jobType)
    {
        var toggles = _jobToggleGroups;


        return toggles.GetComponentsInChildren<UI_JobTrainingToggleSet>()
                      .First(toggle => toggle.JobType == jobType)
                      .GetComponent<Toggle>();
    }



    private Toggle findMoveToggle(Define.Move moveType)
    {
        var toggles = _moveToggleGroups;


        return toggles.GetComponentsInChildren<UI_JobTrainingToggleSet>()
                      .First(toggle => toggle.MoveType == moveType)
                      .GetComponent<Toggle>();
    }



    public void NomalClassButtonKlick()
    {

        _jobToggleGroups.SetAllTogglesOff();

        NomalClass.SetActive(true);
        UniqueClass.SetActive(false);
    }

    public void UniqueClassButtonKlick()
    {
        _jobToggleGroups.SetAllTogglesOff();

        NomalClass.SetActive(false);
        UniqueClass.SetActive(true);
    }


    public void OKButtonClick()
    {


        //Ʈ���� �׷쿡�� ã��
        var toggles = _moveToggleGroups;
        //���� Ʈ���� ��������
        var toggle = toggles.GetFirstActiveToggle();
        //���� ���ȴٸ�
        if (toggle != null)
        {
            //Ʈ���� ���� ������
            var info = toggle.GetComponent<UI_JobTrainingToggleSet>();
            //Ʈ���� ���� ������ �־���
            JobMoveType = info.MoveType;

        }

        //Ʈ���� �׷쿡�� ã��
         toggles = _jobToggleGroups;
        //���� Ʈ���� ��������
         toggle = toggles.GetFirstActiveToggle();
        //���� ���ȴٸ�
        if (toggle != null)
        {
            //Ʈ���� ���� ������
            var info = toggle.GetComponent<UI_JobTrainingToggleSet>();
            //Ʈ���� ���� ������ �־���
            JobClassType = info.JobType;
            //SetJobType(info.JobType);

        }

    
        // ����� GateWay ������ ������Ʈ

        //��� ��� �ݱ�(�ܻ� ������)
        AllOffToggles();

        NomalClass.SetActive(true);
        UniqueClass.SetActive(true);

        //UI �ݱ�
        UI_BuildingMenager.GetInstance.CloseUIController();

    }

    public void DestructionButtonClick()

    {
        //�ǹ� ����



        //UI �ݱ�
        UI_BuildingMenager.GetInstance.CloseUIController();

    }

    public override void Clear()
    {
        throw new NotImplementedException();
    }
}