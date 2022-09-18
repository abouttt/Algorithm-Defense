using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UI_StartSceneAnimation : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup stageCanvasGroup;
    [SerializeField]
    private RectTransform stageRectTransform;
    [SerializeField]
    private CanvasGroup mainMenuCanvasGroup;

    [SerializeField]
    private Button defaultButton;
    [SerializeField]
    private RectTransform stageButtonContainer;
    private List<Button> stage = new List<Button>();

    [SerializeField]
    private RectTransform EndCreditsTransform;

    private int eggCount = 0;


    [System.Serializable]
    public class StageButtonsInformation
    {
        //public Sprite ButtonSprite; //버튼 이미지
        public bool _open;           //가격       
        public int _starCount;
        [HideInInspector]
        public Text NumText;       //가격 텍스트
        [HideInInspector]
        public Button ButtonObj;   //생성된 버튼 오브젝트 정보
    }

    public StageButtonsInformation[] StageButtons;

    private void Start()
    {
        //스테이지 갯수&별갯수&열림확인 받아오기


        CreateButton(stageButtonContainer, defaultButton, StageButtons);

    }


    public void CreateButton(RectTransform container, Button btn_Obj, StageButtonsInformation[] btn_Slot)
    {
        int y_count = 0;

        for (int i = 0; i < btn_Slot.Length; i++)
        {
            int index = i;
           

            //버튼 생성
            Button newButton = Instantiate(btn_Obj, container.transform);
            btn_Slot[i].ButtonObj = newButton;

            if (i != 0 && (i % 5) == 0)
            {
                y_count++;
            }


            //생성된 버튼의 좌표와 이미지 변경
            newButton.transform.localPosition = new Vector3(newButton.transform.localPosition.x + (100f * i)-(500f* y_count), newButton.transform.localPosition.y-(100f* y_count), newButton.transform.localPosition.z);


            GameObject buttonText = newButton.transform.Find("Text").gameObject;
            btn_Slot[i].NumText = buttonText.GetComponent<Text>();

            //찾은 text에 cost를 형변환해서 변경함
            string conversion = (index+1).ToString();
            btn_Slot[i].NumText.text = conversion;


            // newButton.GetComponent<Image>().sprite = btn_Slot[i].ButtonSprite;



            //해당 버튼 클릭시 정보전달
            btn_Slot[index].ButtonObj.onClick.AddListener(() =>
            {
                SoundController.GetInstance.BtnClick();
                StageButtonOnClick(index);

            });

        }


    }


    public void StageButtonOnClick(int num)
    {
        //받은 버튼 번호가 0이면(길이면)
        Debug.Log((num+1) + "번 스테이지 클릭");

        SceneManager.LoadScene("GameScene");
    }



    public void FadeIn()
    {
        // Debug.Log("시작버튼 클릭ㄷ");
        //시작 alpha 0


        mainMenuCanvasGroup.alpha = 1f;
        mainMenuCanvasGroup.DOFade(0, 0.3f);


        stageCanvasGroup.alpha = 0f;
        stageRectTransform.transform.localPosition = new Vector3(0f, 1000f, 0f);
        //페이드 인 종류와 속도
        stageRectTransform.DOAnchorPos(new Vector2(0f, 0f), 1f, false).SetEase(Ease.OutBack);

        stageCanvasGroup.DOFade(1, 0.7f);



        StartCoroutine("StageButtonsAnimation");

    }


    public void FadeOut()
    {

        mainMenuCanvasGroup.alpha = 0f;
        mainMenuCanvasGroup.DOFade(1, 0.3f);

        stageCanvasGroup.alpha = 1f;
        stageRectTransform.transform.localPosition = new Vector3(0f, 0f, 0f);
        stageRectTransform.DOAnchorPos(new Vector2(0f, -900f), 1f, false).SetEase(Ease.InOutQuint);
        stageCanvasGroup.DOFade(0, 0.7f);

      

    }


    IEnumerator StageButtonsAnimation()
    {

        foreach(var stage in StageButtons)
        {
            stage.ButtonObj.transform.localScale = Vector3.zero;
            stage.ButtonObj.transform.DOScale(0f, 0f);
        }

        foreach (var stage in StageButtons)
        {
            SoundController.GetInstance.BtnClick();
            stage.ButtonObj.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.03f);
        }

        //마지막 사운드 정지
        SoundController.GetInstance.BtnClickStop();
    }






    public void EasterEggClick()
    {

        if (eggCount <= 9)
        {
      
            eggCount++;

            if (eggCount == 9)
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
                    EndCreditsTransform.transform.localPosition = new Vector3(0f, -1200f, 0f);
                    //페이드 인 종류와 속도


                    EndCreditsTransform.DOAnchorPos(new Vector2(0f, 1200f), 20f, false).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        EndCreditsTransform.transform.localPosition = new Vector3(0f, -1200f, 0f);


                        Camera.main.DOColor(new Color(255/255f,200/255f,125/255f,0), 3f);
                        mainMenuCanvasGroup.alpha = 0f;
                        mainMenuCanvasGroup.DOFade(1, 3f);


                        SoundController.GetInstance.EndCreditsStop();
                        SoundController.GetInstance.Background();
                        eggCount = 0;
                    });

                });

            }
        }




    }








}
