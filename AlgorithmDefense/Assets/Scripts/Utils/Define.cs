
public static class Define
{
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

    public enum Road
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

    public enum Class
    {
        None,
        Warrior,
        Archer,
        Wizard,
    }

    public enum ClassTier
    {
        None,
        One,
        Two,
        Three,
    }

    public enum Citizen
    {
        None,
        Red,
        Blue,
        Green,
        Yellow,
    }

    public enum Move
    {
        None,
        Left,
        Right,
        Up,
        Down,
    }

    public enum Build
    {
        Ground,
        Building,
    }

    public enum State
    {
        Idle,
        Moving,
        Die,
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
