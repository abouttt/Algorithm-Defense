using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UI_SubMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject MainMenuSet;
    [SerializeField]
    private GameObject SubMenuSet;
    [SerializeField]
    private GameObject EarlySet;
    [SerializeField]
    private GameObject SattingSet;
    [SerializeField]
    private GameObject Resolution;
    [SerializeField]
    private GameObject Sound;


    void Start()
    {
        SubMenuSet.SetActive(false);
        MainMenuSet.SetActive(true);
    }

    void Update()
    {
        // 메뉴창
        if (Input.GetButtonDown("Cancel"))//esc버튼 클릭
        {
            EarlySet.SetActive(true);       //초기화면 키기
            //SattingSet.SetActive(false);    //설정화면 끄기
            //Resolution.SetActive(false);    //해상도화면 끄기
            Sound.SetActive(false);         //소리화면 끄기

            //이미 메뉴가 열린 상태라면
            if (SubMenuSet.activeSelf)
            {
                //전체닫기
                SubMenuSet.SetActive(false);
                Time.timeScale = 1f;                
                //MainMenuSet.SetActive(true);
            }
            else//아니면
            {
                //열기
                SubMenuSet.SetActive(true);
                Time.timeScale = 0f;
                //MainMenuSet.SetActive(false);
            }

        }

    }

    public void SubMenuOpen()
    {
        //Game씬(1번)다시 시작
        Time.timeScale = 0f;
        SubMenuSet.SetActive(true);

        EarlySet.SetActive(true);       //초기화면 키기    
        Sound.SetActive(false);         //소리화면 끄기
    }

    public void ContinueStage()
    {
        //Game씬(1번)다시 시작
        Time.timeScale = 1f;
       
        SubMenuSet.SetActive(false);
    }


    //다시하기
    public void NowStageAgain()
    {
        //Game씬(1번)다시 시작
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
        Managers.Clear();
    }

    public void BackStartScene()
    {
        //시작화면으로 이동
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        Managers.Clear();
    }




    ////종료버튼
    //public void GameExit()
    //{
    //    //종료
    //    Application.Quit();
    //}
}
