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
        // �޴�â
        if (Input.GetButtonDown("Cancel"))//esc��ư Ŭ��
        {
           
            //�̹� �޴��� ���� ���¶��
            if (ExitMenu.activeSelf)
            {
                ExitMenu.SetActive(false);       //�ʱ�ȭ�� Ű��
             
            }
            else//�ƴϸ�
            {
                ExitMenu.SetActive(true);       //�ʱ�ȭ�� Ű��
          
            }

        }

    }


    public void ExitButtonClick()
    {
     
            ExitMenu.SetActive(true);       //�ʱ�ȭ�� Ű��

    }


    public void GameAgain()
    {
        ExitMenu.SetActive(false);       //�ʱ�ȭ�� Ű��
      
    }


    //�����ư
    public void GameExit()
    {
        //����
        Application.Quit();
    }




}
