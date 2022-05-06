using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance = null;
    private static Managers Instance { get { init(); return s_instance; } }

    #region Contents
    private GameManagerEx _game = new GameManagerEx();
    private TileManager _tile = new TileManager();

    public static GameManagerEx Game { get { return Instance._game; } }
    public static TileManager Tile { get { return Instance._tile; } }
    #endregion

    #region Core
    private InputManager _input = new InputManager();
    private PoolManager _pool = new PoolManager();
    private ResourceManager _resource = new ResourceManager();
    private SceneManagerEx _scene = new SceneManagerEx();
    private SoundManager _sound = new SoundManager();
    private UIManager _ui = new UIManager();

    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI { get { return Instance._ui; } }
    #endregion

    private void Start()
    {
        init();
    }

    private void Update()
    {
        _input.OnUpdate();
    }

    public static void Clear()
    {
        Input.Clear();
        Pool.Clear();
        Scene.Clear();
        Sound.Clear();
        UI.Clear();
    }

    private static void init()
    {
        if (s_instance == null)
        {
            var go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance._pool.Init();
            s_instance._sound.Init();
            s_instance._ui.Init();

            s_instance._game.Init();
            s_instance._tile.Init();
        }
    }
}
