using System.Collections;
using UnityEngine;

public class ClassTrainingCenter : BaseBuilding
{
    public static readonly string BATTLE_UNIT_PATH = "Prefabs/WorldObject/BattleUnits/";
    public static readonly string WEAPON_PATH = "Textures/Weapons/";

    [SerializeField]
    private Define.Move _satisfactionMoveType = Define.Move.None;

    [SerializeField]
    private Define.Move _dissatisfactionMoveType = Define.Move.None;

    [SerializeField]
    private Define.ClassTier _tier = Define.ClassTier.None;

    private bool _isSatisfaction = false;

    public void SetSatisfactionMoveType(Define.Move moveType) => _satisfactionMoveType = moveType;
    public void SetDissatisfactionMoveType(Define.Move moveType) => _dissatisfactionMoveType = moveType;

    public override void EnterTheBuilding(CitizenController citizen)
    {
        if (citizen.TempClass != Define.Class.None)
        {
            if (IsSatisfactionClass(citizen))
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
                    string tier = null;
                    switch(citizen.Tier)
                    {
                        case Define.ClassTier.Two:
                            tier = "2";
                            break;
                        case Define.ClassTier.Three:
                            tier = "3";
                            break;
                    }

                    var newCitizen = Managers.Resource.Instantiate($"{BATTLE_UNIT_PATH}{citizen.Class.ToString()}_Tier_{tier}").GetComponent<CitizenController>();
                    citizen.CopyTo(newCitizen);
                    newCitizen.transform.position = citizen.transform.position;

                    citizen.Clear();
                    Managers.Resource.Destroy(citizen.gameObject);
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
        while (true)
        {
            if (_citizenOrderQueue.Count == 0)
            {
                _isReleasing = false;
                yield break;
            }

            yield return new WaitForSeconds(_stayTime);

            var citizen = DequeueCitizen();

            if (_isSatisfaction)
            {
                if (_satisfactionMoveType != Define.Move.None)
                {
                    if (HasRoadNextPosition(_satisfactionMoveType))
                    {
                        citizen.MoveType = _satisfactionMoveType;
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

                _isSatisfaction = false;
            }
            else
            {
                if (_dissatisfactionMoveType != Define.Move.None)
                {
                    if (HasRoadNextPosition(_dissatisfactionMoveType))
                    {
                        citizen.MoveType = _dissatisfactionMoveType;
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
            }

            citizen.SetDest();
            SetPosition(citizen);
        }
    }

    protected override void Init()
    {
        CanSelect = true;
    }

    private bool IsSatisfactionClass(CitizenController citizen)
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
