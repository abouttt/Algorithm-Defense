using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private Animator _anim;
    public int MaxHP;
    [HideInInspector]
    public int CurrentHP;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (CurrentHP <= 0)
        {
            _anim.SetBool("Dead", true);
        }
    }

    public void LoseHp(int amount)
    {
        CurrentHP -= amount;

        _anim.SetBool("Hurt", true);
    }

    public void GetHp(int amount)
    {
        if (CurrentHP >= MaxHP)
        {
            return;
        }

        CurrentHP += amount;
    }

    public void UnitDelete()
    {
        Managers.Resource.Destroy(gameObject);
    }

    public void UnitHit()
    {
        _anim.SetBool("Hurt", false);
    }
}