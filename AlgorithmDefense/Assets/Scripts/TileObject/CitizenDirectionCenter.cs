using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenDirectionCenter : BaseBuilding
{
    [SerializeField]
    private float releaseTime = 0.5f;

    protected Action<CitizenController> BuildingFuncAction;
    protected bool _isApply;

    private Dictionary<Define.Citizen, Define.MoveType> _directionCondition;
    private Queue<CitizenController> _citizenOrderQueue = new Queue<CitizenController>();
    private Camera _camera;

    public override void EnterTheBuilding(CitizenController citizen)
    {
        BuildingFuncAction?.Invoke(citizen);

        _citizenOrderQueue.Enqueue(citizen);
        citizen.gameObject.SetActive(false);
        StartCoroutine(releaseCitizen());
    }

    public override void ShowUIController()
    {
        Managers.UI.CloseAllPopupUI();
        var controller = Managers.UI.ShowPopupUI<UI_CitizenDirectionController>();
        var pos = _camera.WorldToScreenPoint(transform.position) + (Vector3.right * 300);
        controller.transform.GetChild(0).position = pos;
        controller.Target = _directionCondition;
    }

    private IEnumerator releaseCitizen()
    {
        yield return new WaitForSeconds(releaseTime);

        if (_citizenOrderQueue.Count == 0)
        {
            yield break;
        }

        var citizen = _citizenOrderQueue.Dequeue();
        citizen.gameObject.SetActive(true);
        citizen.transform.position = Managers.Tile.GetWorldToCellCenterToWorld(Define.Tilemap.Ground, transform.position);
        citizen.PrevPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Ground, transform.position);
        citizen.IsExit = false;

        if (_isApply)
        {
            var moveType = _directionCondition[citizen.CitizenType];
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

    protected override void Init()
    {
        _directionCondition = new Dictionary<Define.Citizen, Define.MoveType>()
        {
            { Define.Citizen.Red, Define.MoveType.None },
            { Define.Citizen.Blue, Define.MoveType.None },
            { Define.Citizen.Green, Define.MoveType.None },
            { Define.Citizen.Yellow, Define.MoveType.None },
        };

        _camera = Camera.main;
        _isApply = true;
        CanSelect = true;
        AddBuildingFun();
    }

    protected virtual void AddBuildingFun() { }
}
