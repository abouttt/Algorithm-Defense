using System;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_BuildButtons : UI_Base
{
    #region UI_Objects
    enum BuildButtons
    {
        LeftRoadButton,
        RightRoadButton,
        GatewayButton,
    }
    #endregion

    private UI_BuildButton[] _buttons = null;

    public override void Init()
    {
        Bind<UI_BuildButton>(typeof(BuildButtons));
        _buttons = GetAll<UI_BuildButton>();

        foreach (var button in _buttons)
        {
            BindEvent(button.gameObject, onButtonClick, Define.UIEvent.Click);
        }
    }

    private void onButtonClick(PointerEventData data)
    {
        var go = EventSystem.current.currentSelectedGameObject;
        var btnInfo = go.GetComponent<UI_BuildButton>();

        RoadBuilder.GetInstance.Release();
        BuildingBuilder.GetInstance.Release();

        string name = null;
        switch (btnInfo.BuildType)
        {
            case Define.BuildType.Ground:
                name = Enum.GetName(typeof(Define.TileObject), btnInfo.TileName);
                RoadBuilder.GetInstance.Target = Managers.Tile.Load(name);
                break;

            case Define.BuildType.Building:
                name = Enum.GetName(typeof(Define.TileObject), btnInfo.TileName);
                BuildingBuilder.GetInstance.Target = Managers.Tile.Load(name);
                break;
        }
    }
}
