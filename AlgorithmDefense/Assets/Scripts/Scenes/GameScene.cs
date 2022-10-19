using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameScene : MonoBehaviour
{
    [Header("[게임 초기 설정] - 씬 시작 시 1회만 동작한다.")]

    [Header("[카메라 설정]")]
    public float CameraX;
    public float CameraY;
    public float CameraZ;
    public float CameraSize;

    [Header("[그라운드 생성]")]
    public int GroundWidth;
    public int GroundHeight;
    public int GrassPercentage;

    [Header("[성벽 생성]")]
    public Vector3Int StartPosition;
    public int RampartWidth;
    public int RampartHeight;

    [Header("[시민 스폰 설정]")]
    public Vector3Int SpawnCellPos;
    public float SpawnTime;

    [Header("[전투 라인 설정]")]
    public int BattleLineLength;

    private Transform _contentsRoot;

    private void Start()
    {
        InitTileObjects();
        InitCamera();
        InitContents();
        //InitGround();
        InitRampart();
        InitSpawn();
        InitBattleLine();
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

        if (!FindObjectOfType<MouseController>())
        {
            Managers.Resource.Instantiate($"{Define.CONTENTS_PATH}@MouseController").transform.SetParent(_contentsRoot);
        }

        if (!FindObjectOfType<CitizenSpawner>())
        {
            Managers.Resource.Instantiate($"{Define.CONTENTS_PATH}@CitizenSpawner").transform.SetParent(_contentsRoot);
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
        for (int x = 0; x < GroundWidth; x++)
        {
            for (var y = 0; y < GroundHeight; y++)
            {
                int grassTileNum = 1;
                int randNum = UnityEngine.Random.Range(0, 100);
                if (randNum <= GrassPercentage)
                {
                    grassTileNum = UnityEngine.Random.Range(2, 4);
                }

                var tile = Managers.Resource.Load<Tile>($"Tiles/Grounds/Grass_{grassTileNum}");
                Managers.Tile.SetTile(Define.Tilemap.Ground, new Vector3Int(x, y), tile);
            }
        }
    }

    public void InitTileObjects()
    {
        var buildingNames = Enum.GetNames(typeof(Define.Building));
        Tile buildingTile = null;
        foreach (var buildingName in buildingNames)
        {
            buildingTile = Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}{buildingName}");
            buildingTile.gameObject = Managers.Resource.Load<GameObject>($"{Define.BUILDING_PREFAB_PATH}{buildingName}");
            buildingTile.color = Color.white;
        }

        var roadNames = Enum.GetNames(typeof(Define.Road));
        Tile roadTile = null;
        foreach (var roadName in roadNames)
        {
            roadTile = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_{roadName}");
            roadTile.gameObject = Managers.Resource.Load<GameObject>($"{Define.ROAD_PREFAB_PATH}Road_{roadName}");
        }
    }

    private void InitRampart()
    {
        var rampartLR = Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}Rampart_LR");
        var rampartUD = Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}Rampart_UD");
        var rampartCL = Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}Rampart_CL");
        var rampartCR = Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}Rampart_CR");

        for (int x = StartPosition.x + 1; x < RampartWidth - 1; x++)
        {
            Managers.Tile.SetTile(Define.Tilemap.Rampart, new Vector3Int(x, StartPosition.y, 0), rampartLR);
            Managers.Tile.SetTile(Define.Tilemap.Rampart, new Vector3Int(x, RampartHeight - 1, 0), rampartLR);
        }

        for (int y = StartPosition.y + 1; y < RampartHeight - 1; y++)
        {
            Managers.Tile.SetTile(Define.Tilemap.Rampart, new Vector3Int(StartPosition.x, y, 0), rampartUD);
            Managers.Tile.SetTile(Define.Tilemap.Rampart, new Vector3Int(RampartWidth - 1, y, 0), rampartUD);
        }

        Managers.Tile.SetTile(Define.Tilemap.Rampart, StartPosition, rampartCL);
        Managers.Tile.SetTile(Define.Tilemap.Rampart, new Vector3Int(StartPosition.x, RampartHeight - 1, 0), rampartCL);

        Managers.Tile.SetTile(Define.Tilemap.Rampart, new Vector3Int(RampartWidth - 1, StartPosition.y, 0), rampartCR);
        Managers.Tile.SetTile(Define.Tilemap.Rampart, new Vector3Int(RampartWidth - 1, RampartHeight - 1, 0), rampartCR);
    }

    private void InitSpawn()
    {
        CitizenSpawner.GetInstance.Setup(SpawnCellPos, SpawnTime);
    }

    private void InitBattleLine()
    {
        var castleGate = Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}{Define.Building.CastleGate}");
        var bdRoad = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_BD");
        var udRoad = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_UD");
        var monsterCenter = Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}{Define.Building.MonsterGate}");

        // 성벽 / 몬스터 스폰 지역 설치.
        for (int x = 1; x <= 5; x += 2)
        {
            Managers.Tile.SetTile(Define.Tilemap.Rampart, new Vector3Int(StartPosition.x + x, RampartHeight - 1, 0), null);

            Managers.Tile.SetTile(Define.Tilemap.Building, new Vector3Int(StartPosition.x + x, RampartHeight - 1, 0), castleGate);
            Managers.Tile.SetTile(Define.Tilemap.Building, new Vector3Int(StartPosition.x + x, RampartHeight + BattleLineLength, 0), monsterCenter);
        }

        // 길 설치.
        for (int x = 1; x <= 5; x += 2)
        {
            for (int y = 0; y < BattleLineLength; y++)
            {
                Managers.Tile.SetTile(Define.Tilemap.Road, new Vector3Int(StartPosition.x + x, RampartHeight + y, 0), udRoad);
            }

            Managers.Tile.SetTile(Define.Tilemap.Road, new Vector3Int(StartPosition.x + x, RampartHeight + BattleLineLength, 0), bdRoad);
        }
    }
}
