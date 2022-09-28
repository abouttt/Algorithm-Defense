using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_StartSceneUIAnimation : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup stageCanvasGroup;
    [SerializeField]
    private RectTransform stageRectTransform;
    [SerializeField]
    private CanvasGroup mainMenuCanvasGroup;

    [SerializeField]
    private GameObject defaultStageUIBar;
    [SerializeField]
    private RectTransform stageButtonContainer;

    [SerializeField]
    private RectTransform endCreditsTransform;

    [SerializeField]
    private GameObject stageChoiceMenu;
    [SerializeField]
    private RectTransform stageChoiceMenuRectTransform;
    [SerializeField]
    private TMPro.TextMeshProUGUI StageChoiceMenuText;

    private int _easterEggCount = 0;

    [System.Serializable]
    public class StageUIBarInformation
    {
        public bool open;          //스테이지 오픈 여부       
        public int starCount;      //별 갯수
        [HideInInspector]
        public Text stageNumText;        //스테이지 텍스트
        [HideInInspector]
        public Image[] starImages;      //스테이지 텍스트
        [HideInInspector]
        public Button stageButton;   //생성된 버튼 오브젝트 정보
        [HideInInspector]
        public GameObject stageUIBarObj;   //생성된 버튼 오브젝트 정보

    }
    public StageUIBarInformation[] StageUIBar;

    [SerializeField]
    private Sprite openImage;
    [SerializeField]
    private Sprite closeImage;






    private void Start()
    {
        //스테이지 갯수&별갯수&열림확인 받아오기

        CreateStageUIBar(stageButtonContainer, defaultStageUIBar, StageUIBar);
    }


    public void CreateStageUIBar(RectTransform _container, GameObject _stageUIBar, StageUIBarInformation[] _dataSlot)
    {
        int y_count = 0;

        for (int i = 0; i < _dataSlot.Length; i++)
        {
            int index = i;

            //버튼 생성
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

            //클리어 갯수만큼 별 노랗게 만듬
            for (int j = 0; j < _dataSlot[i].starCount; j++)
            {
                _dataSlot[i].starImages[j].color = Color.yellow;
            }


            //해당 버튼 클릭시 정보전달
            _dataSlot[index].stageButton.onClick.AddListener(() =>
            {

                if (_dataSlot[index].open == true)
                {
                    stageChoiceMenu.SetActive(true);

                    string num = ("Stage " + index + 1).ToString();
                    StageChoiceMenuText.text = num;

                    SoundController.GetInstance.BtnClick();

                    stageChoiceMenuRectTransform.transform.localScale = Vector3.zero;
                    stageChoiceMenuRectTransform.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);

                }
            });


        }

    }


    public void ChoiceStageGameStartOnClick()
    {
        //받은 버튼 번호가 0이면(길이면)
        // Debug.Log((num+1) + "번 스테이지 클릭");

        SceneManager.LoadScene("GameScene");
    }



    public void StageUIFadeIn()
    {
        mainMenuCanvasGroup.alpha = 1f;
        mainMenuCanvasGroup.DOFade(0, 0.3f);

        stageCanvasGroup.alpha = 0f;
        stageRectTransform.transform.localPosition = new Vector3(0f, 1800f, 0f);
        //페이드 인 종류와 속도
        stageRectTransform.DOAnchorPos(new Vector2(0f, 0f), 1f, false).SetEase(Ease.OutBack);
        stageCanvasGroup.DOFade(1, 0.7f);

        StartCoroutine("StageButtonsAnimation");
    }


    public void StageUIFadeOut()
    {
        mainMenuCanvasGroup.alpha = 0f;
        mainMenuCanvasGroup.DOFade(1, 0.3f);

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
            SoundController.GetInstance.BtnClick();
            stage.stageUIBarObj.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.05f);
        }

        //마지막 사운드 정지
        SoundController.GetInstance.BtnClickStop();
    }






    public void EasterEggClick()
    {

        if (_easterEggCount <= 9)
        {

            _easterEggCount++;

            if (_easterEggCount == 9)
            {
                Debug.Log("이스터애그 발견!!!!!!");

                SoundController.GetInstance.BackgroundStop();

                Camera.main.DOColor(Color.black, 3f).OnComplete(() =>
                {
                    SoundController.GetInstance.EndCredits();
                });


                mainMenuCanvasGroup.alpha = 1f;
                mainMenuCanvasGroup.DOFade(0, 3f).OnComplete(() =>
                {
                    endCreditsTransform.transform.localPosition = new Vector3(0f, -1600f, 0f);
                    //페이드 인 종류와 속도


                    endCreditsTransform.DOAnchorPos(new Vector2(0f, 1600f), 20f, false).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        endCreditsTransform.transform.localPosition = new Vector3(0f, -1600f, 0f);


                        Camera.main.DOColor(new Color(255 / 255f, 200 / 255f, 125 / 255f, 0), 3f);
                        mainMenuCanvasGroup.alpha = 0f;
                        mainMenuCanvasGroup.DOFade(1, 3f);


                        SoundController.GetInstance.EndCreditsStop();
                        SoundController.GetInstance.Background();
                        _easterEggCount = 0;
                    });

                });

            }
        }

    }




}
