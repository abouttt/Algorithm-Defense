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

        //해상도 목록 받아오기
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate == 60)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }

        //기존항목 지우기
        resolutionsDropdown.options.Clear();

        //해상도 목록만큼 반복
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

        //새로고침
        resolutionsDropdown.RefreshShownValue();
        //전체화면 초기화
        fullscreenButten.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    //velue 값
    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }

    //전체화면 선택 토글버튼
    public void FullScreenButten(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }


    //변경버튼 눌렀을 때
    public void ChangeButtenClik()
    {
        //화면 변경
        Screen.SetResolution(resolutions[resolutionNum].width,
            resolutions[resolutionNum].height, screenMode);
    }


}
