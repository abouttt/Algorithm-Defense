using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.TextCore.Text;


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

            TMP_FontAsset font = Resources.Load<TMP_FontAsset>("Font/DNFBitBitTTF SDF");
            foreach (var tmp in _prefab.transform.GetComponentsInChildren<TextMeshProUGUI>())
            {
                tmp.font = font;
            }

            AssetDatabase.SaveAssets();

            _prefab = null;
        }
    }
}


#endif