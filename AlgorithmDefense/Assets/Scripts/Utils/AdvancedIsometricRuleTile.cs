using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

[CreateAssetMenu(menuName = "VinTools/Custom Tiles/Advanced Isometric Rule Tile")]
public class AdvancedIsometricRuleTile : IsometricRuleTile<AdvancedIsometricRuleTile.Neighbor>
{
    public bool alwaysConnect;
    public TileBase[] tilesToConnect;
    public bool checkSelf;
    public Tile groundTile;

    public class Neighbor : IsometricRuleTile.TilingRule.Neighbor
    {
        public const int Any = 3;
        public const int Specified = 4;
        public const int Nothing = 5;
        public const int AnyOrNothing = 6;
        public const int ThisOrNothing = 7;
    }

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        switch (neighbor)
        {
            case Neighbor.This: return Check_This(tile);
            case Neighbor.NotThis: return Check_NotThis(tile);
            case Neighbor.Any: return Check_Any(tile);
            case Neighbor.Specified: return Check_Specified(tile);
            case Neighbor.Nothing: return Check_Nothing(tile);
            case Neighbor.AnyOrNothing: return Check_AnyOrNothing(tile);
            case Neighbor.ThisOrNothing: return Check_ThisOrNothing(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }

    private bool Check_This(TileBase tile)
    {
        if (!alwaysConnect)
        {
            return tile == this;
        }
        else
        {
            return tilesToConnect.Contains(tile) || tile == this;
        }
    }

    private bool Check_NotThis(TileBase tile)
    {
        return tile != this;
    }

    private bool Check_Any(TileBase tile)
    {
        if (checkSelf)
        {
            return tile != groundTile;
        }
        else
        {
            return tile != groundTile && tile != this;
        }
    }

    private bool Check_Specified(TileBase tile)
    {
        return tilesToConnect.Contains(tile);
    }

    private bool Check_Nothing(TileBase tile)
    {
        return tile == groundTile;
    }

    private bool Check_AnyOrNothing(TileBase tile)
    {
        return ((tile != groundTile) || (tile == groundTile));
    }

    private bool Check_ThisOrNothing(TileBase tile)
    {
        return ((tile == this) || (tile == groundTile));
    }
}