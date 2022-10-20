using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private UnitManager _detectedUnit;
    private Animator _anim;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private int attackDamage;
    [SerializeField] private float _range = 0f;

    void Update()
    {
        CheckLayer();
    }

    public void AddDamaged()
    {
        bool unitDie = _detectedUnit.LoseHp(attackDamage);

        if (unitDie)
        {
            _detectedUnit = null;
        }

    }
    private void CheckLayer()
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
                _detectedUnit = collider2D.GetComponent<UnitManager>();
            }
        }
    }
}
