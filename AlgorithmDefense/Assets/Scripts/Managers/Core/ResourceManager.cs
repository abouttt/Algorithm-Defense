using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            var name = path;
            int index = path.LastIndexOf('/');
            if (index >= 0)
            {
                name = path.Substring(index + 1);
            }

            var original = Managers.Pool.GetOriginal(name);
            if (original)
            {
                return original as T;
            }
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
       => Instantiate(path, Vector3.zero, parent);

    public GameObject Instantiate(string path, Vector3 position, Transform parent = null)
    {
        var original = Load<GameObject>(path);
        if (!original)
        {
            Debug.Log($"[ResourceManager] Failed to load prefab : {path}");
            return null;
        }

        if (original.GetComponent<Poolable>())
        {
            return Managers.Pool.Pop(original, position, parent).gameObject;
        }

        var go = Object.Instantiate(original, parent);
        go.name = original.name;

        return go;
    }

    public void Destroy(GameObject go)
    {
        if (!go)
        {
            return;
        }

        var poolable = go.GetComponent<Poolable>();
        if (poolable)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }
}
