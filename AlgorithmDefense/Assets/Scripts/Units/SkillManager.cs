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

    void Update()
    {
        Destroy(gameObject, destroySkill);
    }

    public void AddDamaged(UnitManager a)
    {
        if (a)
        {
            a.LoseHp(Damaged);
        }
    }

    public void AddHp(UnitManager a)
    {
        if (a)
        {
            a.GetHp(Damaged);
        }
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
            if (colliders != null)
            {
                AddDamaged(collider2D.GetComponent<UnitManager>());

            }
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
            if (colliders != null)
            {
                AddDamaged(collider2D.GetComponent<UnitManager>());

            }
        }
    }
}
