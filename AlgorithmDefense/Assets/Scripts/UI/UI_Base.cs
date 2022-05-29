using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _UIObjects = new Dictionary<Type, UnityEngine.Object[]>();
    public abstract void Init();

    private void Start()
    {
        Init();
    }

    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type)
    {
        var evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            default:
                Debug.Log($"[UI_Base] Non-existent {type}_Handler.");
                break;
        }
    }

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        var names = Enum.GetNames(type);
        var objects = new UnityEngine.Object[names.Length];
        _UIObjects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = Util.FindChild(gameObject, names[i], recursive: true);
            }
            else
            {
                objects[i] = Util.FindChild<T>(gameObject, names[i], recursive: true);
            }

            if (objects[i] == null)
            {
                Debug.Log($"[UI_Base] Failed to bind({names[i]})");
            }
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (!_UIObjects.TryGetValue(typeof(T), out objects))
        {
            return null;
        }

        return objects[idx] as T;
    }

    protected T[] GetAll<T>() where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (!_UIObjects.TryGetValue(typeof(T), out objects))
        {
            return null;
        }

        var castedObject = new T[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            castedObject[i] = objects[i] as T;
        }

        return castedObject;
    }

    protected int Count<T>() where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (!_UIObjects.TryGetValue(typeof(T), out objects))
        {
            return 0;
        }

        return objects.Length;
    }

    protected GameObject GetObject(int idx) => Get<GameObject>(idx);
    protected TextMeshProUGUI GetText(int idx) => Get<TextMeshProUGUI>(idx);
    protected Button GetButton(int idx) => Get<Button>(idx);
    protected Image GetImage(int idx) => Get<Image>(idx);
}
