using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public GameScene Setting;
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
            _currentCastleHP = value;

            if (!_hpBarAnim)
            {
                _hpBarAnim = Transform.FindObjectOfType<HPBarAnimation>();
            }

            _hpBarAnim.CastleAttacked();
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
            _currentDungeonHP = value;

            if (!_hpBarAnim)
            {
                _hpBarAnim = Transform.FindObjectOfType<HPBarAnimation>();
            }

            _hpBarAnim.EnemyAttacked();
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