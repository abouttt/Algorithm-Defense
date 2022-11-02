using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtFlash : MonoBehaviour
{
    [SerializeField]
    private float _duration;

    private SpriteRenderer _sr;
    private Material _originalMtrl;
    private Material _flashMtrl;

    private Coroutine _flashRoutine;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _originalMtrl = _sr.material;
        _flashMtrl = Resources.Load<Material>($"Materials/FlashMtrl");
    }

    public void Flash()
    {
        if (_flashRoutine != null)
        {
            StopCoroutine(_flashRoutine);
        }

        _flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        _sr.material = _flashMtrl;
        yield return new WaitForSeconds(_duration);
        _sr.material = _originalMtrl;
        _flashRoutine = null;
    }
}
