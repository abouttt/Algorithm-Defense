using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_CitizenDirectionController : UI_Popup
{
    #region UI_Object
    enum Buttons
    {
        OKButton,
    }

    enum ToggleGroups
    {
        RedToggleGroup,
        BlueToggleGroup,
        GreenToggleGroup,
        YellowToggleGroup,
    }
    #endregion

    public Dictionary<Define.Citizen, Define.MoveType> Target { get; set; }

    private ToggleGroup[] _toggleGroups;
    private Dictionary<Define.MoveType, int> _moveTypeCntDic;

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<ToggleGroup>(typeof(ToggleGroups));

        BindEvent(GetButton((int)Buttons.OKButton).gameObject, onButtonOK, Define.UIEvent.Click);
        _toggleGroups = GetAll<ToggleGroup>();

        _moveTypeCntDic = new Dictionary<Define.MoveType, int>()
        {
            { Define.MoveType.Up, 0 },
            { Define.MoveType.Down, 0 },
            { Define.MoveType.Left, 0 },
            { Define.MoveType.Right, 0 },
        };

        setupGatewayInfo();
    }

    private void onButtonOK(PointerEventData data)
    {
        if (IsMoveTypeCntOver())
        {
            Managers.UI.CloseAllPopupUI();
            return;
        }

        for (int citizenIdx = 1; citizenIdx < Target.Count; citizenIdx++)
        {
            Target[(Define.Citizen)citizenIdx] = Define.MoveType.None;
        }

        foreach (var toggles in _toggleGroups)
        {
            var toggle = toggles.GetFirstActiveToggle();
            if (toggle != null)
            {
                var info = toggle.GetComponent<UI_DirectionToggle>();
                Target[info.CitizenType] = info.MoveType;
            }
        }

        Managers.UI.CloseAllPopupUI();
    }

    private void setupGatewayInfo()
    {
        foreach (var condition in Target)
        {
            if (condition.Value != Define.MoveType.None)
            {
                var toggle = findToggle(condition.Key, condition.Value);
                toggle.isOn = true;
            }
        }
    }

    private Toggle findToggle(Define.Citizen citizenType, Define.MoveType moveType)
    {
        foreach (var toggles in _toggleGroups)
        {
            if (toggles.name.Contains(Enum.GetName(typeof(Define.Citizen), citizenType)))
            {
                return toggles.GetComponentsInChildren<UI_DirectionToggle>()
                              .First(toggle => toggle.MoveType == moveType)
                              .GetComponent<Toggle>();
            }
        }

        return null;
    }

    private bool IsMoveTypeCntOver()
    {
        foreach (var toggles in _toggleGroups)
        {
            var toggle = toggles.GetFirstActiveToggle();
            if (toggle != null)
            {
                var info = toggle.GetComponent<UI_DirectionToggle>();
                _moveTypeCntDic[info.MoveType]++;
            }
        }

        if (_moveTypeCntDic.Any(dic => dic.Value >= 4))
        {
            return true;
        }

        return false;
    }
}