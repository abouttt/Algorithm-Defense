using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager
{
    public GameScene Setting;

    public bool HasOreMine
    {
        get => _hasGoldMine;
        set
        {
            _hasGoldMine = value;
            if (!_hasGoldMine)
            {
                CreateGoldOnField();
            }
        }
    }

    public bool HasSawmill
    {
        get => _hasSawmill;
        set
        {
            _hasSawmill = value;
            if (!_hasSawmill)
            {
                CreateWoodOnField();
            }
        }
    }

    private bool _hasGoldMine = false;
    private bool _hasSawmill = false;

    public void Init()
    {
        Setting = GameObject.FindObjectOfType<GameScene>();

        CreateGoldOnField();
        CreateWoodOnField();
    }

    public void CreateGoldOnField()
    {
        while (true)
        {
            var cellPos = GetEmptyCellPosition(Define.Tilemap.Building);
            if (!Managers.Tile.GetTile(Define.Tilemap.Item, cellPos))
            {
                var tile = Managers.Resource.Load<Tile>($"{Define.ITEM_TILE_PATH}{Define.Item.Ore}");
                Managers.Tile.SetTile(Define.Tilemap.Item, cellPos, tile);
                break;
            }
        }
    }

    public void CreateWoodOnField()
    {
        while (true)
        {
            var cellPos = GetEmptyCellPosition(Define.Tilemap.Building);
            if (!Managers.Tile.GetTile(Define.Tilemap.Item, cellPos))
            {
                var tile = Managers.Resource.Load<Tile>($"{Define.ITEM_TILE_PATH}{Define.Item.Wood}");
                Managers.Tile.SetTile(Define.Tilemap.Item, cellPos, tile);
                break;
            }
        }
    }

    public void CreateGoldMine()
    {
        var cellPos = GetEmptyCellPosition(Define.Tilemap.Building);
        var tile = Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}{Define.Building.OreMine}");
        TileObjectBuilder.GetInstance.Build(tile, cellPos);
    }

    public void CreateSawmill()
    {
        var cellPos = GetEmptyCellPosition(Define.Tilemap.Building);
        var tile = Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}{Define.Building.Sawmill}");
        TileObjectBuilder.GetInstance.Build(tile, cellPos);
    }

    private Vector3Int GetEmptyCellPosition(Define.Tilemap tilemap)
    {
        while (true)
        {
            int x = UnityEngine.Random.Range(Setting.StartPosition.x + 1, Setting.RampartWidth - 1);
            int y = UnityEngine.Random.Range(Setting.StartPosition.y + 1, Setting.RampartHeight - 1);

            var tile = Managers.Tile.GetTile(tilemap, new Vector3(x, y, 0));
            if (!tile)
            {
                return new Vector3Int(x, y, 0);
            }
        }
    }
}
