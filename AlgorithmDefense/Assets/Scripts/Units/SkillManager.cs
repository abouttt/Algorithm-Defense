using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private UnitManager _detectedUnit;
    [SerializeField]
    private LayerMask _layerMask;
    [SerializeField]
    private int _damage;
    [SerializeField]
    private Vector2 _rangeBoxSize = new Vector2(1, 1);
    [SerializeField]
    private float _destroyTime;

    public enum skillType { Damage, Heal };
    public skillType Type;

    private void Start()
    {
        Destroy(gameObject, _destroyTime);
    }

    public void CheckLayerDamaged()
    {
        if (_detectedUnit)
        {
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, _rangeBoxSize, 0f, _layerMask);

        foreach (Collider2D collider2D in colliders)
        {
            collider2D.GetComponent<BattleUnitController>().TakeDamage(_damage);
        }
    }

    public void CheckLayerHeal()
    {
        if (_detectedUnit)
        {
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, _rangeBoxSize, 0f, _layerMask);

        foreach (Collider2D collider2D in colliders)
        {
            collider2D.GetComponent<BattleUnitController>().TakeHp(_damage);
        }
    }
}
