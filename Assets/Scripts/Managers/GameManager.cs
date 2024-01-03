using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public GameScene Setting
    {
        get
        {
            if (!_setting)
            {
                _setting = GameObject.FindObjectOfType<GameScene>();
            }

            return _setting;
        }
    }

    public int Gold = 0;

    public float CastleMaxHP;
    public float DungeonMaxHP;

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
                Managers.Sound.Play("Unit/CastleDamage", Define.Sound.Effect);
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
                Managers.Sound.Play("Unit/CastleDamage", Define.Sound.Effect);
            }

            _currentDungeonHP = value;
        }
    }

    private int _currentCastleHP = 100;
    private int _currentDungeonHP = 100;

    private GameScene _setting;
    private HPBarAnimation _hpBarAnim;

    public void Init()
    {
        _setting = GameObject.FindObjectOfType<GameScene>();
    }

    public void Clear()
    {
        _setting = null;
    }
}