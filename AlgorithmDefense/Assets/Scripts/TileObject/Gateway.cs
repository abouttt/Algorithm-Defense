using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gateway : BaseBuilding
{
    private Dictionary<Define.Citizen, Define.Move> _directionCondition;
    private Camera _camera;

    public override void EnterTheBuilding(CitizenController citizen)
    {
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
        while (true)
        {
            if (_citizenOrderQueue.Count == 0)
            {
                _isReleasing = false;
                yield break;
            }

            yield return new WaitForSeconds(_stayTime);

            var citizen = DequeueCitizen();

            var moveType = _directionCondition[citizen.CitizenType];
            if (moveType != Define.Move.None)
            {
                if (HasRoadNextPosition(moveType))
                {
                    citizen.MoveType = moveType;
                }
                else
                {
                    citizen.TurnAround();
                }
            }
            else
            {
                citizen.TurnAround();
            }

            citizen.SetDest();
            SetPosition(citizen);
        }
    }

    protected override void Init()
    {
        _directionCondition = new Dictionary<Define.Citizen, Define.Move>()
        {
            { Define.Citizen.Red, Define.Move.None },
            { Define.Citizen.Blue, Define.Move.None },
            { Define.Citizen.Green, Define.Move.None },
            { Define.Citizen.Yellow, Define.Move.None },
        };

        CanSelect = true;
        _camera = Camera.main;
    }
}
