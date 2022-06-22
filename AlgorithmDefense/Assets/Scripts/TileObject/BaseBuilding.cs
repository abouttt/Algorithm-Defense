using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuilding : MonoBehaviour
{
    public bool CanSelect { get; protected set; }

    [SerializeField]
    protected float _stayTime;
    protected Queue<CitizenController> _citizenOrderQueue = new Queue<CitizenController>();
    protected bool _isDirectionOpposite;

    private void Start()
    {
        Init();
    }

    public abstract void EnterTheBuilding(CitizenController citizen);
    public virtual void ShowUIController() { }

    protected abstract void Init();

    protected virtual IEnumerator LeaveTheBuilding()
    {
        yield return new WaitForSeconds(_stayTime);

        if (_citizenOrderQueue.Count == 0)
        {
            yield break;
        }

        var citizen = DequeueCitizen();

        if (_isDirectionOpposite)
        {
            SetOpposite(citizen);
        }

        if (!HasRoadNextPosition(citizen.MoveType))
        {
            SetOpposite(citizen);
        }

        citizen.SetDest();
        SetPosition(citizen);
    }

    protected void EnqueueCitizen(CitizenController citizen)
    {
        _citizenOrderQueue.Enqueue(citizen);
        citizen.gameObject.SetActive(false);

        StartCoroutine(LeaveTheBuilding());
    }

    protected CitizenController DequeueCitizen()
    {
        var citizen = _citizenOrderQueue.Dequeue();
        citizen.gameObject.SetActive(true);
        citizen.PrevPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Ground, transform.position);
        citizen.IsExit = false;
        return citizen;
    }

    protected void SetOpposite(CitizenController citizen)
    {
        switch (citizen.MoveType)
        {
            case Define.MoveType.Right:
                citizen.MoveType = Define.MoveType.Left;
                break;
            case Define.MoveType.Left:
                citizen.MoveType = Define.MoveType.Right;
                break;
            case Define.MoveType.Up:
                citizen.MoveType = Define.MoveType.Down;
                break;
            case Define.MoveType.Down:
                citizen.MoveType = Define.MoveType.Up;
                break;
        }
    }

    protected bool HasRoadNextPosition(Define.MoveType moveType)
    {
        var nextPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Ground, transform.position);

        switch (moveType)
        {
            case Define.MoveType.Right:
                nextPos += Vector3Int.right;
                break;
            case Define.MoveType.Left:
                nextPos += Vector3Int.left;
                break;
            case Define.MoveType.Up:
                nextPos += Vector3Int.up;
                break;
            case Define.MoveType.Down:
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

    protected void SetPosition(CitizenController citizen)
    {
        var pos = Managers.Tile.GetWorldToCellCenterToWorld(Define.Tilemap.Ground, transform.position);

        switch (citizen.MoveType)
        {
            case Define.MoveType.Right:
                pos += new Vector3(0.5f, 0, 0);
                break;
            case Define.MoveType.Left:
                pos += new Vector3(-0.5f, 0, 0);
                break;
            case Define.MoveType.Up:
                pos += new Vector3(0, 0.5f, 0);
                break;
            case Define.MoveType.Down:
                pos += new Vector3(0, -0.5f, 0);
                break;
        }

        citizen.transform.position = pos;
    }
}
