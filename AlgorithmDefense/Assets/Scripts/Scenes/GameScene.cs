using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    public static readonly string CONTENTS_PATH = "Prefabs/Contents/";

    public override void Clear()
    {

    }

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.GameScene;

        if (FindObjectOfType<BuildingBuilder>() == null)
        {
            Managers.Resource.Instantiate($"{CONTENTS_PATH}BuildingBuilder").name = "@BuildingBuilder";
        }

        if (FindObjectOfType<RoadBuilder>() == null)
        {
            Managers.Resource.Instantiate($"{CONTENTS_PATH}RoadBuilder").name = "@RoadBuilder";
        }

        if (FindObjectOfType<TileSelector>() == null)
        {
            Managers.Resource.Instantiate($"{CONTENTS_PATH}TileSelector").name = "@TileSelector";
        }

        if (FindObjectOfType<CitizenSpawner>() == null)
        {
            Managers.Resource.Instantiate($"{CONTENTS_PATH}CitizenSpawner").name = "@CitizenSpawner";
        }

        if (FindObjectOfType<UI_SceneCanvas>() == null)
        {
            Managers.UI.ShowSceneUI<UI_SceneCanvas>();
        }
    }
}
