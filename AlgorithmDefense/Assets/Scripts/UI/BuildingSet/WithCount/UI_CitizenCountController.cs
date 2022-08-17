using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;


public class UI_CitizenCountController : GatewayWithCount
{

    [SerializeField]
    private ToggleGroup[] _toggleGroups;
    [SerializeField]
    private Text countText;

    public Define.Move MoveType;

    public int CitizenCount;


    //현재 연결된 GateWay prtfab(Clone)
    private GameObject ThisWithCount;

    public void SetDirection(GameObject obj)
    {
        //모든 토글 닫기
        AllOffToggles();

        //토글 설정
        SetupGatewayInfo();

        //카운트 설정
        countText.text = CitizenCount.ToString();

        //현재 연결된 GateWay 저장
        ThisWithCount = obj;

    }

    public void AllOffToggles()
    {
        //현재 켜져있는 토글 모두 닫기
        for (int i = 0; i < _toggleGroups.Length; i++)
        {
            _toggleGroups[i].SetAllTogglesOff();

        }
    }

    private void SetupGatewayInfo()
    {

        if (MoveType != Define.Move.None)
        {
            var toggle = findToggle(MoveType);
            toggle.isOn = true;
        }

    }

    private Toggle findToggle(Define.Move moveType)
    {
        foreach (var toggles in _toggleGroups)
        {
            return toggles.GetComponentsInChildren<UI_CitizenCountToggleSet>()
                          .First(toggle => toggle.MoveType == moveType)
                          .GetComponent<Toggle>();
        }

        return null;
    }



    public void OKButtonClick()
    {
        //트리거 그룹에서 찾기
        foreach (var toggles in _toggleGroups)
        {
            //눌린 트리거 가져오기
            var toggle = toggles.GetFirstActiveToggle();
            //무언가 눌렸다면
            if (toggle != null)
            {
                //트리거 정보 가져옴
                var info = toggle.GetComponent<UI_CitizenCountToggleSet>();
                //트리거 색깔에 움직임 넣어줌
                MoveType = info.MoveType;

            }
        }

        // 연결된 GateWay 데이터 업데이트
        ThisWithCount.GetComponent<GatewayWithCount>().SetChangeValue = true;

        //모든 토글 닫기(잔상 때문에)
        AllOffToggles();

        //UI 닫기
        UI_BuildingMenager.GetInstance.CloseUIController(Define.Building.GatewayWithCount);

    }

    public void DestructionButtonClick()

    {
        //건물 삭제


        //UI 닫기
        UI_BuildingMenager.GetInstance.CloseUIController(Define.Building.GatewayWithCount);

    }

    public void CountPlusButtonClick()
    {
        CitizenCount++;

        countText.text = CitizenCount.ToString();

    }
    public void CountMinusButtonClick()
    {
        if (CitizenCount != 0)
        {
            CitizenCount--;

            countText.text = CitizenCount.ToString();
        }

    }

}
