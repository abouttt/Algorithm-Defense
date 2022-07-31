using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    public static readonly string CONTENTS_PATH = "Prefabs/Contents/";
    public static readonly string CITIZEN_PATH = "Prefabs/Units/Citizens/";
    public static readonly string BATTILE_UNIT_PATH = "Prefabs/Units/BattleUnits/";
    public static readonly string BUILDING_PREFAB_PATH = "Prefabs/TileObject/Buildings/";
    public static readonly string ROAD_PREFAB_PATH = "Prefabs/TileObject/Roads/";
    public static readonly string ROAD_TILE_PATH = "Tiles/Roads/";
    public static readonly string RULE_TILE_PATH = "Tiles/RuleTiles/";
    public static readonly string BUILDING_TILE_PATH = "Tiles/Buildings/";
    public static readonly string SAVE_DATA_PATH = $"{Application.dataPath}/Resources/SaveDatas/";
    
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
        JobTrainingCenter,
    }

    public enum Job
    {
        None,
        Warrior,
        Archer,
        Wizard,
        Golem,
        Sniper,
        FreezeWizard,
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

    public enum Data
    {
        TilemapData,
        CitizenData,
        GatewayData,
        JobTrainingData,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }
}
