using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager
{
    private Grid _grid;
    private Dictionary<Define.Tilemap, Tilemap> _tilemaps = new Dictionary<Define.Tilemap, Tilemap>();

    public void Init()
    {
        _grid = UnityEngine.Object.FindObjectOfType<Grid>();

        var names = Enum.GetNames(typeof(Define.Tilemap));
        for (int i = 0; i < names.Length; i++)
        {
            var tilemap = Util.FindChild<Tilemap>(_grid.gameObject, $"Tilemap_{names[i]}", recursive: false);
            if (tilemap != null)
            {
                Define.Tilemap type = (Define.Tilemap)Enum.Parse(typeof(Define.Tilemap), names[i]);
                _tilemaps.Add(type, tilemap);
            }
            else
            {
                Debug.Log($"[TileManager] Failed to find tilemap : {names[i]}");
            }
        }
    }

    public Grid GetGrid() => _grid;

    public Tilemap GetTilemap(Define.Tilemap tilemap) => _tilemaps[tilemap];

    public TileBase GetTile(Define.Tilemap tilemap, Vector3Int cellPos)
        => _tilemaps[tilemap].GetTile(cellPos);

    public TileBase GetTile(Define.Tilemap tilemap, Vector3 pos)
    {
        var cellPos = GetWorldToCell(tilemap, pos);
        return _tilemaps[tilemap].GetTile(cellPos);
    }

    public Vector3Int GetWorldToCell(Define.Tilemap tilemap, Vector3 worldPos)
        => _tilemaps[tilemap].WorldToCell(worldPos);

    public Vector3 GetCellToWorld(Define.Tilemap tilemap, Vector3Int cellPos)
        => _tilemaps[tilemap].CellToWorld(cellPos);

    public Vector3 GetCellCenterToWorld(Define.Tilemap tilemap, Vector3Int cellPos)
        => _tilemaps[tilemap].GetCellCenterWorld(cellPos);

    public Vector3 GetWorldToCellCenterToWorld(Define.Tilemap tilemap, Vector3 worldPos)
    {
        var cellPos = _tilemaps[tilemap].WorldToCell(worldPos);
        return _tilemaps[tilemap].GetCellCenterWorld(cellPos);
    }

    public void SetTile(Define.Tilemap tilemap, Vector3Int cellPos, TileBase tile)
        => _tilemaps[tilemap].SetTile(cellPos, tile);

    public void SetTile(Define.Tilemap tilemap, Vector3 pos, TileBase tile)
    {
        var cellPos = GetWorldToCell(tilemap, pos);
        _tilemaps[tilemap].SetTile(cellPos, tile);
    }
}
