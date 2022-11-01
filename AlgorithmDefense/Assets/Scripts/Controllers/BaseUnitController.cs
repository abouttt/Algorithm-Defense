using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnitController : MonoBehaviour
{
    protected Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetMoveAnimation(Define.Move moveType)
    {
        switch (moveType)
        {
            case Define.Move.Down:
                _animator.SetFloat("Hor", 0);
                _animator.SetFloat("Ver", -1);
                break;
            case Define.Move.Up:
                _animator.SetFloat("Hor", 0);
                _animator.SetFloat("Ver", 1);
                break;
            case Define.Move.Right:
                _animator.SetFloat("Hor", 1);
                _animator.SetFloat("Ver", 0);
                break;
            case Define.Move.Left:
                _animator.SetFloat("Hor", -1);
                _animator.SetFloat("Ver", 0);
                break;
        }
    }
}
