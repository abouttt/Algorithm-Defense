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

    //���� ����� GateWay prtfab(Clone)
    private JobTrainingCenter _jobTraining;



    private void OnEnable()
    {
        _jobTraining = CurrentBuilding.GetComponent<JobTrainingCenter>();

        //Ŭ���� ����
        if (_jobTraining.JobType < Define.Job.Golem)
        {
            NomalClassButtonKlick();
        }
        else
        {
            UniqueClassButtonKlick();
        }


        //��� ����
        SetupJobTrainingInfo();
    }


    public void NomalClassButtonKlick()
    {

        NomalClass.SetActive(true);
        UniqueClass.SetActive(false);
    }

    public void UniqueClassButtonKlick()
    {
        NomalClass.SetActive(false);
        UniqueClass.SetActive(true);
    }



    public void AllOffToggles()
    {
        //���� �����ִ� ��� ��� �ݱ�
        _jobToggleGroups.SetAllTogglesOff();
        _moveToggleGroups.SetAllTogglesOff();

    }

    private void SetupJobTrainingInfo()
    {

        if (_jobTraining.JobType != Define.Job.None)
        {
            var toggle = findJobToggle(_jobTraining.JobType);
            toggle.isOn = true;
        }

        if (_jobTraining.MoveType != Define.Move.None)
        {
            var toggle = findMoveToggle(_jobTraining.MoveType);
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
            _jobTraining.MoveType = info.MoveType;

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
            _jobTraining.JobType = info.JobType;

            //�ǹ��� �ش� ���� ����ϵ��� ����
            _jobTraining.SetJobType(info.JobType);
        }


        //��� ��� �ݱ�(�ܻ� ������)
        AllOffToggles();

        NomalClass.SetActive(true);
        UniqueClass.SetActive(true);

        //UI �ݱ�
        UI_BuildingMenager.GetInstance.CloseUIController();

    }


    public override void Clear()
    {
        AllOffToggles();


        NomalClass.SetActive(true);
        UniqueClass.SetActive(true);
    }

}