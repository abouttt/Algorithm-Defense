using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingControl : MonoBehaviour
{
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
