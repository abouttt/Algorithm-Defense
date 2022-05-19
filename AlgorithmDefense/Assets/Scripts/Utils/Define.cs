
public class Define
{
    public enum Tilemap
    {
        None,
        Ground,
        Building,
        Road,
        BuildingTemp,
        RoadTemp,
        Global,
    }

    public enum WorldObject
    {
        Unknown,
        Citizen,
    }

    public enum TileObject
    {
        Road_UD_RuleTile,
        Road_LR_RuleTile,
        StartGateway,
        EndGateway,
        Gateway,
    }

    public enum BuildType
    {
        Ground,
        Building,
    }

    public enum RoadType
    {
        UD,
        LR,
        TW_U,
        TW_D,
        TW_L,
        TW_R,
        Corner_U,
        Corner_D,
        Corner_L,
        Corner_R,
        Cross,
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
