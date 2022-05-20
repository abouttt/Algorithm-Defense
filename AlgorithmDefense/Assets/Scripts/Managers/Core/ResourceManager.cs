using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            var name = path;
            int idx = path.LastIndexOf('/');
            if (idx >= 0)
            {
                name = path.Substring(idx + 1);
            }

            var original = Managers.Pool.GetOriginal(name);
            if (original != null)
            {
                return original as T;
            }
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        return Instantiate(path, Vector3.zero, parent);
    }

    public GameObject Instantiate(string path, Vector3 position, Transform parent = null)
    {
        var original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"[ResourceManager] Failed to load prefab : {path}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
        {
            return Managers.Pool.Pop(original, position, parent).gameObject;
        }

        var go = Object.Instantiate(original, parent);
        go.name = original.name;

        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        var poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }
}
