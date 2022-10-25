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


    public void SetBackgroundMusicVolume(float volume)//����� ����
    {
        backgroundMusic.volume = volume;
        endCreditSound.volume = volume;
    }

    public void SetButtonClickVolume(float volume)//ȿ���� ����
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

    //Ŭ���� ��
    public void BtnClick()
    {
        //Ŭ���� ���
        buttonClickSound.Play();
    }

    public void BtnClickStop()
    {
        //Ŭ���� ���
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
