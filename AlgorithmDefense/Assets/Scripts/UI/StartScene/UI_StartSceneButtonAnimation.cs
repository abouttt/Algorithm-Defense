using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UI_StartSceneButtonAnimation : MonoBehaviour
{

    private static UI_StartSceneButtonAnimation s_instance;
    public static UI_StartSceneButtonAnimation GetInstance { get { Init(); return s_instance; } }

    [SerializeField]
    private Button gameStartButton;
    [SerializeField]
    private TextMeshProUGUI gameStartText;
    [SerializeField]
    private Button tutorialButton;
    [SerializeField]
    private TextMeshProUGUI tutorialText;
    [SerializeField]
    private Button settingButton;
    [SerializeField]
    private TextMeshProUGUI settingText;
    [SerializeField]
    private Button exitButton;
    [SerializeField]
    private TextMeshProUGUI exitText;


    public void SetButtonsOff()
    {
        gameStartButton.interactable = false;
        tutorialButton.interactable = false;
        settingButton.interactable = false;
        exitButton.interactable = false;
    }

    public void SetButtonsOn()
    {
        gameStartButton.interactable = true;
        tutorialButton.interactable = true;
        settingButton.interactable = true;
        exitButton.interactable = true;
    }

    public void StartButtonFadeIn()
    {

        //gameStartButton.transform.GetComponent<Image>().DOFade(1, 0.2f);
        gameStartText.transform.GetComponent<TextMeshProUGUI>().DOColor(Color.white, 0.4f);
    }

    public void TutorialButtonFadeOut()
    {

        //tutorialButton.transform.GetComponent<Image>().DOFade(0, 0.2f);
        tutorialText.transform.GetComponent<TextMeshProUGUI>().DOColor(new Color(50f / 255f, 4f / 255f, 16f / 255f), 0.4f);
    }

    public void TutorialButtonFadeIn()
    {

        //tutorialButton.transform.GetComponent<Image>().DOFade(1, 0.2f);
        tutorialText.transform.GetComponent<TextMeshProUGUI>().DOColor(Color.white, 0.4f);
    }

    public void StartButtonFadeOut()
    {

        //gameStartButton.transform.GetComponent<Image>().DOFade(0, 0.2f);
        gameStartText.transform.GetComponent<TextMeshProUGUI>().DOColor(new Color(50f / 255f, 4f / 255f, 16f / 255f), 0.4f);
    }


    public void SettingButtonFadeIn()
    {

        //settingButton.transform.GetComponent<Image>().DOFade(1, 0.2f);
        settingText.transform.GetComponent<TextMeshProUGUI>().DOColor(Color.white, 0.4f);
    }

    public void SettingButtonFadeOut()
    {

        //settingButton.transform.GetComponent<Image>().DOFade(0, 0.2f);
        settingText.transform.GetComponent<TextMeshProUGUI>().DOColor(new Color(50f / 255f, 4f / 255f, 16f / 255f), 0.4f);
    }


    public void ExitButtonFadeIn()
    {

        //exitButton.transform.GetComponent<Image>().DOFade(1, 0.2f);
        exitText.transform.GetComponent<TextMeshProUGUI>().DOColor(Color.white, 0.4f);
    }

    public void ExitButtonFadeOut()
    {

        //exitButton.transform.GetComponent<Image>().DOFade(0, 0.2f);
        exitText.transform.GetComponent<TextMeshProUGUI>().DOColor(new Color(50f / 255f, 4f / 255f, 16f / 255f), 0.4f);
    }



    private static void Init()
    {
        if (!s_instance)
        {
            var go = GameObject.Find("MenuManager");
            if (!go)
            {
                go = new GameObject { name = "MenuManager" };
                go.AddComponent<UI_StartSceneButtonAnimation>();
            }

            s_instance = go.GetComponent<UI_StartSceneButtonAnimation>();
        }
    }


}
