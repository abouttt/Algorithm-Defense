using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_BuildingButtons : UI_Base
{
    #region UI_Objects
    enum Buttons
    {
        GatewayButton,
    }
    #endregion

    private Button[] _buttons = null;

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));

        _buttons = GetAll<Button>();

        setBuildingButtons();

        foreach (var button in _buttons)
        {
            BindEvent(button.gameObject, onButtonClick, Define.UIEvent.Click);
        }
    }

    private void onButtonClick(PointerEventData data)
    {
        var go = EventSystem.current.currentSelectedGameObject;
        var type = go.GetComponent<UI_BuildingButton>().Building;
        var name = Enum.GetName(typeof(Define.Building), type);
        var tile = Managers.Resource.Load<Tile>($"Tiles/{name}");
        BuildingBuilder.GetInstance.Target = tile;
    }

    private void setBuildingButtons()
    {
        var values = Enum.GetValues(typeof(Define.Building));

        int currentIdx = 0;
        while (currentIdx < _buttons.Length)
        {
            _buttons[currentIdx].GetComponent<UI_BuildingButton>().Building = (Define.Building)values.GetValue(currentIdx + 1);
            currentIdx++;
        }
    }
}
