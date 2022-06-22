using System.Collections;
using UnityEngine;

public class ClassTrainingCenter : BaseBuilding
{
    [SerializeField]
    private Define.MoveType _satisfactionMoveType = Define.MoveType.None;

    [SerializeField]
    private Define.MoveType _dissatisfactionMoveType = Define.MoveType.None;

    [SerializeField]
    private Define.ClassTier _tier;

    private bool _isSatisfaction = false;

    public void SetSatisfactionMoveType(Define.MoveType moveType) => _satisfactionMoveType = moveType;
    public void SetDissatisfactionMoveType(Define.MoveType moveType) => _dissatisfactionMoveType = moveType;

    public override void EnterTheBuilding(CitizenController citizen)
    {
        if (citizen.TempClass != Define.Class.None)
        {
            if (isSatisfactionClass(citizen))
            {
                citizen.Class = citizen.TempClass;
                citizen.Tier = _tier;
                _isSatisfaction = true;
            }
            else
            {
                citizen.ClassTrainingCount++;
            }
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
                if (HasRoadNextPosition(_satisfactionMoveType))
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

            _isSatisfaction = false;
        }
        else
        {
            if (_dissatisfactionMoveType != Define.MoveType.None)
            {
                if (HasRoadNextPosition(_dissatisfactionMoveType))
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

    private bool isSatisfactionClass(CitizenController citizen)
    {
        if (_tier == Define.ClassTier.One)
        {
            return citizen.ClassTrainingCount >= 3;
        }
        else if (_tier == Define.ClassTier.Two)
        {
            return citizen.ClassTrainingCount >= 5;
        }
        else if (_tier == Define.ClassTier.Three)
        {
            return citizen.ClassTrainingCount >= 10;
        }

        return false;
    }

    public override void ShowUIController()
    {
        // TODO
    }
}
