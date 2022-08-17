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
        // object�� ��Ȱ��ȭ ������ �� ã�� ��

        //�ش� ������Ʈ�� �θ� ã�Ƽ� ã��
        GameObject obj = GameObject.Find("Notice");
        //Debug.Log(obj);

        //��Ȱ��ȭ object
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



    //�ǹ��� ��ġ �� �� ���� ���� Ŭ������ ��
    public void CanNotBeBuilt()
    {
        _showText = true;
        Alpha.a = 1f;
        NoticeText.text = _msg.ToString("��ġ�� �� �� ���� ��ġ�Դϴ�'.'");
        NoticeMenu.SetActive(true);

    }
}
