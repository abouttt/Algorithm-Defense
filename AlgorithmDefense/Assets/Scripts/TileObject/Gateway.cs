using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gateway : BaseBuilding
{
    public Dictionary<Define.Citizen, Define.MoveType> GatewayPassCondition { get; private set; }

    [SerializeField]
    private float releaseTime = 0.5f;

    private Queue<(Define.Citizen, Define.MoveType)> _citizenOrderQueue = new Queue<(Define.Citizen, Define.MoveType)>();
    private Camera _camera;

    private void Start()
    {
        GatewayPassCondition = new Dictionary<Define.Citizen, Define.MoveType>()
        {
            { Define.Citizen.Red, Define.MoveType.None },
            { Define.Citizen.Blue, Define.MoveType.None },
            { Define.Citizen.Green, Define.MoveType.None },
            { Define.Citizen.Yellow, Define.MoveType.None },
        };

        _camera = Camera.main;
    }

    public void SetGateWay(Define.Citizen citizen, Define.MoveType move)
    {
        if (citizen == Define.Citizen.None)
        {
            return;
        }

        GatewayPassCondition[citizen] = move;
    }

    public override void EnterTheBuilding(CitizenController citizen)
    {
        if (citizen == null)
        {
            return;
        }

        var citizenInfo = (citizen.CitizenType, citizen.MoveType);
        _citizenOrderQueue.Enqueue(citizenInfo);
        Managers.Game.Despawn(citizen.gameObject);
        StartCoroutine(releaseCitizen());
    }

    private IEnumerator releaseCitizen()
    {
        yield return new WaitForSeconds(releaseTime);

        var citizenInfo = _citizenOrderQueue.Dequeue();
        
        var pos = Managers.Tile.GetWorldToCellCenterToWorld(Define.Tilemap.Ground, transform.position);
        var go = Managers.Game.Spawn(Define.WorldObject.Citizen, $"{citizenInfo.Item1}Citizen", pos);

        var citizen = go.GetComponent<CitizenController>();
        citizen.MoveType = citizenInfo.Item2;

        var moveType = GatewayPassCondition[citizen.CitizenType];
        if (moveType != Define.MoveType.None)
        {
            citizen.MoveType = moveType;
        }
        else
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

    public override void ShowUIController()
    {
        Managers.UI.CloseAllPopupUI();
        var controller = Managers.UI.ShowPopupUI<UI_GatewayController>();
        var pos = _camera.WorldToScreenPoint(transform.position) + (Vector3.right * 300);
        controller.transform.GetChild(0).position = pos;
        controller.Target = this;
    }
}
