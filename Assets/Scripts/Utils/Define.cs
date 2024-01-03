using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class Define
{
    public static readonly string CONTENTS_PATH = "Prefabs/Contents/";

    public static readonly string GROUND_PREFAB_PATH = "Prefabs/Ground/";
    public static readonly string CITIZEN_PREFAB_PATH = "Prefabs/Units/Citizens/";
    public static readonly string BATTILE_UNIT_PREFAB_PATH = "Prefabs/Units/BattleUnits/";
    public static readonly string MONSTER_UNIT_PREFAB_PATH = "Prefabs/Units/MonsterUnits/";
    public static readonly string BUILDING_PREFAB_PATH = "Prefabs/TileObject/Buildings/";
    public static readonly string PROJECTILE_PREFAB_PATH = "Prefabs/Projectile/";
    public static readonly string ROAD_PREFAB_PATH = "Prefabs/TileObject/Roads/";
    public static readonly string SKILL_PREFAB_PATH = "Prefabs/Skill/";

    public static readonly string ROAD_TILE_PATH = "Tiles/Roads/";
    public static readonly string RULE_TILE_PATH = "Tiles/RuleTiles/";
    public static readonly string BUILDING_TILE_PATH = "Tiles/Buildings/";

    public static int[] DX = { 0, 1, 0, -1 };
    public static int[] DY = { 1, 0, -1, 0 };

    public enum Tilemap
    {
        Ground,
        Road,
        WillRoad,
        Building,
        Rampart,
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
        WarriorCenter,
        ArcherCenter,
        WizardCenter,
        GoldMine,
        CastleGate,
        Dungeon,
        TutorialWarriorCenter,
        TutorialArcherCenter,
        TutorialWizardCenter,
    }

    public enum Job
    {
        None,
        Warrior,
        Archer,
        Wizard,
    }

    public enum Citizen
    {
        None,
        Red,
        Green,
        Blue,
    }

    public enum Move
    {
        None,
        Up,
        Right,
        Down,
        Left,
    }

    public enum Skill
    {
        None,
        Thunderstroke,
        Madness,
        Heal,
    }

    public enum Sound
    {
        Bgm,
        UI,
        Effect,
        MaxCount,
    }
}
