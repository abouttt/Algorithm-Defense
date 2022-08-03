using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            SattingSet.SetActive(false);    //����ȭ�� ����
            Resolution.SetActive(false);    //�ػ�ȭ�� ����
            Sound.SetActive(false);         //�Ҹ�ȭ�� ����

            //�̹� �޴��� ���� ���¶��
            if (SubMenuSet.activeSelf)
            {
                //��ü�ݱ�
                SubMenuSet.SetActive(false);
                MainMenuSet.SetActive(true);
            }
            else//�ƴϸ�
            {
                //����
                SubMenuSet.SetActive(true);
                MainMenuSet.SetActive(false);
            }

        }

    }

    //�����ư
    public void GameExit()
    {
        //����
        Application.Quit();
    }
}
