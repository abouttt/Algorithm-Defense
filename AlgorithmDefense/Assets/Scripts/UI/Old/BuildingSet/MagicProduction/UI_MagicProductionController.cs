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

    //���� ����� GateWay prtfab(Clone)
    //private MagicFactory _magicFactory;


    private void OnEnable()
    {
        //_magicFactory = CurrentBuilding.GetComponent<MagicFactory>();

        //��� ����
        SetupMagicFactoryInfo();
    }


    public void AllOffToggles()
    {
        //���� �����ִ� ��� ��� �ݱ�
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
                //_magicFactory.MagicType = info.MagicType;

            }
        }

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
