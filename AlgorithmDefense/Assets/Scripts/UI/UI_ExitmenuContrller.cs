using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ExitmenuContrller : MonoBehaviour
{
    //[SerializeField]
    //private GameObject mainMenu;
    [SerializeField]
    private GameObject stageMenu;
    [SerializeField]
    private GameObject SubMenu;
    [SerializeField]
    private GameObject ExitMenu;



    // Start is called before the first frame update
    void Start()
    {
        ExitMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // 메뉴창
        if (Input.GetButtonDown("Cancel"))//esc버튼 클릭
        {
           
            //이미 메뉴가 열린 상태라면
            if (ExitMenu.activeSelf)
            {
                ExitMenu.SetActive(false);       //초기화면 키기
             
            }
            else//아니면
            {
                ExitMenu.SetActive(true);       //초기화면 키기
          
            }

        }

    }


    public void ExitButtonClick()
    {
     
            ExitMenu.SetActive(true);       //초기화면 키기

    }


    public void GameAgain()
    {
        ExitMenu.SetActive(false);       //초기화면 키기
      
    }


    //종료버튼
    public void GameExit()
    {
        //종료
        Application.Quit();
    }




}
