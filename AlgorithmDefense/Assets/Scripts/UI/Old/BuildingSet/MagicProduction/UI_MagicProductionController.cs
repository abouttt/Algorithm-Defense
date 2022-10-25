using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class UI_MagicProductionController : UI_BaseBuildingController
{

    [SerializeField]
    private ToggleGroup[] _toggleGroups;

    //현재 연결된 GateWay prtfab(Clone)
    //private MagicFactory _magicFactory;


    private void OnEnable()
    {
        //_magicFactory = CurrentBuilding.GetComponent<MagicFactory>();

        //토글 설정
        SetupMagicFactoryInfo();
    }


    public void AllOffToggles()
    {
        //현재 켜져있는 토글 모두 닫기
        for (int i = 0; i < _toggleGroups.Length; i++)
        {
            _toggleGroups[i].SetAllTogglesOff();

        }
    }

    private void SetupMagicFactoryInfo()
    {

        //if (_magicFactory.MagicType != Define.Magic.None)
        //{
        //    var toggle = findToggle(_magicFactory.MagicType);
        //    toggle.isOn = true;
        //}

    }

    private Toggle findToggle(Define.Magic magicType)
    {
        foreach (var toggles in _toggleGroups)
        {
            return toggles.GetComponentsInChildren<UI_MagicToggleSet>()
                          .First(toggle => toggle.MagicType == magicType)
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
                var info = toggle.GetComponent<UI_MagicToggleSet>();
                //트리거 색깔에 움직임 넣어줌
                //_magicFactory.MagicType = info.MagicType;

            }
        }

        //모든 토글 닫기(잔상 때문에)
        AllOffToggles();

        //UI 닫기
        UI_BuildingMenager.GetInstance.CloseUIController();

    }

    public override void Clear()
    {
        AllOffToggles();
    }

}
