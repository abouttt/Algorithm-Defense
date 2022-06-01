
public static class Define
{
    public enum Class
    {
        None,
        Warrior,
        Archer,
    }

    public enum Tilemap
    {
        Ground,
        Road,
        Building,
        RoadTemp,
        BuildingTemp,
    }

    public enum TileObject
    {
        Road_UD_RuleTile,
        Road_LR_RuleTile,
        StartGateway,
        EndGateway,
        Gateway,
        ClassGiveCenter,
        ClassTrainingCenter,
    }

    public enum WorldObject
    {
        Unknown,
        Citizen,
    }

    public enum Citizen
    {
        None,
        Red,
        Blue,
        Green,
        Yellow,
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
        TU,
        TD,
        TL,
        TR,
        CUL,
        CUR,
        CDL,
        CDR,
        Cross,
    }

    public enum MoveType
    {
        None,
        Left,
        Right,
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
    }

}
