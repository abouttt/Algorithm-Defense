using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject citizenButtenMenu;
    [SerializeField]
    private GameObject buildTileMenu;



    public void GetCitizenButtonDown()
    {

        if (citizenButtenMenu.activeSelf)
        {
            //��ü�ݱ�
            citizenButtenMenu.SetActive(false);
          
        }
        else//�ƴϸ�
        {
            buildTileMenu.SetActive(false);
            citizenButtenMenu.SetActive(true);
        
        }


    }



    public void GetTileButtonDown()
    {
        if (buildTileMenu.activeSelf)
        {
            //��ü�ݱ�
            buildTileMenu.SetActive(false);   

        }
        else//�ƴϸ�
        {
            buildTileMenu.SetActive(true);
            citizenButtenMenu.SetActive(false);       

        }
    }

}
