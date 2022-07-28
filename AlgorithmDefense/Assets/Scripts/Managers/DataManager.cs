using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class Serialization<T>
{
    [SerializeField]
    private List<T> _target;

    public List<T> ToList()
    { 
        return _target;
    }
    public Serialization(List<T> target)
    {
        _target = target;
    }
}

public class DataManager
{
    private List<TilemapSaveData> _tilemapDatas = new List<TilemapSaveData>();
    private List<CitizenSaveData> _citizenDatas = new List<CitizenSaveData>();

    public void Init()
    {
        for (int i = 0; i < Enum.GetValues(typeof(Define.Tilemap)).Length; i++)
        {
            _tilemapDatas.Add(new TilemapSaveData { Tilemap = (Define.Tilemap)i });
        }
    }

    public void SaveData()
    {

        // ≈∏¿œ∏  ¿˙¿Â.

        foreach (var tilemapData in _tilemapDatas)
        {
            var tilemap = Managers.Tile.GetTilemap(tilemapData.Tilemap);
            tilemap.CompressBounds();

            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                var tile = tilemap.GetTile(pos);
                if (tile)
                {
                    tilemapData.Tiles.Add(tile);
                    tilemapData.CellPoses.Add(pos);
                }
            }
        }

        string json = JsonUtility.ToJson(new Serialization<TilemapSaveData>(_tilemapDatas), true);
        File.WriteAllText($"{Define.SAVE_DATA_PATH.ToString()}{Define.Data.TilemapData}.json", json);

        // Ω√πŒ ¿˙¿Â.

        var names = Enum.GetNames(typeof(Define.Citizen));
        foreach (var type in names)
        {
            var pool = Managers.Pool.GetPool($"{type}Citizen");
            if (pool == null)
            {
                continue;
            }

            var poolables = pool.Root.GetComponentsInChildren<Poolable>();
            foreach (var item in poolables)
            {
                if (item.IsUsing)
                {
                    var saveData = new CitizenSaveData
                    {
                        Position = item.transform.position,
                        Rotation = item.transform.rotation,
                        Scale = item.transform.localScale,
                        Data = item.GetComponent<CitizenController>().Data
                    };
                    
                    _citizenDatas.Add(saveData);
                }
            }
        }

        json = JsonUtility.ToJson(new Serialization<CitizenSaveData>(_citizenDatas), true);
        File.WriteAllText($"{Define.SAVE_DATA_PATH.ToString()}{Define.Data.CitizenData}.json", json);
    }

    public void LoadData()
    {

        // ≈∏¿œ∏  ∑ŒµÂ.

        string json = File.ReadAllText($"{Define.SAVE_DATA_PATH.ToString()}{Define.Data.TilemapData}.json");
        _tilemapDatas = JsonUtility.FromJson<Serialization<TilemapSaveData>>(json).ToList();

        foreach (var tilemapData in _tilemapDatas)
        {
            var tilemap = Managers.Tile.GetTilemap(tilemapData.Tilemap);
            tilemap.ClearAllTiles();

            for (int i = 0; i < tilemapData.Tiles.Count; i++)
            {
                tilemap.SetTile(tilemapData.CellPoses[i], tilemapData.Tiles[i]);
            }
        }

        // Ω√πŒ ∑ŒµÂ.

        json = File.ReadAllText($"{Define.SAVE_DATA_PATH.ToString()}{Define.Data.CitizenData}.json");
        _citizenDatas = JsonUtility.FromJson<Serialization<CitizenSaveData>>(json).ToList();

        foreach (var data in _citizenDatas)
        {
            var go = Managers.Resource.Instantiate($"{Define.CITIZEN_PATH}{data.Data.CitizenType.ToString()}Citizen");
            go.transform.position = data.Position;
            go.transform.rotation = data.Rotation;
            go.transform.localScale = data.Scale;
            var citizen = go.GetComponent<CitizenController>();
            citizen.Data = data.Data;
        }
    }

    public void Clear()
    {
        File.WriteAllText($"{Define.SAVE_DATA_PATH.ToString()}{Define.Data.TilemapData}.json", "");
        File.WriteAllText($"{Define.SAVE_DATA_PATH.ToString()}{Define.Data.CitizenData}.json", "");
    }
}
