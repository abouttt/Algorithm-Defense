using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public GameScene Setting;
    public int Gold = 0;

    public int CastleHP
    {
        get
        {
            return _castleHP;
        }
        set
        {
            _castleHP += value;
            
            if(!_hpBarAnim)
            {
                _hpBarAnim = Transform.FindObjectOfType<HPBarAnimation>();
            }

            _hpBarAnim.CastleAttacked();
        }
    }

    public int DungeonHP
    {
        get
        {
            return _dungeonHP;
        }
        set
        {
            _dungeonHP += value;

            if (!_hpBarAnim)
            {
                _hpBarAnim = Transform.FindObjectOfType<HPBarAnimation>();
            }

            _hpBarAnim.EnemyAttacked();
        }
    }

    private int _castleHP = 100;
    private int _dungeonHP = 100;

    private HPBarAnimation _hpBarAnim;

    public void Init()
    {
        Setting = GameObject.FindObjectOfType<GameScene>();
    }
}
