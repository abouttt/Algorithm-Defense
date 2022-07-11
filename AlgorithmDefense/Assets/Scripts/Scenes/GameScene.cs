using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public static readonly string CONTENTS_PATH = "Prefabs/Contents/";

    private void Start()
    {
        Managers.Resource.Instantiate($"Prefabs/UI/EventSystem").name = "@EventSystem";

        if (!FindObjectOfType<BuildingBuilder>())
        {
            Managers.Resource.Instantiate($"{CONTENTS_PATH}BuildingBuilder").name = "@BuildingBuilder";
        }

        if (!FindObjectOfType<RoadBuilder>())
        {
            Managers.Resource.Instantiate($"{CONTENTS_PATH}RoadBuilder").name = "@RoadBuilder";
        }

        if (!FindObjectOfType<TileSelector>())
        {
            Managers.Resource.Instantiate($"{CONTENTS_PATH}TileSelector").name = "@TileSelector";
        }

        if (!FindObjectOfType<CitizenSpawner>())
        {
            Managers.Resource.Instantiate($"{CONTENTS_PATH}CitizenSpawner").name = "@CitizenSpawner";
        }

        if (!FindObjectOfType<UI_SceneCanvas>())
        {
            Managers.UI.ShowSceneUI<UI_SceneCanvas>();
        }
    }
}
