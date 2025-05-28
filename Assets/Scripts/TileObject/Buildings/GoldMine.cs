using UnityEngine;

public class GoldMine : BaseBuilding
{
    public int GoldIncrease;

    private GoldAnimation _goldUI;

    public override void EnterTheBuilding(CitizenUnitController citizen)
    {
        Managers.Resource.Destroy(citizen.gameObject);
        Managers.Game.Gold += GoldIncrease;
        _goldUI.GoldSaving();
    }

    protected override void Init()
    {
        HasUI = false;
        _goldUI = FindAnyObjectByType<GoldAnimation>();
    }
}
