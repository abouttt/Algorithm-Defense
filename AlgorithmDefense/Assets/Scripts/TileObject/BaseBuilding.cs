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

        var citizen = _citizenOrderQueue.Dequeue();
        citizen.gameObject.SetActive(true);
        citizen.transform.position = Managers.Tile.GetWorldToCellCenterToWorld(Define.Tilemap.Ground, transform.position);
        citizen.PrevPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Ground, transform.position);
        citizen.IsExit = false;

        if (_isDirectionOpposite)
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
    }
}
