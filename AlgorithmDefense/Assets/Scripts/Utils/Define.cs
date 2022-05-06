
public class Define
{
    public enum Tilemap
    {
        Ground,
        Main,
        Temp,
    }

    public enum WorldObject
    {
        Unknown,
        Citizen,
    }

    public enum Building
    {
        Unknown,
        Gateway,
    }

    public enum Citizen
    {
        None,
        Red,
        Blue,
        Green,
        Yellow,
    }

    public enum MoveType
    {
        None,
        Right,
        Left,
        Up,
        Down,
    }

    public enum State
    {
        Idle,
        Moving,
        Die,
    }

    public enum Scene
    {
        Unknown,
        Game,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        SingleClick,
        DoubleClick,
    }
}
