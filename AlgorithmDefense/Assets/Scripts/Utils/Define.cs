
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
        Road_UD,
        Road_LR,
        TWRoad_U,
        TWRoad_D,
        TWRoad_L,
        TWRoad_R,
        CornerRoad_U,
        CornerRoad_D,
        CornerRoad_L,
        CornerRoad_R,
        CrossRoad,
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
