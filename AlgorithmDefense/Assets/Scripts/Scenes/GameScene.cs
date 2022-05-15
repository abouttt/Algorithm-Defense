using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameScene : BaseScene
{
    public override void Clear()
    {

    }

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        if (FindObjectOfType<CitizenSpawner>() == null)
        {
            Managers.Resource.Instantiate("Contents/CitizenSpawner").name = "@CitizenSpawner";
        }

        if (FindObjectOfType<ObjectBuilder>() == null)
        {
            Managers.Resource.Instantiate("Contents/ObjectBuilder").name = "@ObjectBuilder";
        }

        if (FindObjectOfType<UI_SceneCanvas>() == null)
        {
            Managers.UI.ShowSceneUI<UI_SceneCanvas>();
        }
    }
}
