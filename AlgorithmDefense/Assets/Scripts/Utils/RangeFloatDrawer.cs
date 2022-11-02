using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(RangeFloat))]
public class RangeFloatDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        {
            float x, w;
            int elementCount = 3;
            for (int i = 0; i < elementCount; i++)
            {
                w = position.width / elementCount;
                x = position.x + w * i;
                switch (i)
                {
                    case 0:
                        EditorGUIUtility.labelWidth = 25;
                        EditorGUI.LabelField(new Rect(x, position.y, w, position.height), property.displayName);
                        break;
                    case 1:
                        EditorGUIUtility.labelWidth = 25;
                        EditorGUI.PropertyField(new Rect(x, position.y, w, position.height), property.FindPropertyRelative("min"));
                        break;
                    case 2:
                        EditorGUIUtility.labelWidth = 25;
                        EditorGUI.PropertyField(new Rect(x, position.y, w, position.height), property.FindPropertyRelative("max"));
                        break;
                }
            }
        }

        EditorGUI.EndProperty();
        EditorGUIUtility.labelWidth = 0;
    }

}
