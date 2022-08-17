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

    //���� ����� GateWay prtfab(Clone)
    private GameObject ThisGateWay;


    public  void SetDirection(GameObject obj)
    {
        //��� ��� �ݱ�
        AllOffToggles();

        //��� ����
        setupGatewayInfo();

        //���� ����� GateWay ����
        ThisGateWay = obj;
     
    }

    public void AllOffToggles()
    { 
        //���� �����ִ� ��� ��� �ݱ�
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

        //Ʈ���� �׷쿡�� ã��
        foreach (var toggles in _toggleGroups)
        {
            //���� Ʈ���� ��������
            var toggle = toggles.GetFirstActiveToggle();
            //���� ���ȴٸ�
            if (toggle != null)
            {
                //Ʈ���� ���� ������
                var info = toggle.GetComponent<UI_DirectionToggleSet>();
                //Ʈ���� ���� ������ �־���
                ToggleDirection[info.CitizenType] = info.MoveType;

            }
        }

        // ����� GateWay ������ ������Ʈ
        ThisGateWay.GetComponent<Gateway>().SetChangeValue = true;

        //��� ��� �ݱ�(�ܻ� ������)
        AllOffToggles();

        //UI �ݱ�
        UI_BuildingMenager.GetInstance.CloseUIController(Define.Building.Gateway);
     
    }



}

