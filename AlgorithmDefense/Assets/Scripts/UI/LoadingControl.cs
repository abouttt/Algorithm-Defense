using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingControl : MonoBehaviour
{
    public Action LoadingCompleteAction;

    [SerializeField]
    private GameObject lodingObject;

    private static LoadingControl s_instance;
    public static LoadingControl GetInstance { get { Init(); return s_instance; } }


    private void Awake()
    {
        lodingObject.SetActive(true);
    }

    public void LoadingComplete()
    {
        lodingObject.SetActive(false);
    }

    public void GameSceneLoadingComplete()
    {
        Managers.Clear();
        LoadingCompleteAction.Invoke();
        lodingObject.SetActive(false);
    }

    private static void Init()
    {
        if (!s_instance)
        {
            var go = GameObject.Find("MenuManager");
            if (!go)
            {
                go = new GameObject { name = "MenuManager" };
                go.AddComponent<LoadingControl>();
            }

            s_instance = go.GetComponent<LoadingControl>();
        }
    }

}
