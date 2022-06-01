using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gateway : BaseBuilding
{
    public Dictionary<Define.Citizen, Define.MoveType> GatewayPassCondition { get; private set; }

    [SerializeField]
    private float releaseTime = 0.5f;

    private Queue<(Define.Citizen, Define.MoveType)> _citizenOrderQueue = new Queue<(Define.Citizen, Define.MoveType)>();
    private Camera _camera;

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

    public override void ShowUIController()
    {
        Managers.UI.CloseAllPopupUI();
        var controller = Managers.UI.ShowPopupUI<UI_CitizenDirectionController>();
        var pos = _camera.WorldToScreenPoint(transform.position) + (Vector3.right * 300);
        controller.transform.GetChild(0).position = pos;
        controller.Target = GatewayPassCondition;
    }

    protected override void Init()
    {
        GatewayPassCondition = new Dictionary<Define.Citizen, Define.MoveType>()
        {
            { Define.Citizen.Red, Define.MoveType.None },
            { Define.Citizen.Blue, Define.MoveType.None },
            { Define.Citizen.Green, Define.MoveType.None },
            { Define.Citizen.Yellow, Define.MoveType.None },
        };

        _camera = Camera.main;
        CanSelect = true;
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
}