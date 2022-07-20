using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public enum Road
    {
        B,
        BD,
        BL,
        BR,
        BU,
        CDL,
        CDR,
        CUL,
        CUR,
        LR,
        TD,
        TL,
        TR,
        TU,
        UD,
        Cross,
    }

    public enum Building
    {
        
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
        Green,
        Blue,
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

    public enum State
    {
        Idle,
        Moving,
        Attack,
        Die,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }
}
