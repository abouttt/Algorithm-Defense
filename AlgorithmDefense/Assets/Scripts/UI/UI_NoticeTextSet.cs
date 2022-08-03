using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_NoticeTextSet : MonoBehaviour
{
    private GameObject noticeMenu;
    Text noticeText;
    public Color alpha;
    public float alphaSpeed;
    public bool showText;
    public int msg;


    void Start()
    {
        // object가 비활성화 되있을 때 찾는 법

        //해당 오브젝트의 부모를 찾아서 찾기
        GameObject obj = GameObject.Find("Notice");
        //Debug.Log(obj);

        //비활성화 object
        noticeMenu = obj.transform.Find("Notice_Text").gameObject;
        //Debug.Log(noticeMenu);

        noticeText = noticeMenu.GetComponent<Text>();
        //Debug.Log(noticeText);

        alpha = noticeText.color;
        showText = false;
        alphaSpeed = 1.5f;
    }



    //건물을 설치 할 수 없는 곳을 클릭했을 때
    public void CannotBeBuilt()
    {
        showText = true;
        alpha.a = 1f;
        noticeText.text = msg.ToString("설치를 할 수 없는 위치입니다'.'");
        noticeMenu.SetActive(true);

    }



    void Update()
    {

        if (showText)
        {
            alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime / alphaSpeed);
            noticeText.color = alpha;
            //Debug.Log(alpha.a);
            if (noticeText.color.a < 0.09f)
            {
                showText = false;
                noticeText.text = msg.ToString(" ");
                noticeMenu.SetActive(false);
            }

        }

    }
}
