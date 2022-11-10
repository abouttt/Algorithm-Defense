    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameScene : MonoBehaviour
{
    [System.Serializable]
    public struct StageLineData
    {
        public bool First;
        public bool Second;
        public bool Third;
    }

    [Header("[게임 초기 설정] - 씬 시작 시 1회만 동작한다.")]

    [Header("[카메라 설정]")]
    public float CameraX;
    public float CameraY;
    public float CameraZ;
    public float CameraSize;

    [Header("[성벽 생성]")]
    public Vector3Int StartPosition;
    public int RampartWidth;
    public int RampartHeight;

    [Header("[시민 스폰 설정]")]
    public Vector3Int SpawnCellPos;
    public float SpawnTime;

    [Header("[전투 라인 생성]")]
    public int BattleLineLength;
    [SerializeField]
    private List<StageLineData> _stageLineDatas = new();

    private Transform _contentsRoot;

    private void Awake()
    {
        Managers.Clear();

        InitCamera();
        InitContents();
        InitGround();
        InitRampart();
        InitCastleAndDungeon();
        InitBattleLine();
        InitSpawn();

        Managers.Game.Gold = 0;
    }

    private void InitCamera()
    {
        var camera = Camera.main;
        camera.transform.position = new Vector3(CameraX, CameraY, CameraZ);
        camera.orthographicSize = CameraSize;
    }

    private void InitContents()
    {
        _contentsRoot = Util.CreateGameObject("@Contens_Root").transform;

        if (!FindObjectOfType<TileManager>())
        {
            Managers.Resource.Instantiate($"{Define.CONTENTS_PATH}@TileManager").transform.SetParent(_contentsRoot);
        }

        if (!FindObjectOfType<MouseController>())
        {
            Managers.Resource.Instantiate($"{Define.CONTENTS_PATH}@MouseController").transform.SetParent(_contentsRoot);
        }

        if (!FindObjectOfType<CitizenSpawner>())
        {
            Managers.Resource.Instantiate($"{Define.CONTENTS_PATH}@CitizenSpawner").transform.SetParent(_contentsRoot);
        }

        if (!FindObjectOfType<CallSkill>())
        {
            Managers.Resource.Instantiate($"{Define.CONTENTS_PATH}@CallSkill").transform.SetParent(_contentsRoot);
        }

        if (!FindObjectOfType<TileObjectBuilder>())
        {
            Managers.Resource.Instantiate($"{Define.CONTENTS_PATH}@TileObjectBuilder").transform.SetParent(_contentsRoot);
        }

        if (!FindObjectOfType<RoadBuilder>())
        {
            Managers.Resource.Instantiate($"{Define.CONTENTS_PATH}@RoadBuilder").transform.SetParent(_contentsRoot);
        }
    }

    private void InitGround()
    {
        int stageNumber = PlayerPrefs.GetInt("Num");
        var go = Managers.Resource.Instantiate($"{Define.GROUND_PREFAB_PATH}Ground_{stageNumber}");
        go.transform.position = new Vector3(5f, 5f, 0f);
        go.transform.SetParent(TileManager.GetInstance.GetGrid().transform);
    }

    private void InitRampart()
    {
        var rampartLR = Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}Rampart_LR");
        var rampartUD = Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}Rampart_UD");
        var rampartCL = Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}Rampart_CL");
        var rampartCR = Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}Rampart_CR");

        for (int x = StartPosition.x + 1; x < RampartWidth - 1; x++)
        {
            TileManager.GetInstance.SetTile(Define.Tilemap.Rampart, new Vector3Int(x, StartPosition.y, 0), rampartLR);
            TileManager.GetInstance.SetTile(Define.Tilemap.Rampart, new Vector3Int(x, RampartHeight - 1, 0), rampartLR);
        }

        for (int y = StartPosition.y + 1; y < RampartHeight - 1; y++)
        {
            TileManager.GetInstance.SetTile(Define.Tilemap.Rampart, new Vector3Int(StartPosition.x, y, 0), rampartUD);
            TileManager.GetInstance.SetTile(Define.Tilemap.Rampart, new Vector3Int(RampartWidth - 1, y, 0), rampartUD);
        }

        TileManager.GetInstance.SetTile(Define.Tilemap.Rampart, StartPosition, rampartCL);
        TileManager.GetInstance.SetTile(Define.Tilemap.Rampart, new Vector3Int(StartPosition.x, RampartHeight - 1, 0), rampartCL);

        TileManager.GetInstance.SetTile(Define.Tilemap.Rampart, new Vector3Int(RampartWidth - 1, StartPosition.y, 0), rampartCR);
        TileManager.GetInstance.SetTile(Define.Tilemap.Rampart, new Vector3Int(RampartWidth - 1, RampartHeight - 1, 0), rampartCR);
    }

    private void InitSpawn()
    {
        CitizenSpawner.GetInstance.Setup(SpawnCellPos, SpawnTime);
    }

    private void InitCastleAndDungeon()
    {
        int stageNumber = PlayerPrefs.GetInt("Num") - 1;

        for (int i = 0; i < 3; i++)
        {
            if (_stageLineDatas[stageNumber].First && (i == 0))
            {
                SetCastleAndDungeon(1);
            }

            if (_stageLineDatas[stageNumber].Second && (i == 1))
            {
                SetCastleAndDungeon(3);
            }

            if (_stageLineDatas[stageNumber].Third && (i == 2))
            {
                SetCastleAndDungeon(5);
            }
        }
    }

    private void InitBattleLine()
    {
        int stageNumber = PlayerPrefs.GetInt("Num") - 1;

        for (int i = 0; i < 3; i++)
        {
            if (_stageLineDatas[stageNumber].First && (i == 0))
            {
                SetBattleLine(1);
            }

            if (_stageLineDatas[stageNumber].Second && (i == 1))
            {
                SetBattleLine(3);
            }

            if (_stageLineDatas[stageNumber].Third && (i == 2))
            {
                SetBattleLine(5);
            }
        }
    }

    private void SetCastleAndDungeon(int x)
    {
        TileManager.GetInstance.SetTile(Define.Tilemap.Rampart, new Vector3Int(StartPosition.x + x, RampartHeight - 1, 0), null);

        TileManager.GetInstance.SetTile(
                    Define.Tilemap.Building,
                    new Vector3Int(StartPosition.x + x, RampartHeight - 1, 0),
                    Define.Building.CastleGate);

        TileManager.GetInstance.SetTile(Define.Tilemap.Building,
            new Vector3Int(StartPosition.x + x, RampartHeight + BattleLineLength, 0),
            Define.Building.Dungeon);
    }

    private void SetBattleLine(int x)
    {
        for (int y = 0; y < BattleLineLength; y++)
        {
            TileManager.GetInstance.SetTile(Define.Tilemap.Road, new Vector3Int(StartPosition.x + x, RampartHeight + y, 0), Define.Road.UD);
        }

        TileManager.GetInstance.SetTile(Define.Tilemap.Road, new Vector3Int(StartPosition.x + x, RampartHeight + BattleLineLength, 0), Define.Road.BD);
    }
}