using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SceneCanvas : UI_Scene
{
    public override void Init()
    {
        base.Init();

        if (GetComponentInChildren<UI_CitizenSpawnController>() == null)
        {
            Managers.Resource.Instantiate("UI/Scene/UI_CitizenSpawnButtons", transform);
        }

        if (GetComponentInChildren<UI_CitizenSpawnController>() == null)
        {
            Managers.Resource.Instantiate("UI/Scene/UI_BuildButtons", transform);
        }
    }
}
