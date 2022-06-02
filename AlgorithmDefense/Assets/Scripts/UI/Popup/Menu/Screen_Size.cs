using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Screen_Size : MonoBehaviour
{
    FullScreenMode screenMode;
    public Dropdown resolutionsDropdown;
    public Toggle fullscreenButten;
    List<Resolution> resolutions = new List<Resolution>();
    int resolutionNum;


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
                resolutions.Add(Screen.resolutions[i]);
            }
        }

        //�����׸� �����
        resolutionsDropdown.options.Clear();

        //�ػ� ��ϸ�ŭ �ݺ�
        foreach (Resolution item in resolutions)
        {
            Dropdown.OptionData Option = new Dropdown.OptionData();
            Option.text = item.width + "x" + item.height + " " + item.refreshRate + "hz";
            resolutionsDropdown.options.Add(Option);


            if (item.width == Screen.width && item.height == Screen.height)
            {
                resolutionsDropdown.value = optionNum;
            }
            optionNum++;
        }

        //���ΰ�ħ
        resolutionsDropdown.RefreshShownValue();
        //��üȭ�� �ʱ�ȭ
        fullscreenButten.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    //velue ��
    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }

    //��üȭ�� ���� ��۹�ư
    public void FullScreenButten(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }


    //�����ư ������ ��
    public void ChangeButtenClik()
    {
        //ȭ�� ����
        Screen.SetResolution(resolutions[resolutionNum].width,
            resolutions[resolutionNum].height, screenMode);
    }


}
