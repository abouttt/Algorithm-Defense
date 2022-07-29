using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class JobTrainingCenter : BaseBuilding
{
    public Define.Job JobType = Define.Job.None;
    public Define.Move MoveType = Define.Move.None;
    public int CurrentCount;

    private JobTrainingCenterData _data;
    private Define.Citizen _citizenType = Define.Citizen.None;

    // UI 존재 전 테스트 업데이트.
    private void Update()
    {
        //SetJobType(JobType);
    }

    public void SetJobType(Define.Job job)
    {
        if (job == Define.Job.None)
        {
            return;
        }

        JobType = job;
        var cellPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Building, transform.position);

        if (JobType == Define.Job.Warrior ||
            JobType == Define.Job.Golem)
        {
            var tile = Managers.Resource.Load<TileBase>($"Tiles/Buildings/JobTrainingCenter_Warrior");
            Managers.Tile.SetTile(Define.Tilemap.Building, cellPos, tile);
        }
        else if (JobType == Define.Job.Archer ||
                 JobType == Define.Job.Sniper)
        {
            var tile = Managers.Resource.Load<TileBase>($"Tiles/Buildings/JobTrainingCenter_Archer");
            Managers.Tile.SetTile(Define.Tilemap.Building, cellPos, tile);
        }
        else if (JobType == Define.Job.Warrior ||
                 JobType == Define.Job.FreezeWizard)
        {
            var tile = Managers.Resource.Load<TileBase>($"Tiles/Buildings/JobTrainingCenter_Wizard");
            Managers.Tile.SetTile(Define.Tilemap.Building, cellPos, tile);
        }
    }

    public override void EnterTheBuilding(CitizenController citizen)
    {
        if ((JobType == Define.Job.None) ||
            (citizen.Data.JobType != Define.Job.None))
        {
            EnqueueCitizen(citizen);
            return;
        }

        if (CurrentCount == 0)
        {
            _citizenType = citizen.Data.CitizenType;
        }

        Managers.Resource.Destroy(citizen.gameObject);
        CurrentCount++;
        
        if (IsJobSatisfaction())
        {
            var go = Managers.Resource.Instantiate($"{Define.BATTILE_UNIT_PATH}{JobType.ToString()}Unit");
            var newCitizen = go.GetOrAddComponent<CitizenController>();
            newCitizen.Data.CitizenType = _citizenType;
            newCitizen.Data.JobType = JobType;
            newCitizen.Data.MoveType = MoveType;
            newCitizen.Data.MoveSpeed = 2.0f;

            SetCitizenPosition(newCitizen);
            newCitizen.SetNextDestination();

            CurrentCount = 0;
        }
    }

    protected override void Init()
    {
        _data = Managers.Resource.Load<JobTrainingCenterData>("Datas/JobTrainingCenterData");
    }

    protected override IEnumerator ReleaseCitizen()
    {
        while (true)
        {
            if (_citizenOrderQueue.Count == 0)
            {
                _isReleasing = false;
                yield break;
            }

            yield return new WaitForSeconds(_releaseTime);

            var citizen = DequeueCitizen();

            citizen.SetReverseMoveType();
            citizen.SetNextDestination();
            SetCitizenPosition(citizen);
        }
    }

    private bool IsJobSatisfaction()
    {
        switch (JobType)
        {
            case Define.Job.Warrior:
                return CurrentCount == _data.WarriorCount;
            case Define.Job.Archer:
                return CurrentCount == _data.ArcherCount;
            case Define.Job.Wizard:
                return CurrentCount == _data.WizardCount;
            case Define.Job.Golem:
                return CurrentCount == _data.GolemCount;
            case Define.Job.Sniper:
                return CurrentCount == _data.SniperCount;
            case Define.Job.FreezeWizard:
                return CurrentCount == _data.FreezeWizardCount;
        }

        return false;
    }
}
