using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassGiveCenter : BaseBuilding
{
    [SerializeField]
    private Define.Class _tempClass = Define.Class.None;

    [SerializeField]
    private Define.Move _moveType = Define.Move.None;

    public void SetTempClass(Define.Class classType) => _tempClass = classType;
    public void SetMoveType(Define.Move moveType) => _moveType = moveType;

    public override void EnterTheBuilding(CitizenController citizen)
    {
        if (citizen.Class == Define.Class.None)
        {
            if (citizen.TempClass != _tempClass)
            {
                citizen.ClassTrainingCount = 0;
            }

            if (_tempClass != Define.Class.None)
            {
                citizen.TempClass = _tempClass;
            }
        }

        EnqueueCitizen(citizen);
    }

    public override void ShowUIController()
    {
        
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

            if (_moveType != Define.Move.None)
            {
                if (HasRoadNextPosition(_moveType))
                {
                    citizen.MoveType = _moveType;
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
        CanSelect = true;
    }
}
