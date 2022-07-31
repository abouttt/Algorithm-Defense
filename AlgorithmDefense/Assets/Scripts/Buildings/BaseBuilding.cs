using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuilding : MonoBehaviour
{
    [System.Serializable]
    public struct CitizenOrderQueueData
    {
        public string CitzienName;
        public CitizenData CitizenData;
    }

    public bool HasUI { get; protected set; }

    [SerializeField]
    protected float _releaseTime;
    protected Queue<CitizenOrderQueueData> _citizenOrderQueue = new Queue<CitizenOrderQueueData>();
    protected bool _isReleasing;

    private void Start()
    {
        Init();
    }

    public abstract void EnterTheBuilding(CitizenController citizen);
    public abstract string GetSaveData();
    public abstract void LoadSaveData(string saveData);

    protected abstract IEnumerator ReleaseCitizen();
    protected abstract void Init();

    protected void EnqueueCitizen(CitizenController citizen)
    {
        _citizenOrderQueue.Enqueue(new CitizenOrderQueueData 
        { 
            CitzienName = citizen.name, 
            CitizenData = citizen.Data
        });

        Managers.Resource.Destroy(citizen.gameObject);

        if (!_isReleasing)
        {
            _isReleasing = true;
            StartCoroutine(ReleaseCitizen());
        }
    }

    protected CitizenController DequeueCitizen()
    {
        var orderQueueData = _citizenOrderQueue.Dequeue();

        GameObject go = null;
        if (orderQueueData.CitzienName.Contains("Citizen"))
        {
            go = Managers.Resource.Instantiate($"{Define.CITIZEN_PATH}{orderQueueData.CitzienName}");
        }
        else
        {
            go = Managers.Resource.Instantiate($"{Define.BATTILE_UNIT_PATH}{orderQueueData.CitzienName}");
        }

        var citizen = go.GetOrAddComponent<CitizenController>();
        citizen.Data = orderQueueData.CitizenData;

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

    protected void SetNextDestination(CitizenController citizen)
    {
        switch (citizen.Data.MoveType)
        {
            case Define.Move.Right:
                citizen.transform.localScale = new Vector3(-1, 1, 1);
                break;
            case Define.Move.Left:
                citizen.transform.localScale = new Vector3(1, 1, 1);
                break;
        }

        citizen.Data.Destination = Managers.Tile.GetWorldToCellCenterToWorld(Define.Tilemap.Ground, citizen.transform.position);
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

    public virtual void CopyTo(BaseBuilding other)
    {
        other.HasUI = HasUI;
        other._releaseTime = _releaseTime;
        other._citizenOrderQueue = _citizenOrderQueue;
        other._isReleasing = _isReleasing;
    }
}
