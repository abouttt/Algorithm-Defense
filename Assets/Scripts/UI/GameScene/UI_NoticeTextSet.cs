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


    public void LackOfGold()    //골드 부족 오류
    {
        NoticeText.text = "골드가 부족합니다.";
        NoticeText.DOFade(1f, 0f);
        NoticeText.DOFade(0f, 3f);
    }

    public void SkillCooldown()     //스킬 쿨타임 오류
    {
        NoticeText.text = "스킬이 사용중에 있습니다.";
        NoticeText.DOFade(1f, 0f);
        NoticeText.DOFade(0f, 3f);
    }

    public void FailedRemoveRoad()
    {
        NoticeText.text = "시민이 있어 삭제할 수 없습니다.";
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
