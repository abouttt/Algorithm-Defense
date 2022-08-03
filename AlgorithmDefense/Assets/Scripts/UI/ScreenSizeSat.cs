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

        //해상도 목록 받아오기
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate == 60)
            {
                _resolutions.Add(Screen.resolutions[i]);
            }
        }

        //기존항목 지우기
        ResolutionsDropdown.options.Clear();

        //해상도 목록만큼 반복
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

        //새로고침
        ResolutionsDropdown.RefreshShownValue();
        //전체화면 초기화
        FullscreenButton.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    //velue 값
    public void DropboxOptionChange(int x)
    {
        _resolutionNum = x;
    }

    //전체화면 선택 토글버튼
    public void FullScreenButten(bool isFull)
    {
        _screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }


    //변경버튼 눌렀을 때
    public void ChangeButtenClik()
    {
        //화면 변경
        Screen.SetResolution(_resolutions[_resolutionNum].width,
            _resolutions[_resolutionNum].height, _screenMode);
    }


}
