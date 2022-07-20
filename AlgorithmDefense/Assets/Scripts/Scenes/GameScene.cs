using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameScene : MonoBehaviour
{
    [Header("그라운드 생성 사이즈")]
    [SerializeField]
    private int _groundWidth;
    [SerializeField]
    private int _groundHeight;

    [Header("카메라 설정")]
    [SerializeField]
    private float _cameraX;
    [SerializeField]
    private float _cameraY;
    [SerializeField]
    private float _cameraZ;
    [SerializeField]
    private float _cameraSize;

    private void Start()
    {
        InitCamera();
        InitGround();
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
                int randNum = Random.Range(1, 4);
                var tile = Managers.Resource.Load<Tile>($"Tiles/Grounds/Grass_{randNum}");
                Managers.Tile.SetTile(Define.Tilemap.Ground, new Vector3Int(x, y), tile);
            }
        }
    }
    
}
