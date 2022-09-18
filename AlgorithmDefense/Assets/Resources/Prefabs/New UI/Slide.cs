using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class Slide : MonoBehaviour
{
    public RectTransform rectTransform;


    public void OpenClike()
    {
        rectTransform.transform.localPosition = new Vector3(-650f, 0f, 0f);

        rectTransform.DOAnchorPos(new Vector2(0f, 0f), 0.7f, false).SetEase(Ease.InOutQuint);

    }

    public void ClouseClike()
    {
        rectTransform.transform.localPosition = new Vector3(0f, 0f, 0f);

        rectTransform.DOAnchorPos(new Vector2(-650f, 0f), 0.5f, false).SetEase(Ease.InOutQuint);

    }



}
