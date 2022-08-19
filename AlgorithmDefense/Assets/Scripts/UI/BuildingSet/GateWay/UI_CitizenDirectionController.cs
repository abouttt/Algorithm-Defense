using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class UI_CitizenDirectionController : UI_BaseBuildingController
{
    [SerializeField]
    private ToggleGroup[] _toggleGroups;

    //public Dictionary<Define.Citizen, Define.Move> ToggleDirection;

    private Gateway _gateway;

    private void OnEnable()
    {
        _gateway = CurrentBuilding.GetComponent<Gateway>();

        // 건물 데이터 로드.
        setupGatewayInfo();
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
        foreach (var condition in _gateway.DirectionCondition)
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


        for (int citizenIdx = 0; citizenIdx < _gateway.DirectionCondition.Count; citizenIdx++)
        {

            _gateway.DirectionCondition[(Define.Citizen)citizenIdx + 1] = Define.Move.None;

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
                _gateway.DirectionCondition[info.CitizenType] = info.MoveType;

            }
        }

        //모든 토글 닫기(잔상 때문에)
        AllOffToggles();

        //UI 닫기
        UI_BuildingMenager.GetInstance.CloseUIController();
    }

    public override void Clear() => AllOffToggles();
}

