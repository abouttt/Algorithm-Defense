using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAnimationEnd : MonoBehaviour
{
    [SerializeField]
    private string _stateName;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        var info = _animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName(_stateName) && info.normalizedTime >= 0.99f)
        {
            Managers.Resource.Destroy(gameObject);
        }
    }
}
