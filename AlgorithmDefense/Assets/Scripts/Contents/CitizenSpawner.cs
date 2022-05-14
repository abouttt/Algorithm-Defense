using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CitizenSpawner : MonoBehaviour
{
    private static CitizenSpawner s_instance = null;
    public static CitizenSpawner GetInstance { get { init(); return s_instance; } }

    public bool IsSpawning { get; private set; } = false;

    private Tilemap _groundTilemap = null;
    private GameObject _spawnTarget = null;

    [SerializeField]
    private Vector3Int _spawnCellPosition;
    [SerializeField]
    private Vector3Int _endCellPosition;
    [SerializeField]
    private float _spawnTime = 5.0f;
    private int _spawnIdx = 0;

    private void Start()
    {
        init();

        _groundTilemap = GameObject.Find("Tilemap_Ground").GetComponent<Tilemap>();

        setup();
    }

    public IEnumerator SpawnCitizen()
    {
        if (IsSpawning)
        {
            yield break;
        }
        else
        {
            IsSpawning = true;
        }

        while (true)
        {
            if (Managers.Game.CitizenSpawnOrderList.Count == 0)
            {
                _spawnIdx = 0;
                _spawnTarget = null;
                IsSpawning = false;
                yield break;
            }

            _spawnTarget = Managers.Game.CitizenSpawnOrderList[_spawnIdx];

            while (_spawnIdx >= 0)
            {
                if (!Managers.Game.CitizenSpawnOrderList.Contains(_spawnTarget))
                {
                    _spawnIdx--;
                    _spawnTarget = Managers.Game.CitizenSpawnOrderList[_spawnIdx];
                }
                else
                {
                    break;
                }
            }

            var name = _spawnTarget.name;
            var citizenName = name.Replace("Button", "Citizen");

            _spawnTarget = Managers.Game.Spawn(Define.WorldObject.Citizen, $"Citizen/{citizenName}");
            _spawnTarget.transform.position = _groundTilemap.GetCellCenterWorld(_spawnCellPosition);

            yield return new WaitForSeconds(_spawnTime);

            _spawnIdx = ++_spawnIdx < Managers.Game.CitizenSpawnOrderList.Count ? _spawnIdx : 0;
        }
    }

    private void setup()
    {
        var tile = Managers.Resource.Load<Tile>("Tiles/StartGateway");
        ObjectBuilder.GetInstance.Build(_spawnCellPosition, tile);

        tile = Managers.Resource.Load<Tile>("Tiles/EndGateway");
        ObjectBuilder.GetInstance.Build(_endCellPosition, tile);

        _spawnCellPosition.z = 1;
        _endCellPosition.z = 1;
    }

    private static void init()
    {
        if (s_instance == null)
        {
            var go = GameObject.Find("@CitizenSpawner");
            if (go == null)
            {
                go = new GameObject { name = "@CitizenSpawner" };
                go.AddComponent<CitizenSpawner>();
            }

            s_instance = go.GetComponent<CitizenSpawner>();
        }
    }
}
