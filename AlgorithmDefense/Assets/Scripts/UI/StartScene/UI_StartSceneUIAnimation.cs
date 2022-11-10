using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UI_StartSceneUIAnimation : MonoBehaviour
{


    private static UI_StartSceneUIAnimation s_instance;
    public static UI_StartSceneUIAnimation GetInstance { get { Init(); return s_instance; } }

    [SerializeField]
    private CanvasGroup stageCanvasGroup;
    [SerializeField]
    private RectTransform stageRectTransform;
    [SerializeField]
    private CanvasGroup mainMenuCanvasGroup;
    [SerializeField]
    private CanvasGroup blackCanvasGroup;
    [SerializeField]
    private GameObject defaultStageUIBar;
    [SerializeField]
    private RectTransform stageButtonContainer;
    [SerializeField]
    private RectTransform endCreditsTransform;

    private int _easterEggCount = 0;

    [System.Serializable]
    public class StageUIBarInformation
    {
        public int stageNum;      //�� ����
        public int starCount;      //�� ����
        public bool open;          //�������� ���� ����   
        [HideInInspector]
        public Text stageNumText;        //�������� �ؽ�Ʈ
        [HideInInspector]
        public Image[] starImages;      //�������� �ؽ�Ʈ
        [HideInInspector]
        public Button stageButton;   //������ ��ư ������Ʈ ����
        [HideInInspector]
        public GameObject stageUIBarObj;   //������ ��ư ������Ʈ ����

    }
    public StageUIBarInformation[] StageUIBar;

    [SerializeField]
    private Sprite openImage;
    [SerializeField]
    private Sprite closeImage;
    [SerializeField]
    private Sprite starGetImage;
    [SerializeField]
    private Sprite starFailImage;


    private void Start()
    {
        //CreateStageUIBar(stageButtonContainer, defaultStageUIBar, StageUIBar);
    }


    //�����͸� ����ȯ������ �� ȣ��
    public void SetStage()
    {
        //���� �������� ���� ����
        PlayerPrefs.SetInt("StageCount", StageUIBar.Length);

        CreateStageUIBar(stageButtonContainer, defaultStageUIBar, StageUIBar);
        LoadingControl.GetInstance.LoadingComplete();
    }


    public void CreateStageUIBar(RectTransform _container, GameObject _stageUIBar, StageUIBarInformation[] _dataSlot)
    {
        int y_count = 0;

        //��ũ�� �� ũ�� ����
        //_container.sizeDelta.y(���� �⺻ ũ��400f) + 300f(��ưũ��)*(��ư����-1(�迭-1))
        _container.sizeDelta = new Vector2(0f, _container.sizeDelta.y + (300f * (StageUIBar.Length - 1)));


        for (int i = 0; i < _dataSlot.Length; i++)
        {
            // _dataSlot[i] = new StageUIBarInformation();

            int index = i;

            //��ư ����
            GameObject newUIObj = Instantiate(_stageUIBar, _container.transform);
            _dataSlot[i].stageUIBarObj = newUIObj;



            if (i != 0 && (i % 5) == 0)
            {
                y_count++;
            }

            //newButton.transform.localPosition = new Vector3(newButton.transform.localPosition.x + (150f * i)-(750f* y_count), newButton.transform.localPosition.y-(150f* y_count), newButton.transform.localPosition.z);
            // newButton.transform.localPosition = new Vector3(newButton.transform.localPosition.x + (300f * i)-(750f* y_count), newButton.transform.localPosition.y-(150f* y_count), newButton.transform.localPosition.z);
            newUIObj.transform.localPosition = new Vector3(newUIObj.transform.localPosition.x, newUIObj.transform.localPosition.y - 300f * i, newUIObj.transform.localPosition.z);

            GameObject newButton = newUIObj.transform.Find("StageButton").gameObject;
            _dataSlot[i].stageButton = newButton.GetComponent<Button>();

            if (_dataSlot[i].open == true)
            {
                newButton.GetComponent<Image>().sprite = openImage;
            }
            else
            {
                newButton.GetComponent<Image>().sprite = closeImage;
            }


            GameObject stageTextBar = newUIObj.transform.Find("StageTextBar").gameObject;
            GameObject stageText = stageTextBar.transform.Find("Text").gameObject;
            _dataSlot[i].stageNumText = stageText.GetComponent<Text>();

            string conversion = "STAGE " + (index + 1).ToString();
            _dataSlot[i].stageNumText.text = conversion;


            GameObject starBar = newUIObj.transform.Find("StarBar").gameObject;
            GameObject starImage1 = starBar.transform.Find("Star1").gameObject;
            GameObject starImage2 = starBar.transform.Find("Star2").gameObject;
            GameObject starImage3 = starBar.transform.Find("Star3").gameObject;

            _dataSlot[i].starImages = new Image[3];
            _dataSlot[i].starImages[0] = starImage1.GetComponent<Image>();
            _dataSlot[i].starImages[1] = starImage2.GetComponent<Image>();
            _dataSlot[i].starImages[2] = starImage3.GetComponent<Image>();

            //Ŭ���� ������ŭ �� ����� ����
            for (int j = 0; j < _dataSlot[i].starCount; j++)
            {
                //_dataSlot[i].starImages[j].color = Color.yellow;
                _dataSlot[i].starImages[j].GetComponent<Image>().sprite = starGetImage;
            }


            //�ش� ��ư Ŭ���� ��������
            _dataSlot[index].stageButton.onClick.AddListener(() =>
            {

                if (_dataSlot[index].open == true)
                {
                    //�ٸ� ������ �����͸� ���������� �Ͻ����� ����(����Ƽ�� �˾Ƽ� ��������)
                    PlayerPrefs.SetInt("Num", _dataSlot[index].stageNum);

                    SceneManager.LoadScene("GameScene");

                }
            });


        }

    }



    public void StageUIFadeIn()
    {
        mainMenuCanvasGroup.alpha = 1f;
        mainMenuCanvasGroup.DOFade(0, 0.3f);

        blackCanvasGroup.DOFade(1, 0.3f);

        stageCanvasGroup.alpha = 0f;

        stageRectTransform.DOGoto(1800f, true);
        stageRectTransform.transform.localPosition = new Vector3(0f, 1800f, 0f);
        //���̵� �� ������ �ӵ�
        stageRectTransform.DOAnchorPos(new Vector2(0f, 0f), 1f, false).SetEase(Ease.OutBack);
        stageCanvasGroup.DOFade(1, 0.7f);

        StartCoroutine("StageButtonsAnimation");
    }


    public void StageUIFadeOut()
    {
        mainMenuCanvasGroup.alpha = 0f;
        mainMenuCanvasGroup.DOFade(1, 0.3f);

        blackCanvasGroup.DOFade(0, 0.3f);

        stageCanvasGroup.alpha = 1f;
        stageRectTransform.transform.localPosition = new Vector3(0f, 0f, 0f);
        stageRectTransform.DOAnchorPos(new Vector2(0f, -1800f), 1f, false).SetEase(Ease.InOutQuint);
        stageCanvasGroup.DOFade(0, 0.7f);
    }


    IEnumerator StageButtonsAnimation()
    {

        foreach (var stage in StageUIBar)
        {
            stage.stageUIBarObj.transform.localScale = Vector3.zero;
            stage.stageUIBarObj.transform.DOScale(0f, 0f);
        }

        foreach (var stage in StageUIBar)
        {
            Managers.Sound.Play("UI/mouse_click", Define.Sound.UI);
            stage.stageUIBarObj.transform.DOScale(1f, 0.8f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.05f);
        }
    }






    public void EasterEggClick()
    {

        if (_easterEggCount <= 10)
        {

            _easterEggCount++;

            if (_easterEggCount == 10)
            {
                Debug.Log("�̽��;ֱ� �߰�!!!!!!");

                Managers.Sound.Stop();

               Camera.main.DOColor(Color.black, 3f).OnComplete(() =>
                {
                    Managers.Sound.Play("UI/CreditsSound",Define.Sound.Bgm);
                });


                mainMenuCanvasGroup.alpha = 1f;
                mainMenuCanvasGroup.DOFade(0, 3f).OnComplete(() =>
                {

                    endCreditsTransform.transform.localPosition = new Vector3(0f, -1800f, 0f);
                    //���̵� �� ������ �ӵ�


                    endCreditsTransform.DOAnchorPos(new Vector2(0f, 1800f), 20f, false).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        endCreditsTransform.transform.localPosition = new Vector3(0f, -1800f, 0f);


                        Camera.main.DOColor(new Color(255 / 255f, 200 / 255f, 125 / 255f, 0), 3f);
                        mainMenuCanvasGroup.alpha = 0f;
                        mainMenuCanvasGroup.DOFade(1, 3f);


                        Managers.Sound.Play("UI/test_music",Define.Sound.Bgm);
                        _easterEggCount = 0;
                    });

                });

            }
        }

    }


    private static void Init()
    {
        if (!s_instance)
        {
            var go = GameObject.Find("MenuManager");
            if (!go)
            {
                go = new GameObject { name = "MenuManager" };
                go.AddComponent<UI_StartSceneUIAnimation>();
            }

            s_instance = go.GetComponent<UI_StartSceneUIAnimation>();
        }
    }

}
