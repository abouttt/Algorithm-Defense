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
    public Queue<string> GatewaySaveDatas = new Queue<string>();
    public Queue<string> GatewayWithCountSaveDatas = new Queue<string>();
    public Queue<string> JobTrainingCenterSaveDatas = new Queue<string>();

    private List<TilemapSaveData> _tilemapDatas = new List<TilemapSaveData>();
    private List<CitizenSaveData> _citizenDatas = new List<CitizenSaveData>();

    private List<Define.Citizen> _citizenSpawnerSaveData = new List<Define.Citizen>();

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

        // 타일맵 저장.

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
        SaveDataToFile($"{Define.STREAM_SAVE_DATA_PATH.ToString()}{Define.Data.TilemapData}.json", json);

        json = JsonUtility.ToJson(new SerializationList<string>(GatewaySaveDatas), true);
        SaveDataToFile($"{Define.STREAM_SAVE_DATA_PATH.ToString()}{Define.Data.GatewayData}.json", json);

        json = JsonUtility.ToJson(new SerializationList<string>(GatewayWithCountSaveDatas), true);
        SaveDataToFile($"{Define.STREAM_SAVE_DATA_PATH.ToString()}{Define.Data.GatewayWithCountData}.json", json);

        json = JsonUtility.ToJson(new SerializationList<string>(JobTrainingCenterSaveDatas), true);
        SaveDataToFile($"{Define.STREAM_SAVE_DATA_PATH.ToString()}{Define.Data.JobTrainingData}.json", json);

        // 시민 저장.

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
        SaveDataToFile($"{Define.STREAM_SAVE_DATA_PATH.ToString()}{Define.Data.CitizenData}.json", json);

        // 시민 스포너 저장.
        var citizenSpawnerList = CitizenSpawner.GetInstance.CitizenSpawnList;
        for (int i = 0; i < citizenSpawnerList.Length; i++)
        {
            if (citizenSpawnerList[i].Item2)
            {
                _citizenSpawnerSaveData.Add(citizenSpawnerList[i].Item1);
            }
        }
        json = JsonUtility.ToJson(new SerializationList<Define.Citizen>(_citizenSpawnerSaveData), true);
        SaveDataToFile($"{Define.STREAM_SAVE_DATA_PATH.ToString()}{Define.Data.CitizenSpawnerData}.json", json);
    }

    public void LoadData()
    {

        // 타일맵 로드.

        string json = LoadDataFromFile($"{Define.STREAM_SAVE_DATA_PATH.ToString()}{Define.Data.TilemapData}.json");
        if (json == null ||
            json.Equals(""))
        {
            Debug.Log("No Exist Save Data.");
            return;
        }

        _tilemapDatas = JsonUtility.FromJson<SerializationList<TilemapSaveData>>(json).ToList();

        json = LoadDataFromFile($"{Define.STREAM_SAVE_DATA_PATH.ToString()}{Define.Data.GatewayData}.json");
        GatewaySaveDatas = new Queue<string>(JsonUtility.FromJson<SerializationList<string>>(json).ToList());

        json = LoadDataFromFile($"{Define.STREAM_SAVE_DATA_PATH.ToString()}{Define.Data.GatewayWithCountData}.json");
        GatewayWithCountSaveDatas = new Queue<string>(JsonUtility.FromJson<SerializationList<string>>(json).ToList());

        json = LoadDataFromFile($"{Define.STREAM_SAVE_DATA_PATH.ToString()}{Define.Data.JobTrainingData}.json");
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

        // 시민 로드.

        json = LoadDataFromFile($"{Define.STREAM_SAVE_DATA_PATH.ToString()}{Define.Data.CitizenData}.json");
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

        // 시민 스포너 로드.

        json = LoadDataFromFile($"{Define.STREAM_SAVE_DATA_PATH.ToString()}{Define.Data.CitizenSpawnerData}.json");
        _citizenSpawnerSaveData = JsonUtility.FromJson<SerializationList<Define.Citizen>>(json).ToList();
        for (int i = 0; i < _citizenSpawnerSaveData.Count; i++)
        {
            CitizenSpawner.GetInstance.SetOnOff(_citizenSpawnerSaveData[i]);
        }
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
