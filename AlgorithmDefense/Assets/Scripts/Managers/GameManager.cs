using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager
{
    public GameScene Setting;

    public void Init()
    {
        Setting = GameObject.FindObjectOfType<GameScene>();
    }
}
