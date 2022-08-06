using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance;
    private static Managers Instance { get { Init(); return s_instance; } }

    private DataManager _data = new DataManager();
    private PoolManager _pool = new PoolManager();
    private ResourceManager _resource = new ResourceManager();
    private SoundManager _sound = new SoundManager();
    private TileManager _tile = new TileManager();

    public static DataManager Data { get { return Instance._data; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static TileManager Tile { get { return Instance._tile; } }

    private void Start()
    {
        Init();
    }

    public static void Clear()
    {
        Data.Clear();
        Pool.Clear();
        Sound.Clear();
    }

    private static void Init()
    {
        if (!s_instance)
        {
            var go = GameObject.Find("@Managers");
            if (!go)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance._tile.Init();
            s_instance._data.Init();
            s_instance._pool.Init();
            s_instance._sound.Init();
        }
    }
}
