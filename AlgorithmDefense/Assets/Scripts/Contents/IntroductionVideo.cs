using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroductionVideo : MonoBehaviour
{
    [SerializeField]
    private float _delayMilliseconds;

    [SerializeField]
    private VideoPlayer _vp;
    [SerializeField]
    private RawImage _videoTexture;
    [SerializeField]
    private Image _blackImage;

    private Vector3 _mousePos;
    private Vector3 _prevMousePos;
    private Stopwatch _sw = new();

    private void Start()
    {
        _sw.Start();
        _vp.Prepare();
    }

    private void Update()
    {
        _mousePos = Input.mousePosition;

        if (_prevMousePos != _mousePos)
        {
            if (_vp.isPlaying)
            {
                _vp.Stop();
                _videoTexture.gameObject.SetActive(false);
                _blackImage.gameObject.SetActive(true);
                StartCoroutine(SetActiveFalseBlackImage());
            }

            _prevMousePos = _mousePos;
            _sw.Restart();
        }

        if (_sw.ElapsedMilliseconds >= _delayMilliseconds)
        {
            _videoTexture.gameObject.SetActive(true);
            _vp.Play();
        }
    }

    private IEnumerator SetActiveFalseBlackImage()
    {
        yield return new WaitForSeconds(0.01f);
        _blackImage.gameObject.SetActive(false);
    }
}
