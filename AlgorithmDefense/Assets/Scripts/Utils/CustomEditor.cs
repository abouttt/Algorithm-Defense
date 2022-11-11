using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR

using UnityEditor;

public class CustomEditor : EditorWindow
{
    private GameObject _prefab;

    [MenuItem("Custom Editor/CustomEditor")]
    private static void Init()
    {
        var window = (CustomEditor)GetWindow(typeof(CustomEditor));
        window.minSize = new Vector2(400, 70);
        window.maxSize = new Vector2(400, 70);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        _prefab = EditorGUILayout.ObjectField("Prefab", _prefab, typeof(GameObject), false) as GameObject;

        GUILayout.Space(10);
        if (GUILayout.Button("RUN"))
        {
            if (!_prefab)
            {
                return;
            }

            foreach (Canvas canvas in _prefab.transform.GetComponentsInChildren<Canvas>())
            {
                if(!canvas.worldCamera)
                {
                    canvas.worldCamera = Camera.main;
                }
            }

            AssetDatabase.SaveAssets();

            _prefab = null;
        }
    }
}


#endif