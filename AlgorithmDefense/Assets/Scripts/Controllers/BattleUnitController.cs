using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnitController : BaseUnitController
{
    public BattleUnitData Data = new();

    [SerializeField]
    private GameObject _projectile;

    private BattleUnitController _targetUnit;
    private BaseBuilding _targetBuilding;

    [SerializeField]
    private float _flashDuration;

    private SpriteRenderer _sr;
    private Material _originalMtrl;
    private Material _flashMtrl;

    private Coroutine _flashRoutine;

    private void Update()
    {
        if (Data.CurrentHp <= 0)
        {
            ClearAttack();
            StopFlash();
            _animator.SetBool("Dead", true);
            return;
        }

        if (_targetUnit || _targetBuilding)
        {
            if (IsEndAnimation("Attacked"))
            {
                ClearAttack();
            }

            return;
        }

        CheckTargetUnit();
        if (_targetUnit)
        {
            _animator.SetBool("Attack", true);
            return;
        }

        CheckTargetBuilding();
        if (_targetBuilding)
        {
            _animator.SetBool("Attack", true);
            return;
        }

        Move();
    }


    public void TakeDamage(int damage)
    {
        Data.CurrentHp -= damage;
        Flash();
        Managers.Sound.Play("Unit/Damage", Define.Sound.Effect);
    }

    public void TakeHp(int hp)
    {
        if (Data.CurrentHp > 0)
        {
            Data.CurrentHp += hp;
        }
    }

    private void Move()
    {
        Vector2 dir = (Data.MoveType == Define.Move.Up) ? Vector2.up :
                      (Data.MoveType == Define.Move.Down) ? Vector2.down : Vector2.zero;

        transform.Translate(dir * (Data.MoveSpeed * Time.deltaTime));
    }

    private void AttackAnimationEvent()
    {
        if (_targetUnit)
        {
            if (_targetUnit.Data.CurrentHp > 0)
            {
                if (_projectile)
                {
                    CreateProjectile(_targetUnit.gameObject);
                }
                else
                {
                    _targetUnit.TakeDamage(Data.Damage);
                }
            }
        }
        else if (_targetBuilding)
        {
            if (_projectile)
            {
                CreateProjectile(_targetBuilding.gameObject);
            }
            else
            {
                if (_targetBuilding.gameObject.layer == LayerMask.NameToLayer("Castle"))
                {
                    Managers.Game.CurrentCastleHP -= Data.Damage;
                }
                else
                {
                    Managers.Game.CurrentDungeonHP -= Data.Damage;
                }
            }
        }

        PlayAttackSound();
    }

    private void DeadAnimationEvent()
    {
        Managers.Sound.Play("Unit/Death", Define.Sound.Effect);
        Managers.Resource.Destroy(gameObject);
    }

    private void PlayAttackSound()
    {
        switch (Data.JobType)
        {
            case Define.Job.Warrior:
                Managers.Sound.Play("Unit/WarriorAttack", Define.Sound.Effect);
                break;
            case Define.Job.Archer:
                Managers.Sound.Play("Unit/ArcherAttack", Define.Sound.Effect);
                break;
            case Define.Job.Wizard:
                Managers.Sound.Play("Unit/WizardAttack", Define.Sound.Effect);
                break;
        }
    }

    private void Flash()
    {
        if (_flashRoutine == null)
        {
            _flashRoutine = StartCoroutine(FlashRoutine());
        }
    }

    private void StopFlash()
    {
        if (_flashRoutine != null)
        {
            _sr.material = _originalMtrl;
            StopCoroutine(_flashRoutine);
            _flashRoutine = null;
        }
    }

    private IEnumerator FlashRoutine()
    {
        _sr.material = _flashMtrl;
        yield return new WaitForSeconds(_flashDuration);
        _sr.material = _originalMtrl;
        _flashRoutine = null;
    }

    private void CreateProjectile(GameObject target)
    {
        var go = Managers.Resource.Instantiate($"{Define.PROJECTILE_PREFAB_PATH}{_projectile.name}");
        var projectile = go.GetComponent<ProjectileController>();

        Quaternion quat = ((1 << target.layer) & LayerMask.GetMask("Castle")) != 0 ? Quaternion.Euler(0f, 0f, -90f) : Quaternion.Euler(0f, 0f, 90f);
        projectile.transform.rotation = quat;
        projectile.transform.position = transform.position;
        projectile.Damage = Data.Damage;
        projectile.Target = target;
    }

    private bool IsEndAnimation(string stateName)
    {
        var info = _animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName(stateName) && info.normalizedTime >= 0.99f)
        {
            return true;
        }

        return false;
    }

    private RaycastHit2D GetRayHit2DInfo(LayerMask layer)
    {
        Vector2 dir = (Data.MoveType == Define.Move.Up) ? Vector2.up : Vector2.down;
        return Physics2D.Raycast(transform.position, dir, Data.Range, layer);
    }

    private void CheckTargetUnit()
    {
        var hit = GetRayHit2DInfo(Data.TargetLayer);
        if (hit.collider != null)
        {
            var unit = hit.collider.gameObject.GetComponent<BattleUnitController>();
            if (unit.Data.CurrentHp > 0)
            {
                _targetUnit = unit;
            }
        }
    }

    private void CheckTargetBuilding()
    {
        LayerMask layer = (Data.TargetLayer & LayerMask.GetMask("Monster")) != 0 ? LayerMask.GetMask("Dungeon") : LayerMask.GetMask("Castle");
        var hit = GetRayHit2DInfo(layer);
        if (hit.collider != null)
        {
            _targetBuilding = hit.collider.GetComponent<BaseBuilding>();
        }
    }

    private void ClearAttack()
    {
        _targetUnit = null;
        _targetBuilding = null;
        _animator.SetBool("Attack", false);
    }

    protected override void Init()
    {
        base.Init();

        _sr = GetComponent<SpriteRenderer>();
        _originalMtrl = _sr.material;
        _flashMtrl = Resources.Load<Material>($"Materials/FlashMtrl");
    }
}
