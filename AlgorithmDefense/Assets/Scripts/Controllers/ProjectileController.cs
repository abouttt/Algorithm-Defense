using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float MoveSpeed;

    [HideInInspector]
    public int Damage;
    [HideInInspector]
    public GameObject Target;

    [SerializeField]
    private bool _isFireball;

    private void Update()
    {
        if (!Target || (Target.activeSelf == false))
        {
            Clear();
            return;
        }

        Move();
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, (MoveSpeed * Time.deltaTime));
        if (Vector2.Distance(transform.position, Target.transform.position) <= 0.01f)
        {
            var battleUnit = Target.GetComponent<BattleUnitController>();
            if (battleUnit)
            {
                if (battleUnit.Data.CurrentHp > 0)
                {
                    battleUnit.TakeDamage(Damage);
                }
            }
            else
            {
                var building = Target.GetComponent<BaseBuilding>();
                if (building)
                {
                    if (Target.layer == LayerMask.NameToLayer("Castle"))
                    {
                        Managers.Game.CurrentCastleHP -= Damage;
                    }
                    else
                    {
                        Managers.Game.CurrentDungeonHP -= Damage;
                    }
                }
            }

            if (_isFireball)
            {
                CreateFireballDamageEffect();
            }

            Clear();
        }
    }

    private void CreateFireballDamageEffect()
    {
        var go = Managers.Resource.Instantiate("Prefabs/Projectile/FireBallDamage");
        go.transform.position = transform.position;
        go.transform.rotation = transform.rotation;
    }

    private void Clear()
    {
        Target = null;
        Managers.Resource.Destroy(gameObject);
    }
}
