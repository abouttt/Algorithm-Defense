using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UI_StartSceneSubMenuController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup subMenuCanvasGroup;
    [SerializeField]
    private RectTransform subMenuRectTransform;
    [SerializeField]
    private CanvasGroup mainMenuCanvasGroup;


    public void SubMenuFadeIn()
    {
        // Debug.Log("���۹�ư Ŭ����");
        //���� alpha 0


        mainMenuCanvasGroup.alpha = 1f;
        mainMenuCanvasGroup.DOFade(0, 0.3f);


        subMenuCanvasGroup.alpha = 0f;
        subMenuRectTransform.transform.localPosition = new Vector3(-1100f, 0f, 0f);


        //���̵� �� ������ �ӵ�
        subMenuRectTransform.DOAnchorPos(new Vector2(0f, 0f), 0.7f, false).SetEase(Ease.OutBack);

        subMenuCanvasGroup.DOFade(1, 0.7f);


    }


    public void SubMenuFadeOut()
    {

        mainMenuCanvasGroup.alpha = 0f;
        mainMenuCanvasGroup.DOFade(1, 0.3f);

        subMenuCanvasGroup.alpha = 1f;
        subMenuRectTransform.transform.localPosition = new Vector3(0f, 0f, 0f);
        subMenuRectTransform.DOAnchorPos(new Vector2(-1100f, 0f), 1f, false).SetEase(Ease.InOutQuint);
        subMenuCanvasGroup.DOFade(0, 0.7f);



    }




    //�����ư
    public void GameExit()
    {
        //����
        Application.Quit();
    }



}
