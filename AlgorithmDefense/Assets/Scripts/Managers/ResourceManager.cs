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
        var prefab = Load<GameObject>(path);
        if (!prefab)
        {
            Debug.Log($"[ResourceManager] Failed to load prefab : {path}");
            return null;
        }

        if (prefab.GetComponent<Poolable>())
        {
            return Managers.Pool.Pop(prefab, position, parent).gameObject;
        }

        var go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;

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

    public void Destroy(GameObject go, float time)
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

        Object.Destroy(go, time);
    }
}
