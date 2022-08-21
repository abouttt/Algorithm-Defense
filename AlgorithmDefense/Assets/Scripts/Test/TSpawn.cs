using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TSpawn : MonoBehaviour
{
    public float SpawnDelay;
    public Vector3Int SpawnCellPos;

    private bool _isSpawning = false;

    private void Start()
    {
        TileObjectBuilder.GetInstance.SetBuildingTarget(Define.Building.TBuilding);
        var tile = Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}TBuilding");
        TileObjectBuilder.GetInstance.Build(tile, new Vector3Int(5, 7, 0));
    }

    public void SpawnButton()
    {
        _isSpawning = _isSpawning ? false : true;
        StartCoroutine(SpawnCitizen());
    }

    public void BuildingButton()
    {
        TileObjectBuilder.GetInstance.SetBuildingTarget(Define.Building.TBuilding);
    }

    public IEnumerator SpawnCitizen()
    {
        while (true)
        {
            if (!_isSpawning)
            {
                yield break;
            }

            var pos = Managers.Tile.GetCellCenterToWorld(Define.Tilemap.Ground, SpawnCellPos);
            var go = Managers.Resource.Instantiate($"{Define.CITIZEN_PATH}RedCitizen", pos);
            Destroy(go.GetComponent<CitizenController>());
            var citizen = go.GetOrAddComponent<TMove>();
            citizen.Dest = Managers.Tile.GetCellCenterToWorld(Define.Tilemap.Ground, new Vector3Int(5, 7, 0));

            yield return new WaitForSeconds(SpawnDelay);
        }
    }
}
