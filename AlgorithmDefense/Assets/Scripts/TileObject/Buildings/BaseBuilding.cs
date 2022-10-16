using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuilding : MonoBehaviour
{
    public bool HasUI { get; protected set; }

    [SerializeField]
    protected float _releaseTime;
    protected Queue<CitizenData> _citizenOrderQueue = new();
    protected bool _isReleasing;

    private void Start()
    {
        Init();
    }

    public abstract void EnterTheBuilding(CitizenController citizen);

    protected abstract IEnumerator ReleaseCitizen();
    protected abstract void Init();

    protected void EnqueueCitizen(CitizenController citizen)
    {
        _citizenOrderQueue.Enqueue(citizen.Data);

        Managers.Resource.Destroy(citizen.gameObject);

        if (!_isReleasing)
        {
            _isReleasing = true;
            StartCoroutine(ReleaseCitizen());
        }
    }

    protected CitizenController DequeueCitizen()
    {
        var citizenData = _citizenOrderQueue.Dequeue();

        GameObject go = null;
        if (citizenData.JobType == Define.Job.None)
        {
            go = Managers.Resource.Instantiate($"{Define.CITIZEN_PATH}{citizenData.CitizenType}Citizen");
        }
        else
        {
            go = Managers.Resource.Instantiate(
                $"{Define.BATTILE_UNIT_PATH}" +
                $"{citizenData.CitizenType}_{citizenData.JobType}");
        }

        var citizen = go.GetOrAddComponent<CitizenController>();
        citizen.Data = citizenData;

        return citizen;
    }

    protected void SetCitizenPosition(CitizenController citizen)
    {
        var pos = Managers.Tile.GetWorldToCellCenterToWorld(Define.Tilemap.Ground, transform.position);

        switch (citizen.Data.MoveType)
        {
            case Define.Move.Right:
                pos += new Vector3(0.51f, 0, 0);
                break;
            case Define.Move.Left:
                pos += new Vector3(-0.51f, 0, 0);
                break;
            case Define.Move.Up:
                pos += new Vector3(0, 0.51f, 0);
                break;
            case Define.Move.Down:
                pos += new Vector3(0, -0.51f, 0);
                break;
        }

        citizen.transform.position = pos;
    }

    protected bool HasRoadNextPosition(Define.Move moveType)
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

        return Util.GetRoad(Define.Tilemap.Road, nextPos) ? true : false;
    }

    protected bool HasNeighborRoad()
    {
        var pos = Managers.Tile.GetWorldToCell(Define.Tilemap.Road, transform.position);
        for (int i = 0; i < 4; i++)
        {
            int nx = pos.x + Define.DX[i];
            int ny = pos.y + Define.DY[i];

            if (Util.GetRoad(Define.Tilemap.Road, new Vector3Int(nx, ny, 0)))
            {
                return true;
            }
        }

        return false;
    }
}
