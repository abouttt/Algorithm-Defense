using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassGiveCenter : BaseBuilding
{
    [SerializeField]
    private Define.Class _tempClass = Define.Class.None;

    [SerializeField]
    private Define.MoveType _moveType = Define.MoveType.None;

    public void SetTempClass(Define.Class classType) => _tempClass = classType;
    public void SetMoveType(Define.MoveType moveType) => _moveType = moveType;

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
        yield return new WaitForSeconds(_stayTime);

        if (_citizenOrderQueue.Count == 0)
        {
            yield break;
        }

        var citizen = DequeueCitizen();

        if (_moveType != Define.MoveType.None)
        {
            if (HasRoadNextPosition(_moveType))
            {
                citizen.MoveType = _moveType;
            }
            else
            {
                SetOpposite(citizen);
            }
        }
        else
        {
            SetOpposite(citizen);
        }

        citizen.SetDest();
        SetPosition(citizen);
    }

    protected override void Init()
    {
        CanSelect = true;
    }
}
