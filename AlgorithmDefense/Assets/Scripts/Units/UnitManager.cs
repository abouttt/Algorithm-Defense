using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private Animator _anim;
    [SerializeField] private int hitPoint;

    public void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    public virtual bool LoseHp(int amount)
    {
        hitPoint -= amount;

         _anim.SetBool("Hurt", true);

        if (hitPoint <= 0)
        {
            _anim.SetBool("Dead", true);
            return true;
        }

        return false;
    }

    public void UnitDelete()
    {
        Destroy(this.gameObject);
    }

    public void UnitHit()
    {
        _anim.SetBool("Hurt", false);
    }
}