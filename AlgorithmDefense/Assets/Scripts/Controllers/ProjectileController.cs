using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ProjectileController : MonoBehaviour
{
    public float MoveSpeed;

    [HideInInspector]
    public int Damage;
    [HideInInspector]
    public GameObject Target;

    private void Update()
    {
        if (!Target)
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
                        Managers.Game.CastleHP -= Damage;
                    }
                    else
                    {
                        Managers.Game.DungeonHP -= Damage;
                    }
                }
            }

            Clear();
        }
    }

    private void Clear()
    {
        Target = null;
        Managers.Resource.Destroy(gameObject);
    }
}
