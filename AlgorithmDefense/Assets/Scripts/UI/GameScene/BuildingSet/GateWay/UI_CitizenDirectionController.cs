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
    private Button[] buttons;
    [SerializeField]
    private Sprite[] redImages;
    [SerializeField]
    private Sprite[] blueImages;
    [SerializeField]
    private Sprite[] greenImages;

    private Gateway _gateway;

    private int R, G, B;


    private void OnEnable()
    {
        //_gateway = CurrentBuilding.GetComponent<Gateway>();

        //// �ǹ� ������ �ε�.
        //setupGatewayInfo();
    }

    public void OnUI()
    {
        _gateway = CurrentBuilding.GetComponent<Gateway>();

        // �ǹ� ������ �ε�.
        setupGatewayInfo();

    }


    public void AllOffToggles()
    {
        ////���� �����ִ� ��� ��� �ݱ�
        //for (int i = 0; i < _toggleGroups.Length; i++)
        //{
        //    _toggleGroups[i].SetAllTogglesOff();

        //}

        buttons[0].GetComponent<Image>().sprite = redImages[0];
        buttons[1].GetComponent<Image>().sprite = blueImages[0];
        buttons[2].GetComponent<Image>().sprite = greenImages[0];

        R = 0;
        G = 0;
        B = 0;

    }

    private void setupGatewayInfo()
    {    
        foreach (var condition in _gateway.DirectionCondition)
        {
           
            if (condition.Value != Define.Move.None)
            {
                switch(condition.Key)
                {
                    case Define.Citizen.Red:
                        buttons[0].GetComponent<Image>().sprite = redImages[(int)condition.Value];
                        R = (int)condition.Value;
                        break;
                    case Define.Citizen.Blue:
                        buttons[1].GetComponent<Image>().sprite = blueImages[(int)condition.Value];
                        B = (int)condition.Value;
                        break;
                    case Define.Citizen.Green:
                        buttons[2].GetComponent<Image>().sprite = greenImages[(int)condition.Value];
                        G = (int)condition.Value;
                        break;

                }

                //var toggle = findToggle(condition.Key, condition.Value);
                //toggle.isOn = true;


            }
        }
    }

    private Toggle findToggle(Define.Citizen citizenType, Define.Move moveType)
    {
        //foreach (var toggles in _toggleGroups)
        //{
        //    if (toggles.name.Contains(Enum.GetName(typeof(Define.Citizen), citizenType)))
        //    {
        //        return toggles.GetComponentsInChildren<UI_DirectionToggleSet>()
        //                      .First(toggle => toggle.MoveType == moveType)
        //                      .GetComponent<Toggle>();
        //    }
        //}

        return null;
    }

    public void RedButtonClick()
    {
        R++;
        if(R>=5)
        {
            R = 0;
        }

        buttons[0].GetComponent<Image>().sprite = redImages[R];
    }

    public void BlueButtonClick()
    {
        B++;
        if (B >= 5)
        {
            B = 0;
        }

        buttons[1].GetComponent<Image>().sprite = blueImages[B];
    }

    public void GreenButtonClick()
    {
        G++;
        if (G >= 5)
        {
            G = 0;
        }

        buttons[2].GetComponent<Image>().sprite = greenImages[G];
    }



    public void OKButtonClick()
    {

        //������ �ʱ�ȭ
        for (int citizenIdx = 0; citizenIdx < _gateway.DirectionCondition.Count; citizenIdx++)
        {

            _gateway.DirectionCondition[(Define.Citizen)citizenIdx + 1] = Define.Move.None;


        }

        _gateway.DirectionCondition[(Define.Citizen)1] = (Define.Move)R;
        _gateway.DirectionCondition[(Define.Citizen)2] = (Define.Move)G;
        _gateway.DirectionCondition[(Define.Citizen)3] = (Define.Move)B;



        ////Ʈ���� �׷쿡�� ã��
        //foreach (var toggles in _toggleGroups)
        //{
        //    //���� Ʈ���� ��������
        //    var toggle = toggles.GetFirstActiveToggle();
        //    //���� ���ȴٸ�
        //    if (toggle != null)
        //    {
        //        //Ʈ���� ���� ������
        //        var info = toggle.GetComponent<UI_DirectionToggleSet>();
        //        //Ʈ���� ���� ������ �־���
        //        _gateway.DirectionCondition[info.CitizenType] = info.MoveType;

        //    }
        //}

        //��� ��� �ݱ�(�ܻ� ������)
        AllOffToggles();

        //UI �ݱ�
        UI_BuildingMenager.GetInstance.CloseUIController();       

    }




    public override void Clear()
    {
        AllOffToggles();
    }

}

