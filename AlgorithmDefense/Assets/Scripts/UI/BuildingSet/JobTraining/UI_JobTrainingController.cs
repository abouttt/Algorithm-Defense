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

    //현재 연결된 GateWay prtfab(Clone)
    private GameObject ThisJobTraining;

    public void SetDirection(GameObject obj)
    {
        //클래스 구분
        if (JobClassType < Define.Job.Golem)
        {
            NomalClassButtonKlick();
        }
        else
        {
            UniqueClassButtonKlick();
        }


        //모든 토글 닫기
        AllOffToggles();

        //토글 설정
        SetupGatewayInfo();


        //현재 연결된 GateWay 저장
        ThisJobTraining = obj;

    }


    public void AllOffToggles()
    {
        //현재 켜져있는 토글 모두 닫기
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
            JobMoveType = info.MoveType;

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
            JobClassType = info.JobType;
            //SetJobType(info.JobType);

        }

    
        // 연결된 GateWay 데이터 업데이트

        //모든 토글 닫기(잔상 때문에)
        AllOffToggles();

        NomalClass.SetActive(true);
        UniqueClass.SetActive(true);

        //UI 닫기
        UI_BuildingMenager.GetInstance.CloseUIController();

    }

    public void DestructionButtonClick()

    {
        //건물 삭제



        //UI 닫기
        UI_BuildingMenager.GetInstance.CloseUIController();

    }

    public override void Clear()
    {
        throw new NotImplementedException();
    }
}