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
        // �޴�â
        if (Input.GetButtonDown("Cancel"))//esc��ư Ŭ��
        {
            EarlySet.SetActive(true);       //�ʱ�ȭ�� Ű��
            //SattingSet.SetActive(false);    //����ȭ�� ����
            //Resolution.SetActive(false);    //�ػ�ȭ�� ����
            Sound.SetActive(false);         //�Ҹ�ȭ�� ����

            //�̹� �޴��� ���� ���¶��
            if (SubMenuSet.activeSelf)
            {
                //��ü�ݱ�
                SubMenuSet.SetActive(false);
                Time.timeScale = 1f;                
                //MainMenuSet.SetActive(true);
            }
            else//�ƴϸ�
            {
                //����
                SubMenuSet.SetActive(true);
                Time.timeScale = 0f;
                //MainMenuSet.SetActive(false);
            }

        }

    }

    public void SubMenuOpen()
    {
        //Game��(1��)�ٽ� ����
        Time.timeScale = 0f;
        SubMenuSet.SetActive(true);

        EarlySet.SetActive(true);       //�ʱ�ȭ�� Ű��    
        Sound.SetActive(false);         //�Ҹ�ȭ�� ����
    }

    public void ContinueStage()
    {
        //Game��(1��)�ٽ� ����
        Time.timeScale = 1f;
       
        SubMenuSet.SetActive(false);
    }


    //�ٽ��ϱ�
    public void NowStageAgain()
    {
        //Game��(1��)�ٽ� ����
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
        Managers.Clear();
    }

    public void BackStartScene()
    {
        //����ȭ������ �̵�
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        Managers.Clear();
    }




    ////�����ư
    //public void GameExit()
    //{
    //    //����
    //    Application.Quit();
    //}
}
