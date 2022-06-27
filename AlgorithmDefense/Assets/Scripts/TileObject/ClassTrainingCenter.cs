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

    public static readonly string BATTLE_UNIT_PATH = "Prefabs/WorldObject/BattleUnits/";
    public static readonly string WEAPON_PATH = "Textures/Weapons/";

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

                if (citizen.Tier == Define.ClassTier.One)
                {
                    var weapon = Managers.Resource.Load<Sprite>($"{WEAPON_PATH}{citizen.Class.ToString()}_Tier_1");
                    citizen.SetWeapon(weapon);
                }
                else
                {
                    string tier = citizen.Tier switch
                    {
                        Define.ClassTier.One => "1",
                        Define.ClassTier.Two => "2",
                        Define.ClassTier.Three => "3",
                        _ => throw new System.NotImplementedException(),
                    };

                    var newCitizen = Managers.Resource.Instantiate($"{BATTLE_UNIT_PATH}{citizen.Class.ToString()}_Tier_{tier}").GetComponent<CitizenController>();
                    citizen.CopyTo(newCitizen);
                    newCitizen.transform.position = citizen.transform.position;

                    CitizenSpawner.GetInstance.Despawn(citizen);
                    citizen = newCitizen;
                }

                _isSatisfaction = true;
            }
            else
            {
                citizen.ClassTrainingCount++;
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
}
