using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR

public class ArrayElementNamedAttribute : PropertyAttribute
{
    public readonly string[] Names;

    public ArrayElementNamedAttribute(string[] names)
    {
        Names = names;
    }

    public ArrayElementNamedAttribute(Type enumType) 
    {
        Names = Enum.GetNames(enumType); 
    }
}




[CustomPropertyDrawer(typeof(ArrayElementNamedAttribute))]
public class ArrayElementDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(rect, label, property);

        try
        {
            var path = property.propertyPath;
            int pos = int.Parse(path.Split('[').LastOrDefault().TrimEnd(']'));
            EditorGUI.PropertyField(
                rect, 
                property, 
                new GUIContent(ObjectNames.NicifyVariableName(((ArrayElementNamedAttribute)attribute).Names[pos])), true);
        }
        catch
        {
            EditorGUI.PropertyField(rect, property, label, true);
        }

        EditorGUI.EndProperty();
    }
}

#endif

