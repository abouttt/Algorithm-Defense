using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using static Define;


public class UI_CitizenCountController : UI_BaseBuildingController
{
    public int CitizenCount;

    [SerializeField]
    private Toggle[] _toggles;
    [SerializeField]
    private Text countText;

    //생성된 프리탭 데이터
    //private GatewayWithCount _withCount;
    //오류출력 TEXT
    private UI_NoticeTextSet GetNoticeInstance;


    private void OnEnable()
    {
        //_withCount = CurrentBuilding.GetComponent<GatewayWithCount>();

        SetupWithCountInfo();

    }


    public void SetupWithCountInfo()
    {
        //countText.text = _withCount.Count.ToString();

        //CitizenCount = _withCount.Count;

        for (int i = 0; i < _toggles.Length; i++)
        {
            //if (_withCount.DirectionCondition[i].IsOn)
            //{
                _toggles[i].isOn = true;
            //}
        }

    }


    public void OKButtonClick()
    {
        //방향버튼 갯수 세는용도
        int count = 0;

        for (int i = 0; i < _toggles.Length; i++)
        {

            if (_toggles[i].isOn == true)
            {
                //_withCount.DirectionCondition[i].IsOn = true;
                count++;
            }
            else
            {
                //_withCount.DirectionCondition[i].IsOn = false;
            }
        }



        if (count < 2 || count > 3)
        {

            //GetNoticeInstance.CanNotBeBuilt();
            return;
        }

        //_withCount.Count = CitizenCount;



        for (int i = 0; i < _toggles.Length; i++)
        {
            _toggles[i].isOn = false;


        }

        //UI 닫기
        UI_BuildingMenager.GetInstance.CloseUIController();

    }





    public void CountPlusButtonClick()
    {
        if (CitizenCount != 10)
        {
            CitizenCount++;

            countText.text = CitizenCount.ToString();
        }

    }
    public void CountMinusButtonClick()
    {
        if (CitizenCount != 0)
        {
            CitizenCount--;

            countText.text = CitizenCount.ToString();
        }

    }


    public override void Clear()
    {
        for (int i = 0; i < _toggles.Length; i++)
        {
            _toggles[i].isOn = false;

        }


        CitizenCount = 0;
    }

}
