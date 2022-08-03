using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSizeSat : MonoBehaviour
{
    
    [SerializeField]
    private Dropdown ResolutionsDropdown;
    [SerializeField]
    private Toggle FullscreenButton;

    List<Resolution> _resolutions = new List<Resolution>();
    FullScreenMode _screenMode;
    private int _resolutionNum;


    void Start()
    {
        InitUI();
    }

    void InitUI()
    {
        int optionNum = 0;

        //�ػ� ��� �޾ƿ���
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate == 60)
            {
                _resolutions.Add(Screen.resolutions[i]);
            }
        }

        //�����׸� �����
        ResolutionsDropdown.options.Clear();

        //�ػ� ��ϸ�ŭ �ݺ�
        foreach (Resolution item in _resolutions)
        {
            Dropdown.OptionData Option = new Dropdown.OptionData();
            Option.text = $"{item.width.ToString()} x {item.height.ToString()} {item.refreshRate.ToString()}hz";
            ResolutionsDropdown.options.Add(Option);


            if (item.width == Screen.width && item.height == Screen.height)
            {
                ResolutionsDropdown.value = optionNum;
            }
            optionNum++;
        }

        //���ΰ�ħ
        ResolutionsDropdown.RefreshShownValue();
        //��üȭ�� �ʱ�ȭ
        FullscreenButton.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    //velue ��
    public void DropboxOptionChange(int x)
    {
        _resolutionNum = x;
    }

    //��üȭ�� ���� ��۹�ư
    public void FullScreenButten(bool isFull)
    {
        _screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }


    //�����ư ������ ��
    public void ChangeButtenClik()
    {
        //ȭ�� ����
        Screen.SetResolution(_resolutions[_resolutionNum].width,
            _resolutions[_resolutionNum].height, _screenMode);
    }


}
