using System;
using UnityEngine;

public static class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        var component = go.GetComponent<T>();
        if (!component)
        {
            component = go.AddComponent<T>();
        }

        return component;
    }

    public static GameObject CreateGameObject<T>(string name, Transform parent = null) where T : UnityEngine.Component
    {
        var go = CreateGameObject(name, parent);
        go.AddComponent<T>();
        return go;
    }

    public static GameObject CreateGameObject(string name, Transform parent = null)
    {
        var go = new GameObject { name = name };
        go.transform.SetParent(parent);
        return go;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        var transform = FindChild<Transform>(go, name, recursive);
        if (!transform)
        {
            return null;
        }

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (!go)
        {
            return null;
        }

        if (recursive)
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name.Equals(name))
                {
                    return component;
                }
            }
        }
        else
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                var transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name.Equals(name))
                {
                    var component = transform.GetComponent<T>();
                    if (component)
                    {
                        return component;
                    }
                }
            }
        }

        return null;
    }
}
