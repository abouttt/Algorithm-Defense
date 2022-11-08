using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public GameScene Setting;
    public int Gold = 0;

    public float CastleMaxHP;
    public float DungeonMaxHP;

    public float BackgroundVolume=1f;
    public float EffectVolume=1f;

    public int CurrentCastleHP
    {
        get
        {
            return _currentCastleHP;
        }
        set
        {
            if (!_hpBarAnim)
            {
                _hpBarAnim = Transform.FindObjectOfType<HPBarAnimation>();
            }

            if (_currentCastleHP > value)
            {
                _hpBarAnim.CastleAttacked();
                Managers.Sound.Play("Unit/CastleDamage");
            }

            _currentCastleHP = value;
        }
    }

    public int CurrentDungeonHP
    {
        get
        {
            return _currentDungeonHP;
        }
        set
        {
            if (!_hpBarAnim)
            {
                _hpBarAnim = Transform.FindObjectOfType<HPBarAnimation>();
            }

            if (_currentDungeonHP > value)
            {
                _hpBarAnim.EnemyAttacked();
            }

            _currentDungeonHP = value;
        }
    }

    private int _currentCastleHP = 100;
    private int _currentDungeonHP = 100;

    private HPBarAnimation _hpBarAnim;

    public void Init()
    {
        Setting = GameObject.FindObjectOfType<GameScene>();
    }
}