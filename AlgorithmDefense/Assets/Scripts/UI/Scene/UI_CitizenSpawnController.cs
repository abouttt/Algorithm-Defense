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

    public static readonly int BUTTON_NUM = 4;

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
        if (Managers.Game.CitizenSpawnOrderList.Contains(citizenBtn.Citizen))
        {
            Managers.Game.CitizenSpawnOrderList.Remove(citizenBtn.Citizen);
        }
        else
        {
            Managers.Game.CitizenSpawnOrderList.Add(citizenBtn.Citizen);
        }

        if (!CitizenSpawner.GetInstance.IsSpawning)
        {
            StartCoroutine(CitizenSpawner.GetInstance.SpawnCitizen());
        }

        updateAllTexts();
    }

    private void updateAllTexts()
    {
        int idx;
        foreach (var tmpro in _buttonTexts)
        {
            var citizenBtn = tmpro.transform.parent.GetComponent<UI_CitizenSpawnButton>();
            idx = Managers.Game.CitizenSpawnOrderList.IndexOf(citizenBtn.Citizen);
            tmpro.text = idx >= 0 ? (idx + 1).ToString() : "";
        }
    }
}
