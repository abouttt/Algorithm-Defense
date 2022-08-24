using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameScene : MonoBehaviour
{
    [Header("[���� �ʱ� ����] - �� ���� �� 1ȸ�� �����Ѵ�.")]

    [Header("[ī�޶� ����]")]
    public float CameraX;
    public float CameraY;
    public float CameraZ;
    public float CameraSize;

    [Header("[�׶��� ����]")]
    public int GroundWidth;
    public int GroundHeight;
    public int GrassPercentage;

    [Header("[���� ����]")]
    public Vector3Int StartPosition;
    public int RampartWidth;
    public int RampartHeight;

    [Header("[�ù� ���� ����]")]
    public Vector3Int SpawnCellPos;
    public float SpawnTime;

    [Header("���� ������")]
    public int OreIncrease;
    [Header("���� ������")]
    public int WoodIncrease;

    private Transform _contentsRoot;

    private void Awake()
    {
        InitCamera();
        InitContents();
        InitTilesObject();
        InitGround();
        InitRampart();
        InitSpawn();
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

        for (int x = StartPosition.x + 1; x < RampartWidth - 1; x++)
        {
            Managers.Tile.SetTile(Define.Tilemap.Building, new Vector3Int(x, StartPosition.y, 0), rampartLR);
            Managers.Tile.SetTile(Define.Tilemap.Building, new Vector3Int(x, RampartHeight - 1, 0), rampartLR);
        }

        for (int y = StartPosition.y + 1; y < RampartHeight - 1; y++)
        {
            Managers.Tile.SetTile(Define.Tilemap.Building, new Vector3Int(StartPosition.x, y, 0), rampartUD);
            Managers.Tile.SetTile(Define.Tilemap.Building, new Vector3Int(RampartWidth - 1, y, 0), rampartUD);
        }

        Managers.Tile.SetTile(Define.Tilemap.Building, StartPosition, rampartCL);
        Managers.Tile.SetTile(Define.Tilemap.Building, new Vector3Int(StartPosition.x, RampartHeight - 1, 0), rampartCL);

        Managers.Tile.SetTile(Define.Tilemap.Building, new Vector3Int(RampartWidth - 1, StartPosition.y, 0), rampartCR);
        Managers.Tile.SetTile(Define.Tilemap.Building, new Vector3Int(RampartWidth - 1, RampartHeight - 1, 0), rampartCR);
    }

    private void InitSpawn()
    {
        CitizenSpawner.GetInstance.Setup(SpawnCellPos, SpawnTime);
    }
}
