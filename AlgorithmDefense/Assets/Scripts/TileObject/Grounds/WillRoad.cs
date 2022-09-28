using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Define;

public class WillRoad : MonoBehaviour
{
    public static List<Vector3Int> WillRoadList = new();

    [field: SerializeField]
    public Define.Road RoadType { get; private set; }
    public int Index = 0;

    public void RefreshTile(Vector3Int cellPos)
    {
        Rule(cellPos);

        if (Index > 0)
        {
            var backCellPos = WillRoadList[Index - 1];
            var go = Managers.Tile.GetTilemap(Define.Tilemap.WillRoad).GetInstantiatedObject(backCellPos);
            if (go)
            {
                go.GetComponent<WillRoad>().Rule(backCellPos);
            }
        }

        if (Index < WillRoadList.Count - 1)
        {
            var frontCellPos = WillRoadList[Index + 1];
            var go = Managers.Tile.GetTilemap(Define.Tilemap.WillRoad).GetInstantiatedObject(frontCellPos);
            if (go)
            {
                go.GetComponent<WillRoad>().Rule(frontCellPos);
            }
        }
    }

    private void Rule(Vector3Int cellPos)
    {
        Tile nextTile = null;

        bool hasBackTile = false;
        bool hasFrontTile = false;

        Vector3Int backGapCellPos = Vector3Int.zero;
        Vector3Int frontGapCellPos = Vector3Int.zero;

        if (Index > 0)
        {
            var backCellPos = WillRoadList[Index - 1];
            backGapCellPos = cellPos - backCellPos;
            hasBackTile = true;
        }

        if (Index < WillRoadList.Count - 1)
        {
            var frontCellPos = WillRoadList[Index + 1];
            frontGapCellPos = cellPos - frontCellPos;
            hasFrontTile = true;
        }

        if (hasBackTile && !hasFrontTile)
        {
            if (backGapCellPos == Vector3Int.up)
            {
                nextTile = Managers.Resource.Load<Tile>($"{Define.WILLROAD_TILE_PATH}Road_BD");
            }
            else if (backGapCellPos == Vector3Int.down)
            {
                nextTile = Managers.Resource.Load<Tile>($"{Define.WILLROAD_TILE_PATH}Road_BU");
            }
            else if (backGapCellPos == Vector3Int.left)
            {
                nextTile = Managers.Resource.Load<Tile>($"{Define.WILLROAD_TILE_PATH}Road_BR");
            }
            else if (backGapCellPos == Vector3Int.right)
            {
                nextTile = Managers.Resource.Load<Tile>($"{Define.WILLROAD_TILE_PATH}Road_BL");
            }
        }

        if (!hasBackTile && hasFrontTile)
        {
            if (frontGapCellPos == Vector3Int.up)
            {
                nextTile = Managers.Resource.Load<Tile>($"{Define.WILLROAD_TILE_PATH}Road_BD");
            }
            else if (frontGapCellPos == Vector3Int.down)
            {
                nextTile = Managers.Resource.Load<Tile>($"{Define.WILLROAD_TILE_PATH}Road_BU");
            }
            else if (frontGapCellPos == Vector3Int.left)
            {
                nextTile = Managers.Resource.Load<Tile>($"{Define.WILLROAD_TILE_PATH}Road_BR");
            }
            else if (frontGapCellPos == Vector3Int.right)
            {
                nextTile = Managers.Resource.Load<Tile>($"{Define.WILLROAD_TILE_PATH}Road_BL");
            }
        }

        if (hasBackTile && hasFrontTile)
        {
            var absBackGapCellPos = Util.GetAbsVector3Int(backGapCellPos);
            var absFrontGapCellPos = Util.GetAbsVector3Int(frontGapCellPos);

            if (absBackGapCellPos == Vector3Int.up &&
                absFrontGapCellPos == Vector3Int.up)
            {
                nextTile = Managers.Resource.Load<Tile>($"{Define.WILLROAD_TILE_PATH}Road_UD");
            }
            else if (absBackGapCellPos == Vector3Int.right &&
                     absFrontGapCellPos == Vector3Int.right)
            {
                nextTile = Managers.Resource.Load<Tile>($"{Define.WILLROAD_TILE_PATH}Road_LR");
            }
            else if ((backGapCellPos == Vector3Int.right && frontGapCellPos == Vector3Int.up) ||
                     (backGapCellPos == Vector3Int.up && frontGapCellPos == Vector3Int.right))
            {
                nextTile = Managers.Resource.Load<Tile>($"{Define.WILLROAD_TILE_PATH}Road_CUL");
            }
            else if ((backGapCellPos == Vector3Int.left && frontGapCellPos == Vector3Int.up) ||
                     (backGapCellPos == Vector3Int.up && frontGapCellPos == Vector3Int.left))
            {
                nextTile = Managers.Resource.Load<Tile>($"{Define.WILLROAD_TILE_PATH}Road_CUR");
            }
            else if ((backGapCellPos == Vector3Int.right && frontGapCellPos == Vector3Int.down) ||
                     (backGapCellPos == Vector3Int.down && frontGapCellPos == Vector3Int.right))
            {
                nextTile = Managers.Resource.Load<Tile>($"{Define.WILLROAD_TILE_PATH}Road_CDL");
            }
            else if ((backGapCellPos == Vector3Int.left && frontGapCellPos == Vector3Int.down) ||
                    (backGapCellPos == Vector3Int.down && frontGapCellPos == Vector3Int.left))
            {
                nextTile = Managers.Resource.Load<Tile>($"{Define.WILLROAD_TILE_PATH}Road_CDR");
            }
        }

        if (nextTile)
        {
            nextTile.color = new Color(0, 0, 1, 0.5f);
            Managers.Tile.SetTile(Define.Tilemap.WillRoad, cellPos, nextTile);
            var go = Managers.Tile.GetTilemap(Define.Tilemap.WillRoad).GetInstantiatedObject(cellPos);
            var road = go.GetComponent<WillRoad>();
            road.Index = Index;
        }
    }
}
