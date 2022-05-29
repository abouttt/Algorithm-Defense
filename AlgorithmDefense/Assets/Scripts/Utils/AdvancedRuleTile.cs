using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Custom Tools/Custom Tiles/Advanced Rule Tile")]
public class AdvancedRuleTile : RuleTile<AdvancedRuleTile.Neighbor>
{
    public TileBase[] tilesToConnect;

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int Null = 3;
        public const int NotNull = 4;
        public const int Other = 5;
        public const int NullOrNotNull = 6;
        public const int ThisOrNull = 7;
        public const int OtherOrNull = 8;
    }

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        switch (neighbor)
        {
            case Neighbor.Null:
                return tile == null;
            case Neighbor.NotNull:
                return tile != null;
            case Neighbor.Other:
                return tilesToConnect.Contains(tile);
            case Neighbor.NullOrNotNull:
                return ((tile == null) || (tile != null));
            case Neighbor.ThisOrNull:
                return ((tile == this) || (tile == null));
            case Neighbor.OtherOrNull:
                return (tilesToConnect.Contains(tile) || (tile == null));
        }
        return base.RuleMatch(neighbor, tile);
    }
}