
public class Define
{
    public enum Tilemap
    {
        None,
        Ground,
        Building,
        GroundTemp,
        BuildingTemp,
    }

    public enum WorldObject
    {
        Unknown,
        Citizen,
    }

    public enum TileObject
    {
        LeftRoad,
        RightRoad,
        Gateway,
    }

    public enum BuildType
    {
        Ground,
        Building,
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
