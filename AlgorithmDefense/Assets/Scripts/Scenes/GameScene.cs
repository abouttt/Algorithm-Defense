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

        if (FindObjectOfType<BuildingBuilder>() == null)
        {
            Managers.Resource.Instantiate("Contents/BuildingBuilder").name = "@BuildingBuilder";
        }

        if (FindObjectOfType<RoadBuilder>() == null)
        {
            Managers.Resource.Instantiate("Contents/RoadBuilder").name = "@RoadBuilder";
        }

        if (FindObjectOfType<CitizenSpawner>() == null)
        {
            Managers.Resource.Instantiate("Contents/CitizenSpawner").name = "@CitizenSpawner";
        }

        if (FindObjectOfType<UI_SceneCanvas>() == null)
        {
            Managers.UI.ShowSceneUI<UI_SceneCanvas>();
        }
    }
}
