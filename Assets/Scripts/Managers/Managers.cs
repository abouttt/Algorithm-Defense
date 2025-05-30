using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance;
    private static Managers Instance { get { Init(); return s_instance; } }

    private GameManager _game = new GameManager();
    private PoolManager _pool = new PoolManager();
    private ResourceManager _resource = new ResourceManager();
    private SoundManager _sound = new SoundManager();

    public static GameManager Game { get { return Instance._game; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SoundManager Sound { get { return Instance._sound; } }

    private void Start()
    {
        Init();
    }

    public static void Clear()
    {
        Pool.Clear();
        Sound.Clear();
        Game.Clear();
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

            s_instance._pool.Init();
            s_instance._sound.Init();
            s_instance._game.Init();
        }
    }
}
