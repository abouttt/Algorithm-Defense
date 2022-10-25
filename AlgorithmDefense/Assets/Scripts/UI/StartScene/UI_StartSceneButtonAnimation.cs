using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class UI_StartSceneButtonAnimation : MonoBehaviour
{
    [SerializeField]
    private Button gameStartButton;
    [SerializeField]
    private Text gameStartText;
    [SerializeField]
    private Button settingButton;
    [SerializeField]
    private Text settingText;

    [SerializeField]
    private Button exitButton;
    [SerializeField]
    private Text exitText;


    public void StartButtonFadeIn()
    {

        //gameStartButton.transform.GetComponent<Image>().DOFade(1, 0.2f);
        gameStartText.transform.GetComponent<Text>().DOColor(Color.black, 0.4f);
    }

    public void StartButtonFadeOut()
    {

        //gameStartButton.transform.GetComponent<Image>().DOFade(0, 0.2f);
        gameStartText.transform.GetComponent<Text>().DOColor(Color.white, 0.4f);
    }


    public void SettingButtonFadeIn()
    {

        //settingButton.transform.GetComponent<Image>().DOFade(1, 0.2f);
        settingText.transform.GetComponent<Text>().DOColor(Color.black, 0.4f);
    }

    public void SettingButtonFadeOut()
    {

        //settingButton.transform.GetComponent<Image>().DOFade(0, 0.2f);
        settingText.transform.GetComponent<Text>().DOColor(Color.white, 0.4f);
    }


    public void ExitButtonFadeIn()
    {

        //exitButton.transform.GetComponent<Image>().DOFade(1, 0.2f);
        exitText.transform.GetComponent<Text>().DOColor(Color.black, 0.4f);
    }

    public void ExitButtonFadeOut()
    {

        //exitButton.transform.GetComponent<Image>().DOFade(0, 0.2f);
        exitText.transform.GetComponent<Text>().DOColor(Color.white, 0.4f);
    }

}
