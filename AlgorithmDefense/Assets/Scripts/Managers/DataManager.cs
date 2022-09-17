using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

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

    public List<T> ToList() => _target;
}

[Serializable]
public class SerializationArray<T>
{
    [SerializeField]
    private T[] _target;

    public SerializationArray(T[] target)
    {
        _target = target;
    }

    public T[] ToArray() => _target;
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

[Serializable]
public class RuntimeData
{
    public int Gold = 0;
}

public class DataManager
{
    // ∞‘¿” ∑±≈∏¿” µ•¿Ã≈Õ.
    public RuntimeData RuntimeDatas = new RuntimeData();

    public int[] MagicCounts { get; private set; } = new int[Enum.GetValues(typeof(Define.Magic)).Length - 1];
    public int[] BattleUnitCounts { get; private set; } = new int[Enum.GetValues(typeof(Define.Job)).Length - 1];

    // ºº¿Ã∫Í µ•¿Ã≈Õ.

    public Queue<string> GatewaySaveDatas { get; private set; } = new Queue<string>();
    public Queue<string> JobTrainingCenterSaveDatas { get; private set; } = new Queue<string>();
    public Queue<string> GoldMineSaveDatas { get; private set; } = new Queue<string>();

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

                    if (tilemapData.Tilemap == Define.Tilemap.Building)
                    {
                        var go = tilemap.GetInstantiatedObject(pos);
                        if (go)
                        {
                            go.GetComponent<BaseBuilding>().CreateSaveData();
                        }
                    }
                }
            }
        }

        string json = JsonUtility.ToJson(new SerializationList<TilemapSaveData>(_tilemapDatas), true);
        SaveDataToFile($"{Define.STREAM_SAVE_DATA_PATH}{Define.Data.Tilemap}.json", json);

        json = JsonUtility.ToJson(new SerializationList<string>(GatewaySaveDatas), true);
        SaveDataToFile($"{Define.STREAM_SAVE_DATA_PATH}{Define.Data.Gateway}.json", json);

        json = JsonUtility.ToJson(new SerializationList<string>(JobTrainingCenterSaveDatas), true);
        SaveDataToFile($"{Define.STREAM_SAVE_DATA_PATH}{Define.Data.JobTraining}.json", json);

        json = JsonUtility.ToJson(new SerializationList<string>(GoldMineSaveDatas), true);
        SaveDataToFile($"{Define.STREAM_SAVE_DATA_PATH}{Define.Data.OreMine}.json", json);

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
        SaveDataToFile($"{Define.STREAM_SAVE_DATA_PATH}{Define.Data.Citizen}.json", json);

        // ∑±≈∏¿” µ•¿Ã≈Õ ¿˙¿Â.

        json = JsonUtility.ToJson(RuntimeDatas);
        SaveDataToFile($"{Define.STREAM_SAVE_DATA_PATH}{Define.Data.Runtime}.json", json);

        json = JsonUtility.ToJson(new SerializationArray<int>(MagicCounts), true);
        SaveDataToFile($"{Define.STREAM_SAVE_DATA_PATH}{Define.Data.Magic}.json", json);

        json = JsonUtility.ToJson(new SerializationArray<int>(BattleUnitCounts), true);
        SaveDataToFile($"{Define.STREAM_SAVE_DATA_PATH}{Define.Data.BattleUnit}.json", json);
    }

    public void LoadData()
    {

        // ≈∏¿œ∏  ∑ŒµÂ.

<<<<<<< HEAD
<<<<<<< HEAD
        string json = LoadDataFromFile($"{Define.STREAM_SAVE_DATA_PATH}{Define.Data.Tilemap}.json");
=======
        string json = LoadDataFromFile($"{Define.STREAM_SAVE_DATA_PATH.ToString()}{Define.Data.Tilemap}.json");
>>>>>>> parent of e5ebc00f (Í∏∏ ÏÇ≠Ï†ú ÏàòÏ†ï.)
=======
        string json = LoadDataFromFile($"{Define.STREAM_SAVE_DATA_PATH.ToString()}{Define.Data.Tilemap}.json");
>>>>>>> parent of e5ebc00f (Í∏∏ ÏÇ≠Ï†ú ÏàòÏ†ï.)
        if (json == null ||
            json.Equals(""))
        {
            Debug.Log("No Exist Save Data.");
            return;
        }

        _tilemapDatas = JsonUtility.FromJson<SerializationList<TilemapSaveData>>(json).ToList();

<<<<<<< HEAD
<<<<<<< HEAD
        json = LoadDataFromFile($"{Define.STREAM_SAVE_DATA_PATH}{Define.Data.Gateway}.json");
        GatewaySaveDatas = new Queue<string>(JsonUtility.FromJson<SerializationList<string>>(json).ToList());

        json = LoadDataFromFile($"{Define.STREAM_SAVE_DATA_PATH}{Define.Data.JobTraining}.json");
=======
        json = LoadDataFromFile($"{Define.STREAM_SAVE_DATA_PATH.ToString()}{Define.Data.Gateway}.json");
        GatewaySaveDatas = new Queue<string>(JsonUtility.FromJson<SerializationList<string>>(json).ToList());

        json = LoadDataFromFile($"{Define.STREAM_SAVE_DATA_PATH.ToString()}{Define.Data.JobTraining}.json");
>>>>>>> parent of e5ebc00f (Í∏∏ ÏÇ≠Ï†ú ÏàòÏ†ï.)
=======
        json = LoadDataFromFile($"{Define.STREAM_SAVE_DATA_PATH.ToString()}{Define.Data.Gateway}.json");
        GatewaySaveDatas = new Queue<string>(JsonUtility.FromJson<SerializationList<string>>(json).ToList());

        json = LoadDataFromFile($"{Define.STREAM_SAVE_DATA_PATH.ToString()}{Define.Data.JobTraining}.json");
>>>>>>> parent of e5ebc00f (Í∏∏ ÏÇ≠Ï†ú ÏàòÏ†ï.)
        JobTrainingCenterSaveDatas = new Queue<string>(JsonUtility.FromJson<SerializationList<string>>(json).ToList());

        foreach (var tilemapData in _tilemapDatas)
        {
            var tilemap = Managers.Tile.GetTilemap(tilemapData.Tilemap);
            tilemap.ClearAllTiles();

            for (int i = 0; i < tilemapData.Tiles.Count; i++)
            {
                tilemap.SetTile(tilemapData.CellPoses[i], tilemapData.Tiles[i]);

                if (tilemapData.Tilemap == Define.Tilemap.Building)
                {
                    var go = tilemap.GetInstantiatedObject(tilemapData.CellPoses[i]);
                    if (go)
                    {
                        go.GetComponent<BaseBuilding>().LoadSaveData();
                    }
                }
            }
        }

        // Ω√πŒ ∑ŒµÂ.

        json = LoadDataFromFile($"{Define.STREAM_SAVE_DATA_PATH.ToString()}{Define.Data.Citizen}.json");
        _citizenDatas = JsonUtility.FromJson<SerializationList<CitizenSaveData>>(json).ToList();

        foreach (var data in _citizenDatas)
        {
            GameObject go = null;

            if (data.Data.JobType == Define.Job.None)
            {
<<<<<<< HEAD
<<<<<<< HEAD
                go = Managers.Resource.Instantiate($"{Define.CITIZEN_PATH}{data.Data.CitizenType}Citizen");
            }
            else
            {
                go = Managers.Resource.Instantiate($"{Define.BATTILE_UNIT_PATH}{data.Data.JobType}Unit");
=======
                go = Managers.Resource.Instantiate($"{Define.CITIZEN_PATH}{data.Data.CitizenType.ToString()}Citizen");
            }
            else
            {
                go = Managers.Resource.Instantiate($"{Define.BATTILE_UNIT_PATH}{data.Data.JobType.ToString()}Unit");
>>>>>>> parent of e5ebc00f (Í∏∏ ÏÇ≠Ï†ú ÏàòÏ†ï.)
=======
                go = Managers.Resource.Instantiate($"{Define.CITIZEN_PATH}{data.Data.CitizenType.ToString()}Citizen");
            }
            else
            {
                go = Managers.Resource.Instantiate($"{Define.BATTILE_UNIT_PATH}{data.Data.JobType.ToString()}Unit");
>>>>>>> parent of e5ebc00f (Í∏∏ ÏÇ≠Ï†ú ÏàòÏ†ï.)
            }

            if (!go)
            {
                Debug.Log("[DataManager] Citizen GameObject is null");
            }

            go.transform.position = data.Position;
            go.transform.rotation = data.Rotation;
            go.transform.localScale = data.Scale;
            var citizen = go.GetOrAddComponent<CitizenController>();
            citizen.Data = data.Data;
        }

        // ∑±≈∏¿” µ•¿Ã≈Õ ∑ŒµÂ.

        json = LoadDataFromFile($"{Define.STREAM_SAVE_DATA_PATH}{Define.Data.Runtime}.json");
        RuntimeDatas= JsonUtility.FromJson<RuntimeData>(json);

        json = LoadDataFromFile($"{Define.STREAM_SAVE_DATA_PATH}{Define.Data.Magic}.json");
        MagicCounts = JsonUtility.FromJson<SerializationArray<int>>(json).ToArray();

        json = LoadDataFromFile($"{Define.STREAM_SAVE_DATA_PATH}{Define.Data.BattleUnit}.json");
        BattleUnitCounts = JsonUtility.FromJson<SerializationArray<int>>(json).ToArray();
    }

    public void Clear()
    {
        for (int i = 0; i < Enum.GetValues(typeof(Define.Data)).Length; i++)
        {
            SaveDataToFile($"{Define.STREAM_SAVE_DATA_PATH.ToString()}{(Define.Data)i}.json", "");
        }
    }

    private void SaveDataToFile(string path, string jsonData)
    {
        var formatter = new BinaryFormatter();
        using (var stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, jsonData);
        }
    }

    private string LoadDataFromFile(string path)
    {
        if (File.Exists(path))
        {
            var formatter = new BinaryFormatter();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return formatter.Deserialize(stream) as string;
            }
        }
        else
        {
            Debug.Log($"[DataManager] wrong path : {path}");
        }

        return null;
    }
}
