using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private Slider backgroundSlider;
    [SerializeField]
    private Slider effectSlider;

    private void Start()
    {
        backgroundSlider.value = Managers.Sound.GetVolume(Define.Sound.Bgm);
        effectSlider.value = Managers.Sound.GetVolume(Define.Sound.UI);
    }


    public void SetBackgroundVolume(float volume)
    {
        Managers.Sound.SetVolume(Define.Sound.Bgm, volume);
    }

    public void SetEffectVolume(float volume)
    {
        Managers.Sound.SetVolume(Define.Sound.UI, volume);
        Managers.Sound.SetVolume(Define.Sound.Effect, volume);
        Managers.Sound.Play("UI/mouse_click", Define.Sound.UI);
    }

    public void ButtonClick()
    {
        Managers.Sound.Play("UI/mouse_click", Define.Sound.UI);
    }

}
