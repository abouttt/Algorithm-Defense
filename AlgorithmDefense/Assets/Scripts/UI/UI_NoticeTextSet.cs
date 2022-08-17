using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_NoticeTextSet : MonoBehaviour
{
    private GameObject NoticeMenu;
    private Text NoticeText;
    private Color Alpha;
    private float _alphaSpeed;
    private bool _showText;
    private int _msg;


    void Start()
    {
        // object가 비활성화 되있을 때 찾는 법

        //해당 오브젝트의 부모를 찾아서 찾기
        GameObject obj = GameObject.Find("Notice");
        //Debug.Log(obj);

        //비활성화 object
        NoticeMenu = obj.transform.Find("NoticeText").gameObject;
        //Debug.Log(noticeMenu);

        NoticeText = NoticeMenu.GetComponent<Text>();
        //Debug.Log(noticeText);

        Alpha = NoticeText.color;
        _showText = false;
        _alphaSpeed = 2f;
    }



    void Update()
    {

        if (_showText)
        {
            Alpha.a = Mathf.Lerp(Alpha.a, 0, Time.deltaTime / _alphaSpeed);
            NoticeText.color = Alpha;
            //Debug.Log(alpha.a);
            if (NoticeText.color.a < 0.25f)
            {
                _showText = false;
                NoticeText.text = _msg.ToString("Null Text");
                NoticeMenu.SetActive(false);
            }

        }

    }



    //건물을 설치 할 수 없는 곳을 클릭했을 때
    public void CanNotBeBuilt()
    {
        _showText = true;
        Alpha.a = 1f;
        NoticeText.text = _msg.ToString("설치를 할 수 없는 위치입니다'.'");
        NoticeMenu.SetActive(true);

    }
}
