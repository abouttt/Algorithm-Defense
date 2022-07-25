using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameScene : MonoBehaviour
{
    [Header("[게임 초기 설정] - 씬 시작 시 1회만 동작한다.")]

    [Header("[카메라 설정]")]
    [SerializeField]
    private float _cameraX;
    [SerializeField]
    private float _cameraY;
    [SerializeField]
    private float _cameraZ;
    [SerializeField]
    private float _cameraSize;

    [Header("[그라운드 생성]")]
    [SerializeField]
    private int _groundWidth;
    [SerializeField]
    private int _groundHeight;
    [SerializeField]
    private int _grassPercentage;

    [Header("[성벽 생성]")]
    [SerializeField]
    private int _rampartWidth;
    [SerializeField]
    private int _rampartHeight;

    [Header("[시민 스폰 설정]")]
    [SerializeField]
    private Vector3Int _spawnCellPos;
    [SerializeField]
    private float _spawnTime;

    private Transform _contentsRoot;

    private void Awake()
    {
        InitCamera();
        InitContents();
        InitTilesObject();
        InitGround();
        InitRampart();
        InitSpawn();

        TileObjectBuilder.GetInstance.SetBuildingTarget(Define.Building.Gateway);

        Destroy(this);
    }

    private void InitCamera()
    {
        var camera = Camera.main;
        camera.transform.position = new Vector3(_cameraX, _cameraY, _cameraZ);
        camera.orthographicSize = _cameraSize;
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
    }

    private void InitGround()
    {
        for (int x = 0; x < _groundWidth; x++)
        {
            for (var y = 0; y < _groundHeight; y++)
            {
                int grassTileNum = 1;
                int randNum = UnityEngine.Random.Range(0, 100);
                if (randNum <= _grassPercentage)
                {
                    grassTileNum = UnityEngine.Random.Range(2, 4);
                }

                var tile = Managers.Resource.Load<Tile>($"Tiles/Grounds/Grass_{grassTileNum}");
                Managers.Tile.SetTile(Define.Tilemap.Ground, new Vector3Int(x, y), tile);
            }
        }
    }

    private void InitTilesObject()
    {
        var buildingNames = Enum.GetNames(typeof(Define.Building));
        foreach (var buildingName in buildingNames)
        {
            var tile = Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}{buildingName}");
            tile.gameObject = Managers.Resource.Load<GameObject>($"{Define.BUILDING_PREFAB_PATH}{buildingName}");
        }

        var roadNames = Enum.GetNames(typeof(Define.Road));
        foreach (var roadName in roadNames)
        {
            var tile = Managers.Resource.Load<Tile>($"{Define.ROAD_TILE_PATH}Road_{roadName}");
            tile.gameObject = Managers.Resource.Load<GameObject>($"{Define.ROAD_PREFAB_PATH}Road_{roadName}");
        }
    }

    private void InitRampart()
    {
        var rampartLR = Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}Rampart_LR");
        var rampartUD = Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}Rampart_UD");
        var rampartCL = Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}Rampart_CL");
        var rampartCR = Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}Rampart_CR");

        for (int x = 1; x < _rampartWidth - 1; x++)
        {
            Managers.Tile.SetTile(Define.Tilemap.Building, new Vector3Int(x, 0, 0), rampartLR);
            Managers.Tile.SetTile(Define.Tilemap.Building, new Vector3Int(x, _rampartHeight - 1, 0), rampartLR);
        }

        for (int y = 1; y < _rampartHeight - 1; y++)
        {
            Managers.Tile.SetTile(Define.Tilemap.Building, new Vector3Int(0, y, 0), rampartUD);
            Managers.Tile.SetTile(Define.Tilemap.Building, new Vector3Int(_rampartWidth - 1, y, 0), rampartUD);
        }

        Managers.Tile.SetTile(Define.Tilemap.Building, new Vector3Int(0, 0, 0), rampartCL);
        Managers.Tile.SetTile(Define.Tilemap.Building, new Vector3Int(0, _rampartHeight - 1, 0), rampartCL);

        Managers.Tile.SetTile(Define.Tilemap.Building, new Vector3Int(_rampartWidth - 1, 0, 0), rampartCR);
        Managers.Tile.SetTile(Define.Tilemap.Building, new Vector3Int(_rampartWidth - 1, _rampartHeight - 1, 0), rampartCR);
    }

    private void InitSpawn()
    {
        CitizenSpawner.GetInstance.Setup(_spawnCellPos, _spawnTime);
    }
}
