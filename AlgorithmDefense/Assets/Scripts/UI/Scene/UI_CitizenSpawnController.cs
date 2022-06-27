using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_CitizenSpawnController : UI_Base
{
    #region UI_Objects
    enum Buttons
    {
        RedButton,
        BlueButton,
        GreenButton,
        YellowButton,
    }

    enum Texts
    {
        RedTurnNumberText,
        BlueTurnNumberText,
        GreenTurnNumberText,
        YellowTurnNumberText,
    }
    #endregion

    public const int BUTTON_NUM = 4;

    private List<TextMeshProUGUI> _buttonTexts;

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));

        _buttonTexts = new List<TextMeshProUGUI>(BUTTON_NUM);
        _buttonTexts.AddRange(GetAll<TextMeshProUGUI>());

        var buttons = GetAll<Button>();
        foreach (var button in buttons)
        {
            BindEvent(button.gameObject, onButtonSpawnCitizen, Define.UIEvent.Click);
        }

        updateAllTexts();
    }

    private void onButtonSpawnCitizen(PointerEventData data)
    {
        var citizenBtn = EventSystem.current.currentSelectedGameObject.GetComponent<UI_CitizenSpawnButton>();
        CitizenSpawner.GetInstance.SetOnOff(citizenBtn.CitizenType);
        updateAllTexts();
    }

    private void updateAllTexts()
    {
        for (int i = 0; i < BUTTON_NUM; i++)
        {
            _buttonTexts[i].text = CitizenSpawner.GetInstance.CitizenSpawnList[i].Item2 ? "ON" : "OFF";
        }
    }
}
