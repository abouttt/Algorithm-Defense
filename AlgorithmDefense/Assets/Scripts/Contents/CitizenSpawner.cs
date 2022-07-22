using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CitizenSpawner : MonoBehaviour
{
    private static CitizenSpawner s_instance;
    public static CitizenSpawner GetInstance { get { Init(); return s_instance; } }

    [HideInInspector]
    public int OnCount;
    public (Define.Citizen, bool)[] CitizenSpawnList { get; private set; }
    public bool IsSpawning { get; private set; }

    private Vector3Int _spawnCellPos;
    private float _spawnTime;

    private Define.Citizen _spawnTarget;
    private int _spawnIndex = 0;

    // 인스펙터 실험용 변수
    [SerializeField]
    private bool _onRed;
    [SerializeField]
    private bool _onGreen;
    [SerializeField]
    private bool _onBlue;
    [SerializeField]
    private bool _onYellow;

    private void Start()
    {
        Init();

        CitizenSpawnList = new (Define.Citizen, bool)[4]
        {
            (Define.Citizen.Red, false),
            (Define.Citizen.Green, false),
            (Define.Citizen.Blue, false),
            (Define.Citizen.Yellow, false),
        };
    }

    private void Update()
    {
        if (_onRed)
        {
            SetOnOff(Define.Citizen.Red);
            _onRed = false;
        }
        if (_onGreen)
        {
            SetOnOff(Define.Citizen.Green);
            _onGreen = false;
        }
        if (_onBlue)
        {
            SetOnOff(Define.Citizen.Blue);
            _onBlue = false;
        }
        if (_onYellow)
        {
            SetOnOff(Define.Citizen.Yellow);
            _onYellow = false;
        }
    }

    public void Setup(Vector3Int spawnPos, float spawnTime)
    {
        _spawnCellPos = spawnPos;
        _spawnTime = spawnTime;

        var road = Managers.Resource.Load<RuleTile>($"{Define.RULE_TILE_PATH}RoadRuleTile");
        //Managers.Tile.SetTile(Define.Tilemap.Road, spawnPos, road);
        TileObjectBuilder.GetInstance.Build(road, spawnPos);
    }

    // 시민 종류에 따라 스폰 여부를 결정한다.
    public void SetOnOff(Define.Citizen citizenType)
    {
        for (int i = 0; i < CitizenSpawnList.Length; i++)
        {
            if (CitizenSpawnList[i].Item1 == citizenType)
            {
                CitizenSpawnList[i].Item2 = !CitizenSpawnList[i].Item2;
                OnCount = CitizenSpawnList[i].Item2 ? OnCount + 1 : OnCount - 1;
            }
        }

        if (!IsSpawning && OnCount > 0)
        {
            IsSpawning = true;
            StartCoroutine(SpawnCitizen());
        }
    }

    public IEnumerator SpawnCitizen()
    {
        while (true)
        {
            if (OnCount == 0)
            {
                _spawnIndex = 0;
                IsSpawning = false;
                yield break;
            }

            while (true)
            {
                if (CitizenSpawnList[_spawnIndex].Item2)
                {
                    _spawnTarget = CitizenSpawnList[_spawnIndex].Item1;
                    break;
                }
                else
                {
                    _spawnIndex = ++_spawnIndex < CitizenSpawnList.Length ? _spawnIndex : 0;
                }
            }

            var pos = Managers.Tile.GetCellCenterToWorld(Define.Tilemap.Ground, _spawnCellPos) + new Vector3(0.5f, 0, 0);
            var go = Managers.Resource.Instantiate($"{Define.CITIZEN_PATH}{_spawnTarget.ToString()}Citizen", pos);
            var citizen = go.GetComponent<CitizenController>();
            citizen.MoveType = Define.Move.Right;
            citizen.SetNextDestination();

            yield return new WaitForSeconds(_spawnTime);

            _spawnIndex = ++_spawnIndex < CitizenSpawnList.Length ? _spawnIndex : 0;
        }
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            var go = GameObject.Find("@CitizenSpawner");
            if (go == null)
            {
                go = Util.CreateGameObject<CitizenSpawner>("@CitizenSpawner");
            }

            s_instance = go.GetComponent<CitizenSpawner>();
        }
    }
}
