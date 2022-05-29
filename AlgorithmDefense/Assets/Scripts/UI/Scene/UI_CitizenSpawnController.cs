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
        _buttonTexts = new List<TextMeshProUGUI>(BUTTON_NUM);

        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));

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
        var go = EventSystem.current.currentSelectedGameObject;
        if (Managers.Game.CitizenSpawnOrderList.Contains(go))
        {
            Managers.Game.CitizenSpawnOrderList.Remove(go);
        }
        else
        {
            Managers.Game.CitizenSpawnOrderList.Add(go);
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
            idx = Managers.Game.CitizenSpawnOrderList.IndexOf(tmpro.transform.parent.gameObject);
            tmpro.text = idx >= 0 ? (idx + 1).ToString() : "";
        }
    }
}
