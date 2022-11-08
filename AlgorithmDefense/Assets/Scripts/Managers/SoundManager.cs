using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager
{
    private AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    private Dictionary<string, AudioClip> _audioClips = new();

    public void Init()
    {
        var root = GameObject.Find("@Sound");
        if (!root)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            var soundNames = System.Enum.GetNames(typeof(Define.Sound));
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                var go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.AddComponent<SoundCounter>();
                go.transform.parent = root.transform;
            }

            _audioSources[(int)Define.Sound.Bgm].loop = true;
        }
    }

    public void Play(string path, Define.Sound type = Define.Sound.Effect, float volume = 1.0f)
    {
        var audioClip = getOrAddAudioClip(path, type);
        Play(audioClip, type, volume);
    }

    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float volume = 1.0f)
    {
        if (!audioClip)
        {
            return;
        }

        if (type == Define.Sound.Bgm)
        {
            var audioSource = _audioSources[(int)Define.Sound.Bgm];
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.volume = volume;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            var audioSource = _audioSources[(int)type];
            var audioCountManager = audioSource.GetComponent<SoundCounter>();
            if (audioCountManager.CanPlayOneShot(audioClip))
            {
                audioSource.volume = volume;
                audioSource.PlayOneShot(audioClip);
                audioCountManager.IncreaseAudioClipCount(audioClip);
            }
        }
    }

    public void SetVolume(Define.Sound soundType, float volume)
    {
        _audioSources[(int)soundType].volume = volume;
    }

    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        _audioClips.Clear();
    }

    private AudioClip getOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        if (!path.Contains("Sounds/"))
        {
            path = $"Sounds/{path}";
        }

        AudioClip audioClip = null;

        if (type == Define.Sound.Bgm)
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
        }
        else
        {
            if (!_audioClips.TryGetValue(path, out audioClip))
            {
                audioClip = Managers.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        if (!audioClip)
        {
            Debug.Log($"[SoundManager] AudioClip Missing. {path}");
        }

        return audioClip;
    }
}
