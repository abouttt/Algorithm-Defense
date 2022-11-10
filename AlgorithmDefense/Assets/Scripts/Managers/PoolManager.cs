using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    #region Pool

    public class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; private set; }

        private Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject($"{Original.name}_Root").transform;

            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        public void Push(Poolable poolable)
        {
            if (!poolable)
            {
                Debug.Log("[PoolManager\\Pool] Poolable Object is null.");
                return;
            }

            poolable.transform.parent = Root;
            poolable.IsUsing = false;
            poolable.gameObject.SetActive(false);

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            return Pop(Vector3.zero, parent);
        }

        public Poolable Pop(Vector3 position, Transform parent)
        {
            var poolable = _poolStack.Count > 0 ? _poolStack.Pop() : Create();
            poolable.gameObject.SetActive(true);
            poolable.transform.position = position;
            poolable.transform.parent = parent == null ? Root : parent;
            poolable.IsUsing = true;

            return poolable;
        }

        private Poolable Create()
        {
            var go = Object.Instantiate(Original);
            go.name = Original.name;
            return Util.GetOrAddComponent<Poolable>(go);
        }
    }
    #endregion

    private Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    private Transform _root;
    private List<Pool> _tempPool = new List<Pool>();

    public void Init()
    {
        var root = GameObject.Find("@Pool_Root");
        if (!root)
        {
            root = new GameObject { name = "@Pool_Root" };
        }

        _root = root.transform;
    }

    public void CreatePool(GameObject original, int count = 5)
    {
        if (_pool.ContainsKey(original.name))
        {
            Debug.Log($"[PoolManager] {original.name}_Pool that already exist.");
            return;
        }

        var pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;

        _pool.Add(original.name, pool);
    }

    public void Push(Poolable poolable)
    {
        var name = poolable.gameObject.name;
        if (!_pool.ContainsKey(name))
        {
            Debug.Log($"[PoolManager] Non-existent {name}_Pool.");
            Object.Destroy(poolable.gameObject);
            return;
        }

        _pool[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
       => Pop(original, Vector3.zero, parent);

    public Poolable Pop(GameObject original, Vector3 position, Transform parent = null)
    {
        if (!_pool.ContainsKey(original.name))
        {
            CreatePool(original);
        }

        return _pool[original.name].Pop(position, parent);
    }

    public Pool GetPool(string name)
    {
        if (!_pool.ContainsKey(name))
        {
            return null;
        }

        return _pool[name];
    }

    public List<Pool> GetAllPool()
    {
        _tempPool.Clear();
        foreach (var pool in _pool)
        {
            _tempPool.Add(pool.Value);
        }

        return _tempPool;
    }

    public GameObject GetOriginal(string name)
    {
        var pool = GetPool(name);
        if(pool == null)
        {
            return null;
        }

        return pool.Original;
    }

    public void Clear()
    {
        foreach (Transform child in _root)
        {
            Object.Destroy(child.gameObject);
        }

        _pool.Clear();
    }

    public void ClearPool(GameObject go)
    {
        var poolable = go.GetComponent<Poolable>();
        if (!poolable)
        {
            Debug.Log($"[PoolManager] {go.name} is not a Poolabe Object.");
            return;
        }

        ClearPool(poolable.name);
    }

    public void ClearPool(string name)
    {
        if (!_pool.ContainsKey(name))
        {
            Debug.Log($"[PoolManager] Non-existent {name}_Pool.");
            return;
        }

        foreach (Transform child in _pool[name].Root)
        {
            Object.Destroy(child.gameObject);
        }

        Object.Destroy(_pool[name].Root.gameObject);
        _pool.Remove(name);
    }
}
