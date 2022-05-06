using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CitizenSpawner : MonoBehaviour
{
    private static CitizenSpawner s_instance = null;
    public static CitizenSpawner GetInstance { get { init(); return s_instance; } }

    public bool IsSpawning { get; private set; } = false;

    private Tilemap _mainTilemap = null;
    private GameObject _spawnTarget = null;

    [SerializeField]
    private Vector3Int _spawnPosition;
    [SerializeField]
    private Vector3Int _endPosition;
    [SerializeField]
    private float _spawnTime = 5.0f;
    private int _spawnIdx = 0;

    private void Start()
    {
        init();

        _mainTilemap = GameObject.Find("Tilemap_Main").GetComponent<Tilemap>();

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
            _spawnTarget.transform.position = _mainTilemap.GetCellCenterWorld(_spawnPosition);

            yield return new WaitForSeconds(_spawnTime);

            _spawnIdx = ++_spawnIdx < Managers.Game.CitizenSpawnOrderList.Count ? _spawnIdx : 0;
        }
    }

    private void setup()
    {
        var tile = Managers.Resource.Load<Tile>($"Tiles/StartGateway");
        BuildingBuilder.GetInstance.Build(_spawnPosition, tile);
        tile = Managers.Resource.Load<Tile>($"Tiles/EndGateway");
        BuildingBuilder.GetInstance.Build(_endPosition, tile);
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
