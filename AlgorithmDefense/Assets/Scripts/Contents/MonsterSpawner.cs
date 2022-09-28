using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MonsterSpawner : MonoBehaviour
{
    public float SpawnTime;

    private List<Vector3> _gatePos = new();

    private void Start()
    {
        for (int x = 1; x <= 5; x += 2)
        {
            _gatePos.Add(Managers.Tile.GetCellToWorld(Define.Tilemap.Building, new Vector3Int(
                    Managers.Game.Setting.StartPosition.x + x,
                    Managers.Game.Setting.RampartHeight + Managers.Game.Setting.BattleLineLength, 0)));

        }

        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(SpawnTime);

        for (int i = 0; i < _gatePos.Count; i++)
        {
            var go = Managers.Resource.Instantiate("Prefabs/Units/MonsterUnits/Goblin_Warrior");
            go.transform.position = _gatePos[i] + (Vector3.right * 0.5f);
        }
    }
}
