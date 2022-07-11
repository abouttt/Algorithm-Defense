using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_TileButtons : UI_Base
{
    enum BuildButtons
    {
        RoadButton,
        GatewayButton,
        ClassGiveCenterButton,
        ClassTrainingCenterButton,
    }

    private UI_TileButton[] _buttons;

    public override void Init()
    {
        Bind<UI_TileButton>(typeof(BuildButtons));
        _buttons = GetAll<UI_TileButton>();

        foreach (var button in _buttons)
        {
            BindEvent(button.gameObject, onButtonClick, Define.UIEvent.Click);
        }
    }

    private void onButtonClick(PointerEventData data)
    {
        var go = EventSystem.current.currentSelectedGameObject;
        var btnInfo = go.GetComponent<UI_TileButton>();

        switch (btnInfo.BuildType)
        {
            case Define.Build.Ground:
                RoadBuilder.GetInstance.SetTarget(btnInfo.TileObject);
                break;
            case Define.Build.Building:
                BuildingBuilder.GetInstance.SetTarget(btnInfo.TileObject);
                break;
        }
    }
}
