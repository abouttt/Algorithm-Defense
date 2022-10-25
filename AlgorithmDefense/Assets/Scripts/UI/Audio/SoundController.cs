using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    private static SoundController s_instance;
    public static SoundController GetInstance { get { Init(); return s_instance; } }


    [SerializeField]
    private AudioSource backgroundMusic;
    [SerializeField]
    private AudioSource buttonClickSound;
    [SerializeField]
    private AudioSource endCreditSound;


    private void Awake()
    {
        backgroundMusic.Stop();
        buttonClickSound.Stop();

    }


    public void SetBackgroundMusicVolume(float volume)//배경음 볼륨
    {
        backgroundMusic.volume = volume;
        endCreditSound.volume = volume;
    }

    public void SetButtonClickVolume(float volume)//효과음 볼륨
    {
        buttonClickSound.volume = volume;
    }



    public void Background()
    {
        backgroundMusic.Play();
    }

    public void BackgroundStop()
    {
        backgroundMusic.Stop();
    }

    //클릭할 때
    public void BtnClick()
    {
        //클릭음 재생
        buttonClickSound.Play();
    }

    public void BtnClickStop()
    {
        //클릭음 재생
        buttonClickSound.Stop();
    }

    public void EndCredits()
    {
        endCreditSound.Play();
        
    }


    public void EndCreditsStop()
    {
       
        endCreditSound.Stop();
    }



    private static void Init()
    {
        if (!s_instance)
        {
            var go = GameObject.Find("SoundManager");
            if (!go)
            {
                go = new GameObject { name = "SoundManager" };
                go.AddComponent<SoundController>();
            }

            s_instance = go.GetComponent<SoundController>();
        }
    }


}
