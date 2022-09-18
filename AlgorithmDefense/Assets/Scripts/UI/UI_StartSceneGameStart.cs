using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class UI_StartSceneGameStart : MonoBehaviour
{
    [SerializeField]
    private Button gameStartButton;
    [SerializeField]
    private Text gameStartText;
    [SerializeField]
    private Button settingButton;
    [SerializeField]
    private Text settingText;


    public void StartButtonSinIn()
    {

        gameStartButton.transform.GetComponent<Image>().DOFade(1, 0.2f);
        gameStartText.transform.GetComponent<Text>().DOColor(Color.black, 0.4f);
    }

    public void StartButtonSinOut()
    {

        gameStartButton.transform.GetComponent<Image>().DOFade(0, 0.2f);
        gameStartText.transform.GetComponent<Text>().DOColor(Color.white, 0.4f);
    }


    public void SettingButtonSinIn()
    {

        settingButton.transform.GetComponent<Image>().DOFade(1, 0.2f);
        settingText.transform.GetComponent<Text>().DOColor(Color.black, 0.4f);
    }

    public void SettingButtonSinOut()
    {

        settingButton.transform.GetComponent<Image>().DOFade(0, 0.2f);
        settingText.transform.GetComponent<Text>().DOColor(Color.white, 0.4f);
    }

}
