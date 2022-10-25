using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    private static TileManager s_instance;
    public static TileManager GetInstance { get { Init(); return s_instance; } }

    private Grid _grid;
    private Dictionary<Define.Tilemap, Tilemap> _tilemaps = new();
    private Dictionary<Define.Road, TileBase> _roadTiles = new();
    private Dictionary<Define.Building, TileBase> _buildingTiles = new();

    private void Awake()
    {
        _grid = FindObjectOfType<Grid>();

        InitTilemap();
        InitTileObject();
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

    public void SetTile(Define.Tilemap tilemap, Vector3Int cellPos, Define.Road road)
        => _tilemaps[tilemap].SetTile(cellPos, _roadTiles[road]);

    public void SetTile(Define.Tilemap tilemap, Vector3Int cellPos, Define.Building building)
        => _tilemaps[tilemap].SetTile(cellPos, _buildingTiles[building]);

    private void InitTilemap()
    {
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

    private void InitTileObject()
    {
        var buildingNames = Enum.GetNames(typeof(Define.Building));
        foreach (var buildingName in buildingNames)
        {
            var buildingTile = Instantiate(Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}{buildingName}"));
            buildingTile.gameObject = Managers.Resource.Load<GameObject>($"{Define.BUILDING_PREFAB_PATH}{buildingName}");
            _buildingTiles.Add((Define.Building)Enum.Parse(typeof(Define.Building), buildingName), buildingTile);
        }

        var roadNames = Enum.GetNames(typeof(Define.Road));
        foreach (var roadName in roadNames)
        {
            var roadTile = Instantiate(Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_{roadName}"));
            roadTile.gameObject = Managers.Resource.Load<GameObject>($"{Define.ROAD_PREFAB_PATH}Road_{roadName}");
            _roadTiles.Add((Define.Road)Enum.Parse(typeof(Define.Road), roadName), roadTile);
        }
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            var go = GameObject.Find("@TileManager");
            if (!go)
            {
                go = Util.CreateGameObject<TileManager>("@TileManager");
            }

            s_instance = go.GetComponent<TileManager>();
        }
    }
}
