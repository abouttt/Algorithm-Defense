using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuilding : MonoBehaviour
{
    public bool HasUI { get; protected set; }

    [SerializeField]
    protected float _releaseTime;
    protected Queue<CitizenController> _citizenOrderQueue = new Queue<CitizenController>();
    protected bool _isReleasing = false;

    private void Start()
    {
        Init();
    }

    public abstract void EnterTheBuilding(CitizenController citizen);

    protected abstract IEnumerator ReleaseCitizen();
    protected abstract void Init();

    protected void EnqueueCitizen(CitizenController citizen)
    {
        _citizenOrderQueue.Enqueue(citizen);
        citizen.gameObject.SetActive(false);

        if (!_isReleasing)
        {
            _isReleasing = true;
            StartCoroutine(ReleaseCitizen());
        }
    }

    protected CitizenController DequeueCitizen()
    {
        var citizen = _citizenOrderQueue.Dequeue();
        citizen.gameObject.SetActive(true);
        return citizen;
    }

    protected void SetCitizenPosition(CitizenController citizen)
    {
        var pos = Managers.Tile.GetWorldToCellCenterToWorld(Define.Tilemap.Ground, transform.position);

        switch (citizen.Data.MoveType)
        {
            case Define.Move.Right:
                pos += new Vector3(0.5f, 0, 0);
                break;
            case Define.Move.Left:
                pos += new Vector3(-0.5f, 0, 0);
                break;
            case Define.Move.Up:
                pos += new Vector3(0, 0.5f, 0);
                break;
            case Define.Move.Down:
                pos += new Vector3(0, -0.5f, 0);
                break;
        }

        citizen.transform.position = pos;
    }

    protected bool IsRoadNextPosition(Define.Move moveType)
    {
        var nextPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Ground, transform.position);

        switch (moveType)
        {
            case Define.Move.Right:
                nextPos += Vector3Int.right;
                break;
            case Define.Move.Left:
                nextPos += Vector3Int.left;
                break;
            case Define.Move.Up:
                nextPos += Vector3Int.up;
                break;
            case Define.Move.Down:
                nextPos += Vector3Int.down;
                break;
        }

        var tile = Managers.Tile.GetTile(Define.Tilemap.Road, nextPos);
        if (!tile)
        {
            return false;
        }

        return true;
    }
}
