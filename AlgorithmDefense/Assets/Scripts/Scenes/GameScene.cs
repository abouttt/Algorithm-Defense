using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if (FindObjectOfType<BuildingBuilder>() == null)
        {
            Managers.Resource.Instantiate("Contents/BuildingBuilder").name = "@BuildingBuilder";
        }

        if (FindObjectOfType<UI_SceneCanvas>() == null)
        {
            Managers.UI.ShowSceneUI<UI_SceneCanvas>();
        }
    }
}
