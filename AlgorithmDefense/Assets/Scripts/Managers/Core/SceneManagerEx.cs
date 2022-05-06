using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return Object.FindObjectOfType<BaseScene>(); } }

    public void LoadScene(Define.Scene type)
    {
        Managers.Clear();
        SceneManager.LoadScene(getSceneName(type));
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }

    private string getSceneName(Define.Scene type) => System.Enum.GetName(typeof(Define.Scene), type);
}
