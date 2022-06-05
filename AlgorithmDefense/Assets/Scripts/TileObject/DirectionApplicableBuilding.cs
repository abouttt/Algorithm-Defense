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

        EnqueueCitizen(citizen);
    }

    public override void ShowUIController()
    {
        Managers.UI.CloseAllPopupUI();
        var controller = Managers.UI.ShowPopupUI<UI_CitizenDirectionController>();
        var pos = _camera.WorldToScreenPoint(transform.position) + (Vector3.right * 300);
        controller.transform.GetChild(0).position = pos;
        controller.Target = _directionCondition;
    }

    protected override IEnumerator LeaveTheBuilding()
    {
        yield return new WaitForSeconds(_stayTime);

        if (_citizenOrderQueue.Count == 0)
        {
            yield break;
        }

        var citizen = DequeueCitizen();

        var moveType = _directionCondition[citizen.CitizenType];
        if (moveType != Define.MoveType.None)
        {
            if (!IsRoad(moveType))
            {
                SetOpposite(citizen);
            }
            else
            {
                citizen.MoveType = moveType;
            }
        }
        else
        {
            SetOpposite(citizen);
        }

        SetPosition(citizen);
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
