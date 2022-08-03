using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_StartSubMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject MainMenuSet;
    [SerializeField]
    private GameObject SubMenuSet;
    [SerializeField]
    private GameObject SattingSet;
    [SerializeField]
    private GameObject Resolution;
    [SerializeField]
    private GameObject Sound;

    [SerializeField]
    private Button ExitButton;


    void Start()
    {
        SubMenuSet.SetActive(false);
        MainMenuSet.SetActive(true);

        ExitButton.onClick.AddListener(GameExit);
    }

    void Update()
    {
        // 메뉴창
        if (Input.GetButtonDown("Cancel"))//esc버튼 클릭
        {
            SattingSet.SetActive(true);    //설정화면 끄기
            Resolution.SetActive(false);    //해상도화면 끄기
            Sound.SetActive(false);         //소리화면 끄기

            //이미 메뉴가 열린 상태라면
            if (SubMenuSet.activeSelf)
            {
                //전체닫기
                SubMenuSet.SetActive(false);
                MainMenuSet.SetActive(true);
            }
            else//아니면
            {
                //열기
                SubMenuSet.SetActive(true);
                MainMenuSet.SetActive(false);
            }

        }

    }

    //종료버튼
    public void GameExit()
    {
        //종료
        Application.Quit();
    }



}
