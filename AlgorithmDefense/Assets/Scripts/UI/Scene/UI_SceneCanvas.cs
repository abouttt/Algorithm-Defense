using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SceneCanvas : UI_Scene
{
    public override void Init()
    {
        base.Init();

        if (FindObjectOfType<UI_CitizenSpawnButtons>() == null)
        {
            Managers.Resource.Instantiate("UI/Scene/UI_CitizenSpawnButtons", transform);
        }

        if (FindObjectOfType<UI_BuildingButtons>() == null)
        {
            Managers.Resource.Instantiate("UI/Scene/UI_BuildingButtons", transform);
        }
    }
}
