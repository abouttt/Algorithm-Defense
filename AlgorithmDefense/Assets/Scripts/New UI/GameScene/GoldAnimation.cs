using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GoldAnimation : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI goldText;
    [SerializeField]
    private RectTransform goldTextTransform;

    private void Start()
    {
        goldText.text = Managers.Game.Gold.ToString();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {

            GoldSaving(10);

        }

        if (Input.GetKeyDown(KeyCode.E))
        {

            GoldExpenditure(10);

        }


    }


    public void GoldSaving(int price)
    {
        Managers.Game.Gold += price;
        goldText.text = Managers.Game.Gold.ToString();

        goldTextTransform.DOScale(1.2f, 0.2f);
        goldText.DOColor(Color.yellow, 0.3f).OnComplete(() =>
        {
            goldText.DOColor(Color.white, 0.3f);
            goldTextTransform.DOScale(1f, 0.2f);
        });
    }


    public void GoldExpenditure(int price)
    {
        //�������� ����ϰ� 0���� �ƴҶ�
        if (Managers.Game.Gold >= price && Managers.Game.Gold > 0)
        {
            Managers.Game.Gold -= price;
            goldText.text = Managers.Game.Gold.ToString();

            goldTextTransform.DOScale(0.85f, 0.2f);
            goldText.DOColor(Color.red, 0.3f).OnComplete(() =>
            {
                goldText.DOColor(Color.white, 0.3f);
                goldTextTransform.DOScale(1f, 0.2f);
            });

        }
        else//���� ������ ��
        {
            goldTextTransform.DOShakePosition(0.2f, 5, 20, 90, true, false);
            goldText.DOColor(Color.red, 0.3f).OnComplete(() =>
            {
                goldText.DOColor(Color.white, 0.3f);
                goldTextTransform.localPosition = new Vector3(-20f, 0f, 0f);
            });


        }


    }


}
