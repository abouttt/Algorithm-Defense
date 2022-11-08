using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public void SetBackgroundMusicVolume(float volume)
    {
        Managers.Sound.SetVolume(Define.Sound.Bgm, volume);
    }

    public void SetButtonClickVolume(float volume)
    {
        Managers.Sound.SetVolume(Define.Sound.Effect, volume);
    }
}
