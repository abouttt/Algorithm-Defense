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
        // 범위 안에 적이 있는지
        //_state = Define.State.Moving;

        var m = Physics2D.OverlapCircle(transform.position, _radius, LayerMask.GetMask("Monster"));
        if (m)
        {
            Debug.Log(m.name);
        }

    }

    protected override void UpdateMoving()
    {
        // 적에게 이동하다가 공격 범위안에 있는지
        //_state = Define.State.Attack;

        // 적에게 다가가는 로직

    }

    protected override void UpdateAttack()
    {
        // 적에게 공격하는 로직

        // 적이 죽거나 사라졌다?
        //_state = Define.State.Idle;
    }

    protected override void UpdateDie()
    {

    }
}
