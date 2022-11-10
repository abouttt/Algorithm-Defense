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
                var go = Managers.Resource.Instantiate($"{Define.CONTENTS_PATH}@GameSceneSetting");
                _setting = go.GetComponent<GameScene>();
                GameObject.DontDestroyOnLoad(go);
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
            }

            _currentDungeonHP = value;
        }
    }

    private GameScene _setting;

    private int _currentCastleHP = 100;
    private int _currentDungeonHP = 100;

    private HPBarAnimation _hpBarAnim;
}