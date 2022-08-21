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

    //현재 연결된 GateWay prtfab(Clone)
    private JobTrainingCenter _jobTraining;



    private void OnEnable()
    {
        _jobTraining = CurrentBuilding.GetComponent<JobTrainingCenter>();

        //클래스 구분
        if (_jobTraining.JobType < Define.Job.Golem)
        {
            NomalClassButtonKlick();
        }
        else
        {
            UniqueClassButtonKlick();
        }


        //토글 설정
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
        //현재 켜져있는 토글 모두 닫기
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


        //트리거 그룹에서 찾기
        var toggles = _moveToggleGroups;
        //눌린 트리거 가져오기
        var toggle = toggles.GetFirstActiveToggle();
        //무언가 눌렸다면
        if (toggle != null)
        {
            //트리거 정보 가져옴
            var info = toggle.GetComponent<UI_JobTrainingToggleSet>();
            //트리거 색깔에 움직임 넣어줌
            _jobTraining.MoveType = info.MoveType;

        }

        //트리거 그룹에서 찾기
        toggles = _jobToggleGroups;
        //눌린 트리거 가져오기
        toggle = toggles.GetFirstActiveToggle();
        //무언가 눌렸다면
        if (toggle != null)
        {
            //트리거 정보 가져옴
            var info = toggle.GetComponent<UI_JobTrainingToggleSet>();
            //트리거 색깔에 움직임 넣어줌
            _jobTraining.JobType = info.JobType;

            //건물에 해당 유닛 출력하도록 실행
            _jobTraining.SetJobType(info.JobType);
        }


        //모든 토글 닫기(잔상 때문에)
        AllOffToggles();

        NomalClass.SetActive(true);
        UniqueClass.SetActive(true);

        //UI 닫기
        UI_BuildingMenager.GetInstance.CloseUIController();

    }


    public override void Clear()
    {
        AllOffToggles();


        NomalClass.SetActive(true);
        UniqueClass.SetActive(true);
    }

}