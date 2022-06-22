using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassGiveCenter : BaseBuilding
{
    [SerializeField]
    private Define.MoveType _satisfactionMoveType = Define.MoveType.None;

    [SerializeField]
    private Define.MoveType _dissatisfactionMoveType = Define.MoveType.None;

    private bool _isSatisfaction = false;

    public void SetSatisfactionMoveType(Define.MoveType moveType) => _satisfactionMoveType = moveType;
    public void SetDissatisfactionMoveType(Define.MoveType moveType) => _dissatisfactionMoveType = moveType;

    public override void EnterTheBuilding(CitizenController citizen)
    {
        if (citizen.ClassTrainingCount >= 3)
        {
            citizen.Class = citizen.TempClass;
            citizen.TempClass = Define.Class.None;

            if (citizen.ClassTrainingCount >= 10)
            {
                citizen.Tier = 3;
            }
            else if(citizen.ClassTrainingCount >= 5)
            {
                citizen.Tier = 2;
            }
            else if (citizen.ClassTrainingCount >= 3)
            {
                citizen.Tier = 1;
            }

            _isSatisfaction = true;
        }

        EnqueueCitizen(citizen);
    }

    protected override IEnumerator LeaveTheBuilding()
    {
        yield return new WaitForSeconds(_stayTime);

        if (_citizenOrderQueue.Count == 0)
        {
 
            yield break;
        }

        var citizen = DequeueCitizen();

        if (_isSatisfaction)
        {
            if (_satisfactionMoveType != Define.MoveType.None)
            {
                if (IsRoad(_satisfactionMoveType))
                {
                    citizen.MoveType = _satisfactionMoveType;
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
        }
        else
        {
            if (_dissatisfactionMoveType != Define.MoveType.None)
            {
                if (IsRoad(_dissatisfactionMoveType))
                {
                    citizen.MoveType = _dissatisfactionMoveType;
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
        }

        citizen.SetDest();
        SetPosition(citizen);
    }

    protected override void Init()
    {
        CanSelect = true;
    }

    public override void ShowUIController() 
    { 
        // TODO
    }
}
