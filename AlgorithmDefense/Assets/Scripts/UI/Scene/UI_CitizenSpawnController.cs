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
        for (int i = 0; i < BUTTON_NUM; i++)
        {
            if (Managers.Game.CitizenSpawnList[i].Item1 == citizenBtn.CitizenType)
            {
                Managers.Game.CitizenSpawnList[i].Item2 = !Managers.Game.CitizenSpawnList[i].Item2;
                CitizenSpawner.GetInstance.OnNum = Managers.Game.CitizenSpawnList[i].Item2 ?
                    CitizenSpawner.GetInstance.OnNum + 1 : CitizenSpawner.GetInstance.OnNum - 1;
                updateText(citizenBtn.CitizenType);
            }
        }

        if (!CitizenSpawner.GetInstance.IsSpawning)
        {
            StartCoroutine(CitizenSpawner.GetInstance.SpawnCitizen());
        }

        updateAllTexts();
    }

    private void updateAllTexts()
    {
        for (int i = 0; i < BUTTON_NUM; i++)
        {
            _buttonTexts[i].text = Managers.Game.CitizenSpawnList[i].Item2 ? "ON" : "OFF";
        }
    }

    private void updateText(Define.Citizen citizenType)
    {
        for (int i = 0; i < BUTTON_NUM; i++)
        {
            if (Managers.Game.CitizenSpawnList[i].Item1 == citizenType)
            {
                _buttonTexts[i].text = Managers.Game.CitizenSpawnList[i].Item2 ? "ON" : "OFF";
                break;
            }
        }
    }
}
