using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using DG.Tweening;
using TMPro;


public class ClearMenuAnimation : MonoBehaviour
{

    private static ClearMenuAnimation s_instance;
    public static ClearMenuAnimation GetInstance { get { Init(); return s_instance; } }

    [SerializeField]
    private GameObject clearMenuObj;
    [SerializeField]
    private GameObject clearMenuButtonsObj;
    [SerializeField]
    private RectTransform clearMenuTransform;
    [SerializeField]
    private TextMeshProUGUI clearText;
    [SerializeField]
    private Sprite starGetImage;
    [SerializeField]
    private Sprite starFailImage;

    private GameObject[] starImages;

    public void Start()
    {
       
        clearMenuObj.SetActive(false);
        clearMenuTransform.DOScale(0.3f, 0f);
        clearMenuButtonsObj.SetActive(false);
        starImages = new GameObject[3];

        GameObject star = clearMenuTransform.Find("StarImages").gameObject;
        starImages[0] = star.transform.Find("Star1").gameObject;
        starImages[1] = star.transform.Find("Star2").gameObject;
        starImages[2] = star.transform.Find("Star3").gameObject;

        starImages[0].transform.DOScale(0f, 0f);
        starImages[1].transform.DOScale(0f, 0f);
        starImages[2].transform.DOScale(0f, 0f);

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
          
            ClearMenuCall(3,true);
        }
    }


    public void ClearMenuCall(int _star,bool _victory)
    {
        
        clearMenuObj.SetActive(true);
        Managers.Sound.SetVolume(Define.Sound.Effect, 0f);

        if (_victory)
        {
            clearText.text = "Victory";
            Managers.Sound.Play("UI/Victory", Define.Sound.Bgm);
        }
        else
        {
            clearText.text = "Defeat";
            Managers.Sound.Play("UI/Defeat", Define.Sound.Bgm);
        }
      

        for(int i=0;i<_star;i++)
        {
            starImages[i].GetComponent<Image>().sprite = starGetImage;
        }


        clearMenuTransform.DOScale(1.2f, 0.4f).OnComplete(() =>
        {
            clearMenuTransform.DOScale(1f, 0.3f).OnComplete(() =>
            {
               starImages[0].transform.DORotate(new Vector3(0f, 360f, 0f), 1f,RotateMode.FastBeyond360);
               starImages[0].transform.DOScale(1.2f, 0.2f).OnComplete(() =>
                {
                    
                    starImages[0].transform.DOScale(1f, 0.2f);
                    starImages[1].transform.DORotate(new Vector3(0f, 360f, 30f), 1f, RotateMode.FastBeyond360);
                    starImages[1].transform.DOScale(1.2f, 0.2f).OnComplete(() =>
                    {

                        starImages[1].transform.DOScale(1f, 0.2f);
                        starImages[2].transform.DORotate(new Vector3(0f, 360f, 330f), 1f, RotateMode.FastBeyond360);
                        starImages[2].transform.DOScale(1.2f, 0.2f).OnComplete(() =>
                        {

                            starImages[2].transform.DOScale(1f, 0.2f).OnComplete(() =>
                            {
                                //타이밍맞추기용 애니메이션(아무의미 없음)
                                starImages[0].transform.DOScale(1f, 0.8f).OnComplete(() =>
                                {
                                    clearMenuButtonsObj.SetActive(true);
                                    Time.timeScale = 0f;
                                    Managers.Sound.SetVolume(Define.Sound.Effect, Managers.Game.EffectVolume);
                                });
                            });

                        });

                    });

                });
    
            });


        });


       
    }



    private static void Init()
    {
        if (!s_instance)
        {
            var go = GameObject.Find("MenuManager");
            if (!go)
            {
                go = new GameObject { name = "MenuManager" };
                go.AddComponent<ClearMenuAnimation>();
            }

            s_instance = go.GetComponent<ClearMenuAnimation>();
        }
    }



}
