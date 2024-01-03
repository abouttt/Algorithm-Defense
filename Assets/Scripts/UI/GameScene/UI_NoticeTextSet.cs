using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using DG.Tweening;


public class UI_NoticeTextSet : MonoBehaviour
{
    private static UI_NoticeTextSet s_instance;
    public static UI_NoticeTextSet GetInstance { get { Init(); return s_instance; } }

    [SerializeField]
    private TextMeshProUGUI NoticeText;


    private void Start()
    {
        NoticeText.DOFade(0f, 0f);
    }


    public void LackOfGold()    //��� ���� ����
    {
        NoticeText.text = "��尡 �����մϴ�.";
        NoticeText.DOFade(1f, 0f);
        NoticeText.DOFade(0f, 3f);
    }

    public void SkillCooldown()     //��ų ��Ÿ�� ����
    {
        NoticeText.text = "��ų�� ����߿� �ֽ��ϴ�.";
        NoticeText.DOFade(1f, 0f);
        NoticeText.DOFade(0f, 3f);
    }

    public void FailedRemoveRoad()
    {
        NoticeText.text = "�ù��� �־� ������ �� �����ϴ�.";
        NoticeText.DOFade(1f, 0f);
        NoticeText.DOFade(0f, 3f);
    }

    private static void Init()
    {
        if (!s_instance)
        {
            var go = GameObject.Find("Notice");
            if (!go)
            {
                go = new GameObject { name = "Notice" };
                go.AddComponent<UI_NoticeTextSet>();
            }

            s_instance = go.GetComponent<UI_NoticeTextSet>();
        }
    }


}
