using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class JobTrainingCenter : BaseBuilding
{
    public Define.Move MoveType = Define.Move.None;

    [field: SerializeField]
    public int CurrentCount { get; private set; }

    [SerializeField]
    private Define.Job _jobType = Define.Job.None;
    private JobTrainingCenterCommonData _commonData;
    private Define.Citizen _citizenType = Define.Citizen.None;

    // 테스트 변수.
    private bool _isChanged = false;
    private Define.Job _prevJob = Define.Job.None;

    // UI 존재 전 테스트 업데이트.
    private void Update()
    {
        if (_jobType != _prevJob)
        {
            _isChanged = true;
            _prevJob = _jobType;
        }

        if (_isChanged)
        {
            SetJobType(_jobType);
            _isChanged = false;
        }
    }

    // 건물의 직업 설정에 따라 타일을 바꾼다.
    public void SetJobType(Define.Job job)
    {
        _jobType = job;

        if (_jobType == Define.Job.Warrior ||
            _jobType == Define.Job.Golem)
        {
            ChangeTile("JobTrainingCenter_Warrior");
        }
        else if (_jobType == Define.Job.Archer ||
                 _jobType == Define.Job.Sniper)
        {
            ChangeTile("JobTrainingCenter_Archer");
        }
        else if (_jobType == Define.Job.Wizard ||
                 _jobType == Define.Job.FreezeWizard)
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
        if ((_jobType == Define.Job.None) ||
            (MoveType == Define.Move.None) ||
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

        StartCoroutine(CreateJobCitizen());
    }

    public override void CreateSaveData()
    {
        string data = JsonUtility.ToJson(this, true);
        string q = JsonUtility.ToJson(new SerializationQueue<CitizenOrderQueueData>(_citizenOrderQueue), true);
        Managers.Data.JobTrainingCenterSaveDatas.Enqueue(JsonUtility.ToJson(new JobTrainingCenterSaveData(data, q), true));
    }

    public override void LoadSaveData()
    {
        var saveData = JsonUtility.FromJson<JobTrainingCenterSaveData>(Managers.Data.JobTrainingCenterSaveDatas.Dequeue());

        JsonUtility.FromJsonOverwrite(saveData.Data, this);
        _citizenOrderQueue =
            JsonUtility.FromJson<SerializationQueue<CitizenOrderQueueData>>(saveData.OrderQueue).ToQueue();

        if (!_isReleasing)
        {
            _isReleasing = true;
            StartCoroutine(ReleaseCitizen());
        }
    }

    protected override void Init()
    {
        _commonData = Managers.Resource.Load<JobTrainingCenterCommonData>("Datas/JobTrainingCenterCommonData");
        HasUI = true;
        StartCoroutine(CreateJobCitizen());
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

            var go = Managers.Resource.Instantiate($"{Define.BATTILE_UNIT_PATH}{_jobType.ToString()}Unit");
            var citizen = go.GetOrAddComponent<CitizenController>();
            citizen.Data.CitizenType = _citizenType;
            citizen.Data.JobType = _jobType;
            citizen.Data.MoveType = MoveType;
            citizen.Data.MoveSpeed = 2.0f;

            SetCitizenPosition(citizen);
            SetNextDestination(citizen);

            yield return new WaitForSeconds(_releaseTime * 0.5f);
        }
    }

    // 건물의 시민 카운트를 확인하여 만족했는지 확인한다.
    private bool IsJobSatisfaction()
    {
        if (_jobType == Define.Job.None)
        {
            return false;
        }

        if (CurrentCount >= _commonData.JobCountData[(int)_jobType - 1])
        {
            CurrentCount -= _commonData.JobCountData[(int)_jobType - 1];
            return true;
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

        CopyTo(newCenter);
    }

    private void CopyTo(JobTrainingCenter other)
    {
        base.CopyTo(other);

        other._jobType = _jobType;
        other.MoveType = MoveType;
        other.CurrentCount = CurrentCount;
        other._citizenType = _citizenType;
    }
}
