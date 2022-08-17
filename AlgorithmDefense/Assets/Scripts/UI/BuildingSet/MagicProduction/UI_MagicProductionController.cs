using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class UI_MagicProductionController : MagicFactory
{

    [SerializeField]
    private ToggleGroup[] _toggleGroups;

    public Define.Magic MagicType;

    //���� ����� GateWay prtfab(Clone)
    private GameObject ThisMagicFactory;


    public void SetDirection(GameObject obj)
    {
        //��� ��� �ݱ�
        AllOffToggles();

        //��� ����
        SetupGatewayInfo();

        //���� ����� GateWay ����
        ThisMagicFactory = obj;

    }

    public void AllOffToggles()
    {
        //���� �����ִ� ��� ��� �ݱ�
        for (int i = 0; i < _toggleGroups.Length; i++)
        {
            _toggleGroups[i].SetAllTogglesOff();

        }
    }

    private void SetupGatewayInfo()
    {
        
            if (MagicType != Define.Magic.None)
            {
                var toggle = findToggle(MagicType);
                toggle.isOn = true;
            }
        
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

        //Ʈ���� �׷쿡�� ã��
        foreach (var toggles in _toggleGroups)
        {
            //���� Ʈ���� ��������
            var toggle = toggles.GetFirstActiveToggle();
            //���� ���ȴٸ�
            if (toggle != null)
            {
                //Ʈ���� ���� ������
                var info = toggle.GetComponent<UI_MagicToggleSet>();
                //Ʈ���� ���� ������ �־���
                MagicType = info.MagicType;

            }
        }

        // ����� GateWay ������ ������Ʈ
        ThisMagicFactory.GetComponent<MagicFactory>().SetChangeValue = true;

        //��� ��� �ݱ�(�ܻ� ������)
        AllOffToggles();

        //UI �ݱ�
        UI_BuildingMenager.GetInstance.CloseUIController(Define.Building.MagicFactory);

    }

    public void DestructionButtonClick()

    {
        //�ǹ� ����



        //UI �ݱ�
        UI_BuildingMenager.GetInstance.CloseUIController(Define.Building.MagicFactory);

    }
}
