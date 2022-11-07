using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCountManager : MonoBehaviour
{
    private Dictionary<AudioClip, int> _audioClipCounts = new();
    private int _maxDuplicateOneShotAudioClips = 5;

    public bool CanPlayOneShot(AudioClip audioClip)
    {
        if (!_audioClipCounts.ContainsKey(audioClip))
        {
            _audioClipCounts.Add(audioClip, 0);
        }

        if (_audioClipCounts[audioClip] >= _maxDuplicateOneShotAudioClips)
        {
            return false;
        }

        return true;
    }

    public void IncreaseAudioClipCount(AudioClip audioClip)
    {
        if (!_audioClipCounts.ContainsKey(audioClip))
        {
            _audioClipCounts.Add(audioClip, 0);
        }

        if (_audioClipCounts[audioClip] >= _maxDuplicateOneShotAudioClips)
        {
            return;
        }

        _audioClipCounts[audioClip]++;
        StartCoroutine(DecreaseAudioClipCount(audioClip));
    }

    private IEnumerator DecreaseAudioClipCount(AudioClip audioClip)
    {
        yield return new WaitForSeconds(audioClip.length);
        _audioClipCounts[audioClip]--;
    }
}
