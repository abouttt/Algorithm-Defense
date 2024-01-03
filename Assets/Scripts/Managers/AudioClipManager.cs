using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipManager : MonoBehaviour
{
    private Dictionary<AudioClip, int> _audioClipCounts = new();
    private Dictionary<AudioClip, bool> _audioPlaying = new();
    private int _maxDuplicateOneShotAudioClips = 5;
    private float _soundSkipTime = 0.05f;

    public bool CanPlayOneShot(AudioClip audioClip)
    {
        if (!_audioClipCounts.ContainsKey(audioClip))
        {
            _audioClipCounts.Add(audioClip, 0);
        }

        if (!_audioPlaying.ContainsKey(audioClip))
        {
            _audioPlaying.Add(audioClip, false);
        }

        if (_audioClipCounts[audioClip] >= _maxDuplicateOneShotAudioClips)
        {
            return false;
        }

        if (_audioPlaying[audioClip] == true)
        {
            return false;
        }

        return true;
    }

    public void IncreaseAudioClipCount(AudioClip audioClip)
    {
        _audioClipCounts[audioClip]++;
        _audioPlaying[audioClip] = true;

        StartCoroutine(DecreaseAudioClipCount(audioClip));
        StartCoroutine(AudioPlayingClear(audioClip));
    }

    public void Clear()
    {
        _audioClipCounts.Clear();
        _audioPlaying.Clear();
    }

    private IEnumerator DecreaseAudioClipCount(AudioClip audioClip)
    {
        yield return YieldCache.WaitForSeconds(audioClip.length);

        if (_audioClipCounts.ContainsKey(audioClip))
        {
            _audioClipCounts[audioClip]--;
        }
    }

    private IEnumerator AudioPlayingClear(AudioClip audioClip)
    {
        yield return YieldCache.WaitForSeconds(_soundSkipTime);

        if (_audioPlaying.ContainsKey(audioClip))
        {
            _audioPlaying[audioClip] = false;
        }
    }
}
