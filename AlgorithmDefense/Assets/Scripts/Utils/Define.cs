using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    public static readonly string CONTENTS_PATH = "Prefabs/Contents/";
    public static readonly string CITIZEN_PATH = "Prefabs/Citizens/";
    public static readonly string BUILDING_PATH = "Prefabs/Buildings/";
    public static readonly string ROAD_TILE_PATH = "Tiles/Roads/";
    public static readonly string RULE_TILE_PATH = "Tiles/RuleTiles/";
    public static readonly string BUILDING_TILE_PATH = "Tiles/Buildings/";

    public enum Tilemap
    {
        Ground,
        Road,
        Building,
        Temp,
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
        Gateway,
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
