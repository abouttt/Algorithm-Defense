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
        backgroundSlider.value = Managers.Game.BackgroundVolume;
        effectSlider.value = Managers.Game.EffectVolume;
    }


    public void SetBackgroundVolume(float volume)
    {
        Managers.Sound.SetVolume(Define.Sound.Bgm, volume);
        Managers.Game.BackgroundVolume = volume;
    }

    public void SetEffectVolume(float volume)
    {
        Managers.Sound.SetVolume(Define.Sound.Effect, volume);
        Managers.Game.EffectVolume = volume;
        Managers.Sound.Play("UI/mouse_click", Define.Sound.Effect);
    }

    public void ButtonClick()
    {

        Managers.Sound.Play("UI/mouse_click", Define.Sound.Effect);
    }

}
