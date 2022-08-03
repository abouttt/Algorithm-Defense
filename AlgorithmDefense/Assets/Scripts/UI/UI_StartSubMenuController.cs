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
        // �޴�â
        if (Input.GetButtonDown("Cancel"))//esc��ư Ŭ��
        {
            SattingSet.SetActive(true);    //����ȭ�� ����
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
