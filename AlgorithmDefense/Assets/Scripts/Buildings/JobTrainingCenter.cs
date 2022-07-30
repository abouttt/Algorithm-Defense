using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class JobTrainingCenter : BaseBuilding
{
    public JobTrainingCenterData Data = new JobTrainingCenterData();

    private JobTrainingCenterCommonData _commonData;

    // 테스트 변수.
    private bool _isChanged = false;
    private Define.Job _prevJob = Define.Job.None;

    // UI 존재 전 테스트 업데이트.
    private void Update()
    {
        if (Data.JobType != _prevJob)
        {
            _isChanged = true;
            _prevJob = Data.JobType;
        }

        if (_isChanged)
        {
            SetJobType(Data.JobType);
            _isChanged = false;
        }
    }

    // 건물의 직업 설정에 따라 타일을 바꾼다.
    public void SetJobType(Define.Job job)
    {
        Data.JobType = job;

        if (Data.JobType == Define.Job.Warrior ||
            Data.JobType == Define.Job.Golem)
        {
            ChangeTile("JobTrainingCenter_Warrior");
        }
        else if (Data.JobType == Define.Job.Archer ||
                 Data.JobType == Define.Job.Sniper)
        {
            ChangeTile("JobTrainingCenter_Archer");
        }
        else if (Data.JobType == Define.Job.Wizard ||
                 Data.JobType == Define.Job.FreezeWizard)
        {
            ChangeTile("JobTrainingCenter_Wizard");
        }
        else
        {
            ChangeTile("JobTrainingCenter");
        }
    }

    public override void EnterTheBuilding(CitizenController citizen)
    {
        if ((Data.JobType == Define.Job.None) ||
            (Data.MoveType == Define.Move.None) ||
            (citizen.Data.JobType != Define.Job.None))
        {
            EnqueueCitizen(citizen);
            return;
        }

        if (Data.CurrentCount == 0)
        {
            Data.CitizenType = citizen.Data.CitizenType;
        }

        Managers.Resource.Destroy(citizen.gameObject);
        Data.CurrentCount++;

        StartCoroutine(CreateJobCitizen());
    }

    protected override void Init()
    {
        _commonData = Managers.Resource.Load<JobTrainingCenterCommonData>("Datas/JobTrainingCenterCommonData");
        _baseBuildingData.HasUI = true;

        StartCoroutine(CreateJobCitizen());
    }

    protected override IEnumerator ReleaseCitizen()
    {
        while (true)
        {
            if (_baseBuildingData.CitizenOrderQueue.Count == 0)
            {
                _baseBuildingData.IsReleasing = false;
                yield break;
            }

            yield return new WaitForSeconds(_baseBuildingData.ReleaseTime);

            var citizen = DequeueCitizen();

            citizen.SetReverseMoveType();
            SetCitizenPosition(citizen);
            citizen.SetNextDestination();
        }
    }

    private IEnumerator CreateJobCitizen()
    {
        while (true)
        {
            if (!IsJobSatisfaction())
            {
                yield break;
            }

            var go = Managers.Resource.Instantiate($"{Define.BATTILE_UNIT_PATH}{Data.JobType.ToString()}Unit");
            var citizen = go.GetOrAddComponent<CitizenController>();
            citizen.Data.CitizenType = Data.CitizenType;
            citizen.Data.JobType = Data.JobType;
            citizen.Data.MoveType = Data.MoveType;
            citizen.Data.MoveSpeed = 2.0f;

            SetCitizenPosition(citizen);
            SetNextDestination(citizen);

            yield return new WaitForSeconds(_baseBuildingData.ReleaseTime * 0.5f);
        }
    }

    // 건물의 시민 카운트를 확인하여 만족했는지 확인한다.
    private bool IsJobSatisfaction()
    {
        switch (Data.JobType)
        {
            case Define.Job.Warrior:
                if (Data.CurrentCount >= _commonData.WarriorCount)
                {
                    Data.CurrentCount -= _commonData.WarriorCount;
                    return true;
                }
                break;
            case Define.Job.Archer:
                if (Data.CurrentCount >= _commonData.ArcherCount)
                {
                    Data.CurrentCount -= _commonData.ArcherCount;
                    return true;
                }
                break;
            case Define.Job.Wizard:
                if (Data.CurrentCount >= _commonData.WizardCount)
                {
                    Data.CurrentCount -= _commonData.WizardCount;
                    return true;
                }
                break;
            case Define.Job.Golem:
                if (Data.CurrentCount >= _commonData.GolemCount)
                {
                    Data.CurrentCount -= _commonData.GolemCount;
                    return true;
                }
                break;
            case Define.Job.Sniper:
                if (Data.CurrentCount >= _commonData.SniperCount)
                {
                    Data.CurrentCount -= _commonData.SniperCount;
                    return true;
                }
                break;
            case Define.Job.FreezeWizard:
                if (Data.CurrentCount >= _commonData.FreezeWizardCount)
                {
                    Data.CurrentCount -= _commonData.FreezeWizardCount;
                    return true;
                }
                break;
        }

        return false;
    }

    private void ChangeTile(string name)
    {
        var loadTile = Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}{name}");
        loadTile.gameObject = Managers.Resource.Load<GameObject>($"{Define.BUILDING_PREFAB_PATH}JobTrainingCenter");

        var cellPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Building, transform.position);

        Managers.Tile.SetTile(Define.Tilemap.Building, cellPos, loadTile);
        var newCenter = Managers.Tile.GetTilemap(Define.Tilemap.Building).GetInstantiatedObject(cellPos).GetComponent<JobTrainingCenter>();

        _baseBuildingData.CopyTo(newCenter._baseBuildingData);
        Data.CopyTo(newCenter.Data);
    }
}
