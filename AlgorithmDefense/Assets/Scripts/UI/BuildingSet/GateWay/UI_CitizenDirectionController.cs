using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class UI_CitizenDirectionController : Gateway
{
    [SerializeField]
    private ToggleGroup[] _toggleGroups;

    public  Dictionary<Define.Citizen, Define.Move> ToggleDirection;

    //현재 연결된 GateWay prtfab(Clone)
    private GameObject ThisGateWay;


    public  void SetDirection(GameObject obj)
    {
        //모든 토글 닫기
        AllOffToggles();

        //토글 설정
        setupGatewayInfo();

        //현재 연결된 GateWay 저장
        ThisGateWay = obj;
     
    }

    public void AllOffToggles()
    { 
        //현재 켜져있는 토글 모두 닫기
        for (int i = 0; i < _toggleGroups.Length; i++)
        {
            _toggleGroups[i].SetAllTogglesOff();

        }
    }

    private void setupGatewayInfo()
    {
        foreach (var condition in ToggleDirection)
        {
            if (condition.Value != Define.Move.None)
            {
                var toggle = findToggle(condition.Key, condition.Value);
                toggle.isOn = true;
            }
        }
    }

    private Toggle findToggle(Define.Citizen citizenType, Define.Move moveType)
    {
        foreach (var toggles in _toggleGroups)
        {
            if (toggles.name.Contains(Enum.GetName(typeof(Define.Citizen), citizenType)))
            {
                return toggles.GetComponentsInChildren<UI_DirectionToggleSet>()
                              .First(toggle => toggle.MoveType == moveType)
                              .GetComponent<Toggle>();
            }
        }

        return null;
    }


    public void OKButtonClick()
    {

       
        for (int citizenIdx = 0; citizenIdx < ToggleDirection.Count; citizenIdx++)
        {

            ToggleDirection[(Define.Citizen)citizenIdx + 1] = Define.Move.None;

          
        }

        //트리거 그룹에서 찾기
        foreach (var toggles in _toggleGroups)
        {
            //눌린 트리거 가져오기
            var toggle = toggles.GetFirstActiveToggle();
            //무언가 눌렸다면
            if (toggle != null)
            {
                //트리거 정보 가져옴
                var info = toggle.GetComponent<UI_DirectionToggleSet>();
                //트리거 색깔에 움직임 넣어줌
                ToggleDirection[info.CitizenType] = info.MoveType;

            }
        }

        // 연결된 GateWay 데이터 업데이트
        ThisGateWay.GetComponent<Gateway>().SetChangeValue = true;

        //모든 토글 닫기(잔상 때문에)
        AllOffToggles();

        //UI 닫기
        UI_BuildingMenager.GetInstance.CloseUIController(Define.Building.Gateway);
     
    }



}

