using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Menu : UI_Popup
{ 
    enum Buttons
    {
        AgainButton,
        SettingButton,
        ExitButton,
    }

    private Button _againButton;
    private Button _settingButton;
    private Button _exitButton;

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        _againButton = GetButton((int)Buttons.AgainButton);
        _settingButton = GetButton((int)Buttons.SettingButton);
        _exitButton = GetButton((int)Buttons.ExitButton);

        BindEvent(_exitButton.gameObject, GameExit, Define.UIEvent.Click);
        BindEvent(_againButton.gameObject, Test, Define.UIEvent.Click);
    }

    public void GameExit(PointerEventData data)
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void Test(PointerEventData data)
    {
        Debug.Log("Test");
    }

}
