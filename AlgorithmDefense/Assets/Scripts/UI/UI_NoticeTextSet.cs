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
        // object�� ��Ȱ��ȭ ������ �� ã�� ��

        //�ش� ������Ʈ�� �θ� ã�Ƽ� ã��
        GameObject obj = GameObject.Find("Notice");
        //Debug.Log(obj);

        //��Ȱ��ȭ object
        noticeMenu = obj.transform.Find("Notice_Text").gameObject;
        //Debug.Log(noticeMenu);

        noticeText = noticeMenu.GetComponent<Text>();
        //Debug.Log(noticeText);

        alpha = noticeText.color;
        showText = false;
        alphaSpeed = 1.5f;
    }



    //�ǹ��� ��ġ �� �� ���� ���� Ŭ������ ��
    public void CannotBeBuilt()
    {
        showText = true;
        alpha.a = 1f;
        noticeText.text = msg.ToString("��ġ�� �� �� ���� ��ġ�Դϴ�'.'");
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
