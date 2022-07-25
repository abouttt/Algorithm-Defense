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
            //전체닫기
            citizenButtenMenu.SetActive(false);
          
        }
        else//아니면
        {
            buildTileMenu.SetActive(false);
            citizenButtenMenu.SetActive(true);
        
        }


    }



    public void GetTileButtonDown()
    {
        if (buildTileMenu.activeSelf)
        {
            //전체닫기
            buildTileMenu.SetActive(false);   

        }
        else//아니면
        {
            buildTileMenu.SetActive(true);
            citizenButtenMenu.SetActive(false);       

        }
    }

}
