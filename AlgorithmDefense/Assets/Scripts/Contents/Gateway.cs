using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class Gateway : MonoBehaviour
{
    public Dictionary<Define.Citizen, Define.MoveType> GatewayPassCondition { get; private set; }

    [SerializeField]
    private float releaseTime = 0.5f;

    private Queue<(Define.Citizen, Define.MoveType)> _citizenOrderQueue = new Queue<(Define.Citizen, Define.MoveType)>();
    private Tilemap _globalTilemap = null;
    private Camera _camera = null;

    private void Start()
    {
        GatewayPassCondition = new Dictionary<Define.Citizen, Define.MoveType>()
        {
            { Define.Citizen.Red, Define.MoveType.None },
            { Define.Citizen.Blue, Define.MoveType.None },
            { Define.Citizen.Green, Define.MoveType.None },
            { Define.Citizen.Yellow, Define.MoveType.None },
        };

        _globalTilemap = Managers.Tile.GetTilemap(Define.Tilemap.Global);
        _camera = Camera.main;
    }

    public void SetGateWay(Define.Citizen citizen, Define.MoveType move)
    {
        if (GatewayPassCondition.ContainsKey(citizen))
        {
            GatewayPassCondition[citizen] = move;
        }
    }

    public void EnterCitizen(CitizenController citizen)
    {
        if (citizen != null)
        {
            var citizenInfo = (citizen.CitizenType, citizen.MoveType);
            _citizenOrderQueue.Enqueue(citizenInfo);
            Managers.Game.Despawn(citizen.gameObject);
            StartCoroutine(releaseCitizen());
        }
    }

    private IEnumerator releaseCitizen()
    {
        yield return new WaitForSeconds(releaseTime);

        var citizenInfo = _citizenOrderQueue.Dequeue();

        var cellPos = _globalTilemap.WorldToCell(transform.position);
        var pos = _globalTilemap.GetCellCenterWorld(cellPos);
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

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (BaseBuilder.IsBuilding)
        {
            return;
        }

        Managers.UI.CloseAllPopupUI();
        var controller = Managers.UI.ShowPopupUI<UI_GatewayController>();
        var pos = _camera.WorldToScreenPoint(transform.position) + (Vector3.right * 300);
        controller.transform.GetChild(0).position = pos;
        controller.Target = this;
    }
}
