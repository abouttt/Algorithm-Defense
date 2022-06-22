
public static class Define
{
    public enum Class
    {
        None,
        Warrior,
        Archer,
        Wizard,
    }

    public enum ClassTier
    {
        One,
        Two,
        Three,
    }

    public enum Tilemap
    {
        Ground,
        Road,
        Building,
        GroundTemp,
        RoadTemp,
        BuildingTemp,
    }

    public enum TileObject
    {
        Road_RuleTile,
        StartGateway,
        EndGateway,
        Gateway,
        ClassGiveCenter,
        ClassTrainingCenter,
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
        B,
        BU,
        BD,
        BL,
        BR,
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
        GameScene,
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
