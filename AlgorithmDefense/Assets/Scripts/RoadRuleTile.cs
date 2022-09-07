using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "2D/Tiles/Custom Tiles/RoadRuleTile")]
public class RoadRuleTile : RuleTile<RoadRuleTile.Neighbor>
{
    public bool customField;

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int SameGroup = 3;
    }

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        switch (neighbor)
        {
            case Neighbor.SameGroup: return IsSameGroup(tile);
        }

        return base.RuleMatch(neighbor, tile);
    }

    bool IsSameGroup(TileBase tile)
    {
        
        return false;
    }
}