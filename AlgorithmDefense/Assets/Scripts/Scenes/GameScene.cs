using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameScene : MonoBehaviour
{
    [Header("[그라운드 생성]")]
    [SerializeField]
    private int _groundWidth;
    [SerializeField]
    private int _groundHeight;
    [SerializeField]
    private int _grassPercentage;

    [Header("[카메라 설정]")]
    [SerializeField]
    private float _cameraX;
    [SerializeField]
    private float _cameraY;
    [SerializeField]
    private float _cameraZ;
    [SerializeField]
    private float _cameraSize;

    [Header("[시민 스폰 설정]")]
    [SerializeField]
    private Vector3Int _spawnCellPos;
    [SerializeField]
    private float _spawnTime;

    private Transform _contentsRoot;

    private void Start()
    {
        InitContents();
        InitCamera();
        InitGround();
        InitSpawn();

        TileObjectBuilder.GetInstance.SetBuildingTarget(Define.Building.Gateway);
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

    private void InitCamera()
    {
        var camera = Camera.main;
        camera.transform.position = new Vector3(_cameraX, _cameraY, _cameraZ);
        camera.orthographicSize = _cameraSize;
    }

    private void InitGround()
    {
        for (int x = 0; x < _groundWidth; x++)
        {
            for (var y = 0; y < _groundHeight; y++)
            {
                int grassTileNum = 1;
                int randNum = Random.Range(0, 100);
                if (randNum <= _grassPercentage)
                {
                    grassTileNum = Random.Range(2, 4);
                }

                var tile = Managers.Resource.Load<Tile>($"Tiles/Grounds/Grass_{grassTileNum}");
                Managers.Tile.SetTile(Define.Tilemap.Ground, new Vector3Int(x, y), tile);
            }
        }
    }

    private void InitSpawn()
    {
        CitizenSpawner.GetInstance.Setup(_spawnCellPos, _spawnTime);
    }
}
