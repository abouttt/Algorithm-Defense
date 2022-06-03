using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionApplicableBuilding : BaseBuilding
{
    protected Action<CitizenController> _buildingFuncAction;

    private Dictionary<Define.Citizen, Define.MoveType> _directionCondition;
    private Camera _camera;

    public override void EnterTheBuilding(CitizenController citizen)
    {
        _buildingFuncAction?.Invoke(citizen);

        _citizenOrderQueue.Enqueue(citizen);
        citizen.gameObject.SetActive(false);
        StartCoroutine(ReleaseCitizen());
    }

    public override void ShowUIController()
    {
        Managers.UI.CloseAllPopupUI();
        var controller = Managers.UI.ShowPopupUI<UI_CitizenDirectionController>();
        var pos = _camera.WorldToScreenPoint(transform.position) + (Vector3.right * 300);
        controller.transform.GetChild(0).position = pos;
        controller.Target = _directionCondition;
    }

    protected override IEnumerator ReleaseCitizen()
    {
        yield return new WaitForSeconds(_releaseTime);

        if (_citizenOrderQueue.Count == 0)
        {
            yield break;
        }

        var citizen = _citizenOrderQueue.Dequeue();
        citizen.gameObject.SetActive(true);
        citizen.transform.position = Managers.Tile.GetWorldToCellCenterToWorld(Define.Tilemap.Ground, transform.position);
        citizen.PrevPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Ground, transform.position);
        citizen.IsExit = false;

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

    protected override void Init()
    {
        _directionCondition = new Dictionary<Define.Citizen, Define.MoveType>()
        {
            { Define.Citizen.Red, Define.MoveType.None },
            { Define.Citizen.Blue, Define.MoveType.None },
            { Define.Citizen.Green, Define.MoveType.None },
            { Define.Citizen.Yellow, Define.MoveType.None },
        };

        CanSelect = true;
        _isDirectionOpposite = true;
        _camera = Camera.main;

        AddBuildingFun();
    }

    protected virtual void AddBuildingFun() { }
}
