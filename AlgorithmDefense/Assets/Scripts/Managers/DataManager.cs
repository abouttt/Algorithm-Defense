using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using Newtonsoft.Json;

[Serializable]
public class SerializationList<T>
{
    [SerializeField]
    private List<T> _target;

    public SerializationList(List<T> target)
    {
        _target = target;
    }

    public SerializationList(Queue<T> target)
    {
        _target = new List<T>(target);
    }

    public List<T> ToList()
    {
        return _target;
    }
}

[Serializable]
public class SerializationDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> _keys;
    [SerializeField]
    private List<TValue> _values;

    private Dictionary<TKey, TValue> _target;

    public SerializationDictionary(Dictionary<TKey, TValue> target)
    {
        _target = target;
    }

    public void OnBeforeSerialize()
    {
        _keys = new List<TKey>(_target.Keys);
        _values = new List<TValue>(_target.Values);
    }

    public void OnAfterDeserialize()
    {
        var count = Math.Min(_keys.Count, _values.Count);
        _target = new Dictionary<TKey, TValue>(count);
        for (var i = 0; i < count; i++)
        {
            _target.Add(_keys[i], _values[i]);
        }
    }

    public Dictionary<TKey, TValue> ToDictionary() => _target;
}

[Serializable]
public class SerializationQueue<T> : ISerializationCallbackReceiver
{
    [SerializeField]
    private List<T> _items;

    private Queue<T> _target;

    public SerializationQueue(Queue<T> target)
    {
        _target = target;
    }

    public void OnBeforeSerialize()
    {
        _items = new List<T>(_target);
    }

    public void OnAfterDeserialize()
    {
        _target = new Queue<T>(_items);
    }

    public Queue<T> ToQueue() => _target;
}

public class DataManager
{
    private List<TilemapSaveData> _tilemapDatas = new List<TilemapSaveData>();
    private List<CitizenSaveData> _citizenDatas = new List<CitizenSaveData>();
    private Queue<string> _gatewayDatas = new Queue<string>();
    private Queue<string> _jobTrainingCenterDatas = new Queue<string>();

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

                    if (tileBase.name.Equals("Gateway"))
                    {
                        var gw = tilemap.GetInstantiatedObject(pos).GetComponent<Gateway>();
                        _gatewayDatas.Enqueue(gw.GetSaveData());
                    }
                    else if (tileBase.name.Contains("JobTrainingCenter"))
                    {
                        var jtc = tilemap.GetInstantiatedObject(pos).GetComponent<JobTrainingCenter>();
                        _jobTrainingCenterDatas.Enqueue(jtc.GetSaveData());
                    }
                }
            }
        }

        string json = JsonUtility.ToJson(new SerializationList<TilemapSaveData>(_tilemapDatas), true);
        File.WriteAllText($"{Define.SAVE_DATA_PATH.ToString()}{Define.Data.TilemapData}.json", json);

        json = JsonUtility.ToJson(new SerializationList<string>(_gatewayDatas), true);
        File.WriteAllText($"{Define.SAVE_DATA_PATH.ToString()}{Define.Data.GatewayData}.json", json);

        json = JsonUtility.ToJson(new SerializationList<string>(_jobTrainingCenterDatas), true);
        File.WriteAllText($"{Define.SAVE_DATA_PATH.ToString()}{Define.Data.JobTrainingData}.json", json);

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

            json = File.ReadAllText($"{Define.SAVE_DATA_PATH.ToString()}{Define.Data.GatewayData}.json");
            _gatewayDatas = new Queue<string>(JsonUtility.FromJson<SerializationList<string>>(json).ToList());

            json = File.ReadAllText($"{Define.SAVE_DATA_PATH.ToString()}{Define.Data.JobTrainingData}.json");
            _jobTrainingCenterDatas = new Queue<string>(JsonUtility.FromJson<SerializationList<string>>(json).ToList());

            foreach (var tilemapData in _tilemapDatas)
            {
                var tilemap = Managers.Tile.GetTilemap(tilemapData.Tilemap);
                tilemap.ClearAllTiles();

                for (int i = 0; i < tilemapData.Tiles.Count; i++)
                {
                    tilemap.SetTile(tilemapData.CellPoses[i], tilemapData.Tiles[i]);

                    if (tilemapData.Tiles[i].name.Equals("Gateway"))
                    {
                        var gw = tilemap.GetInstantiatedObject(tilemapData.CellPoses[i]).GetComponent<Gateway>();
                        gw.LoadSaveData(_gatewayDatas.Dequeue());
                    }
                    else if (tilemapData.Tiles[i].name.Contains("JobTrainingCenter"))
                    {
                        var jtc = tilemap.GetInstantiatedObject(tilemapData.CellPoses[i]).GetComponent<JobTrainingCenter>();
                        jtc.LoadSaveData(_jobTrainingCenterDatas.Dequeue());
                    }
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
