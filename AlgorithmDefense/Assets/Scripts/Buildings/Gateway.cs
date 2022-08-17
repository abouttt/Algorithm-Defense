using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gateway : BaseBuilding
{
    public Dictionary<Define.Citizen, Define.Move> DirectionCondition { get; private set; }

    private UI_CitizenDirectionController Controller;
    private Camera _camera;

    // 테스트 변수.
    public Define.Move Red = Define.Move.None;
    public Define.Move Green = Define.Move.None;
    public Define.Move Blue = Define.Move.None;
    public Define.Move Yellow = Define.Move.None;
    public bool SetChangeValue = false;

    // 테스트 업데이트.
    private void Update()
    {
        if (SetChangeValue)
        {
            //설정된 데이터 입력
            DirectionCondition[Define.Citizen.Red] = Controller.ToggleDirection[Define.Citizen.Red];
            DirectionCondition[Define.Citizen.Green] = Controller.ToggleDirection[Define.Citizen.Green];
            DirectionCondition[Define.Citizen.Blue] = Controller.ToggleDirection[Define.Citizen.Blue];
            DirectionCondition[Define.Citizen.Yellow] = Controller.ToggleDirection[Define.Citizen.Yellow];
            SetChangeValue = false;


            //입력한 데이터 저장
            Red = Controller.ToggleDirection[Define.Citizen.Red];
            Green = Controller.ToggleDirection[Define.Citizen.Green];
            Blue = Controller.ToggleDirection[Define.Citizen.Blue];
            Yellow = Controller.ToggleDirection[Define.Citizen.Yellow];


            Debug.Log("-------------------------------------");
            Debug.Log("Red: " + $"{DirectionCondition[Define.Citizen.Red]}");
            Debug.Log("green: " + $"{DirectionCondition[Define.Citizen.Green]}");
            Debug.Log("Blue: " + $"{DirectionCondition[Define.Citizen.Blue]}");
            Debug.Log("yellow: " + $"{DirectionCondition[Define.Citizen.Yellow]}");

            Controller = null;
        }
    }

    public override void GateWayInformationTransfer(GameObject clone)
    {

        Controller = FindObjectOfType<UI_CitizenDirectionController>();

        //건물 오른쪽에 생성되도록 좌표지정
        var pos = _camera.WorldToScreenPoint(transform.position) + (Vector3.right * 300);
        Controller.transform.GetChild(0).position = pos;

        Controller.ToggleDirection = DirectionCondition;
        Controller.SetDirection(clone);

    }

    public override void EnterTheBuilding(CitizenController citizen)
    {
        EnqueueCitizen(citizen);
    }

    public override void CreateSaveData()
    {
        string data = JsonUtility.ToJson(this, true);
        string q = JsonUtility.ToJson(new SerializationQueue<CitizenOrderQueueData>(_citizenOrderQueue), true);
        string dc = JsonUtility.ToJson(new SerializationDictionary<Define.Citizen, Define.Move>(DirectionCondition), true);
        Managers.Data.GatewaySaveDatas.Enqueue(JsonUtility.ToJson(new GatewaySaveData(data, q, dc), true));
    }

    public override void LoadSaveData()
    {
        var saveData = JsonUtility.FromJson<GatewaySaveData>(Managers.Data.GatewaySaveDatas.Dequeue());

        JsonUtility.FromJsonOverwrite(saveData.Data, this);
        _citizenOrderQueue =
            JsonUtility.FromJson<SerializationQueue<CitizenOrderQueueData>>(saveData.OrderQueue).ToQueue();
        DirectionCondition =
            JsonUtility.FromJson<SerializationDictionary<Define.Citizen, Define.Move>>(saveData.DirectionCondition).ToDictionary();

        if (!_isReleasing)
        {
            _isReleasing = true;
            StartCoroutine(ReleaseCitizen());
        }
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

            var directionConditionMoveType = DirectionCondition[citizen.Data.CitizenType];
            if (directionConditionMoveType == Define.Move.None)
            {
                citizen.SetReverseMoveType();
            }
            else
            {
                if (IsRoadNextPosition(directionConditionMoveType))
                {
                    citizen.Data.MoveType = directionConditionMoveType;
                }
                else
                {
                    citizen.SetReverseMoveType();
                }
            }

            SetCitizenPosition(citizen);
            SetNextDestination(citizen);
        }
    }

    protected override void Init()
    {
        if (DirectionCondition == null)
        {
            DirectionCondition = new Dictionary<Define.Citizen, Define.Move>()
            {
                { Define.Citizen.Red, Define.Move.None },
                { Define.Citizen.Green, Define.Move.None },
                { Define.Citizen.Blue, Define.Move.None },
                { Define.Citizen.Yellow, Define.Move.None },
            };
        }

        HasUI = true;

        _camera = Camera.main;
    }
}
