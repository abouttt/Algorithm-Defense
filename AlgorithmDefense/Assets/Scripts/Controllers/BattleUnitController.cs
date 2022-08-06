using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnitController : BaseController
{
    [SerializeField]
    private float _radius;

    public override void Init()
    {
        _state = Define.State.Idle;
    }

    protected override void UpdateIdle()
    {
        // ���� �ȿ� ���� �ִ���
        //_state = Define.State.Moving;

        var m = Physics2D.OverlapCircle(transform.position, _radius, LayerMask.GetMask("Monster"));
        if (m)
        {
            Debug.Log(m.name);
        }

    }

    protected override void UpdateMoving()
    {
        // ������ �̵��ϴٰ� ���� �����ȿ� �ִ���
        //_state = Define.State.Attack;

        // ������ �ٰ����� ����

    }

    protected override void UpdateAttack()
    {
        // ������ �����ϴ� ����

        // ���� �װų� �������?
        //_state = Define.State.Idle;
    }

    protected override void UpdateDie()
    {

    }
}
