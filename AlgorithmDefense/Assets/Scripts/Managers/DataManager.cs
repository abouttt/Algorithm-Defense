using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

[Serializable]
public class SerializationList<T>
{
    [SerializeField]
    private List<T> _target;

    public List<T> ToList()
    {
        return _target;
    }
    public SerializationList(List<T> target)
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
        Clear();

        // ≈∏¿œ∏  ¿˙¿Â.

        foreach (var tilemapData in _tilemapDatas)
        {
            var tilemap = Managers.Tile.GetTilemap(tilemapData.Tilemap);
            tilemap.CompressBounds();

            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                var tileBase = tilemap.GetTile(pos);
                if (tileBase)
                {
                    tilemapData.Tiles.Add(tileBase);
                    tilemapData.CellPoses.Add(pos);
                }
            }
        }

        string json = JsonUtility.ToJson(new SerializationList<TilemapSaveData>(_tilemapDatas), true);
        File.WriteAllText($"{Define.SAVE_DATA_PATH.ToString()}{Define.Data.TilemapData}.json", json);

        // Ω√πŒ ¿˙¿Â.

        var pools = Managers.Pool.GetAllPool();
        foreach (var pool in pools)
        {
            var poolables = pool.Root.GetComponentsInChildren<Poolable>();
            foreach (var item in poolables)
            {
                var citizen = item.GetComponent<CitizenController>();
                if (!citizen)
                {
                    continue;
                }

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

        json = JsonUtility.ToJson(new SerializationList<CitizenSaveData>(_citizenDatas), true);
        File.WriteAllText($"{Define.SAVE_DATA_PATH.ToString()}{Define.Data.CitizenData}.json", json);
    }

    public void LoadData()
    {

        // ≈∏¿œ∏  ∑ŒµÂ.

        string json = File.ReadAllText($"{Define.SAVE_DATA_PATH.ToString()}{Define.Data.TilemapData}.json");
        if (!json.Equals(""))
        {
            _tilemapDatas = JsonUtility.FromJson<SerializationList<TilemapSaveData>>(json).ToList();

            foreach (var tilemapData in _tilemapDatas)
            {
                var tilemap = Managers.Tile.GetTilemap(tilemapData.Tilemap);
                tilemap.ClearAllTiles();

                for (int i = 0; i < tilemapData.Tiles.Count; i++)
                {
                    tilemap.SetTile(tilemapData.CellPoses[i], tilemapData.Tiles[i]);
                }
            }
        }

        // Ω√πŒ ∑ŒµÂ.

        json = File.ReadAllText($"{Define.SAVE_DATA_PATH.ToString()}{Define.Data.CitizenData}.json");
        if (!json.Equals(""))
        {
            _citizenDatas = JsonUtility.FromJson<SerializationList<CitizenSaveData>>(json).ToList();

            foreach (var data in _citizenDatas)
            {
                GameObject go = null;

                if (data.Data.JobType == Define.Job.None)
                {
                    go = Managers.Resource.Instantiate($"{Define.CITIZEN_PATH}{data.Data.CitizenType.ToString()}Citizen");
                }
                else
                {
                    go = Managers.Resource.Instantiate($"{Define.BATTILE_UNIT_PATH}{data.Data.JobType.ToString()}Unit");
                }

                go.transform.position = data.Position;
                go.transform.rotation = data.Rotation;
                go.transform.localScale = data.Scale;
                var citizen = go.GetOrAddComponent<CitizenController>();
                citizen.Data = data.Data;
            }
        }
    }

    public void Clear()
    {
        for (int i = 0; i < Enum.GetValues(typeof(Define.Data)).Length; i++)
        {
            File.WriteAllText($"{Define.SAVE_DATA_PATH.ToString()}{(Define.Data)i}.json", "");
        }
    }
}
