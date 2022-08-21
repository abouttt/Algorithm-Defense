using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMove : MonoBehaviour
{
    public float MoveSpeed = 2.0f;
    public Vector3 Dest;

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Dest, (MoveSpeed * Time.deltaTime));

        var cellPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Ground, transform.position);
        if (Vector2.Distance(transform.position, Dest) <= 0.01f)
        {
            var go = Managers.Tile.GetTilemap(Define.Tilemap.Building).GetInstantiatedObject(cellPos);
            if (!go)
            {
                return;
            }

            go.GetComponent<TBuilding>().EnterTheBuilding(this);
        }
    }
}
