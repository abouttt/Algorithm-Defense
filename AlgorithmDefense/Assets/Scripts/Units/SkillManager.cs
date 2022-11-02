using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private UnitManager _detectedUnit;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private int Damaged;
    [SerializeField] private float _range = 0f;
    [SerializeField] private float destroySkill;

    public enum skillType { Damage, Heal };
    public skillType Type;

    private void Start()
    {
        Destroy(gameObject, destroySkill);
    }

    public void CheckLayerDamaged()
    {
        if (_detectedUnit)
        {
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _range, layerMask);

        foreach (Collider2D collider2D in colliders)
        {
            collider2D.GetComponent<BattleUnitController>().TakeDamage(Damaged);
        }
    }

    public void CheckLayerHeal()
    {
        if (_detectedUnit)
        {
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _range, layerMask);

        foreach (Collider2D collider2D in colliders)
        {
            collider2D.GetComponent<BattleUnitController>().TakeHp(Damaged);
        }
    }
}
