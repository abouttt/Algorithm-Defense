using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public UnitData Data = new();

    public float CurrentSpeed = 0f;

    protected UnitController _targetUnit;
    protected BaseBuilding _targetBuilding;

    private Animator _animator;

    private IUnitState _moveState;
    private IUnitState _attackState;
    private IUnitState _hurtState;
    private IUnitState _dieState;

    private UnitStateContext _unitStateContext;


    private void Awake()
    {
        _unitStateContext = new(this);

        _animator = GetComponent<Animator>();

        _moveState = gameObject.GetOrAddComponent<UnitMoveState>();
        _attackState = gameObject.GetOrAddComponent<UnitAttackState>();
        _hurtState = gameObject.GetOrAddComponent<UnitHurtState>();
        _dieState = gameObject.GetOrAddComponent<UnitDieState>();
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, Vector2.up * Data.Range);
    }

    public void Move()
    {
        _unitStateContext.Transition(_moveState);
    }

    public void Attack()
    {
        _animator.SetBool("Attack", true);
        _unitStateContext.Transition(_attackState);
    }

    public void Hurt()
    {
        _animator.SetBool("Hurt", true);
        _unitStateContext.Transition(_hurtState);
    }

    public void Dead()
    {
        _animator.SetBool("Dead", true);
        _unitStateContext.Transition(_dieState);
    }

    public void SetNextDestination(Vector3 startPos)
    {
        var cellPos = TileManager.GetInstance.GetWorldToCell(Define.Tilemap.Ground, startPos);
        switch (Data.MoveType)
        {
            case Define.Move.Down:
                cellPos.y--;
                break;
            case Define.Move.Up:
                cellPos.y++;
                break;
            case Define.Move.Right:
                cellPos.x++;
                break;
            case Define.Move.Left:
                cellPos.x--;
                break;
        }

        Data.Destination = TileManager.GetInstance.GetCellCenterToWorld(Define.Tilemap.Ground, cellPos);
    }

    public void SetMoveAnimation()
    {
        switch (Data.MoveType)
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

    public void SetReverseMoveType()
    {
        switch (Data.MoveType)
        {
            case Define.Move.Right:
                Data.MoveType = Define.Move.Left;
                break;
            case Define.Move.Left:
                Data.MoveType = Define.Move.Right;
                break;
            case Define.Move.Up:
                Data.MoveType = Define.Move.Down;
                break;
            case Define.Move.Down:
                Data.MoveType = Define.Move.Up;
                break;
        }
    }
}
