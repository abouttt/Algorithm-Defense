using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR

using UnityEditor;

public class UnitSpriteSetting : EditorWindow
{
    private GameObject _prefab;

    [MenuItem("Custom Editor/Unit Sprite Setting")]
    private static void Init()
    {
        var window = (UnitSpriteSetting)GetWindow(typeof(UnitSpriteSetting));
        window.minSize = new Vector2(400, 70);
        window.maxSize = new Vector2(400, 70);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        _prefab = EditorGUILayout.ObjectField("Unit Prefab", _prefab, typeof(GameObject), false) as GameObject;

        GUILayout.Space(10);
        if (GUILayout.Button("RUN"))
        {
            if (!_prefab)
            {
                return;
            }

            var material = Resources.Load<Material>("Materials/SpriteDiffuse");
            var srArr = _prefab.GetComponentsInChildren<SpriteRenderer>();

            foreach (var sr in srArr)
            {
                sr.material = material;
            }

            AssetDatabase.SaveAssets();

            _prefab = null;
        }
    }
}


#endif