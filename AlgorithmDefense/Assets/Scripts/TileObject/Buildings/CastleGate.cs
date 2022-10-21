using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CastleGate : BaseBuilding
{
    private Queue<CitizenData> _citizenOrderQueue = new();
    private bool _isReleasing;

    public override void EnterTheBuilding(CitizenController citizen)
    {
        _citizenOrderQueue.Enqueue(citizen.Data);

        Managers.Resource.Destroy(citizen.gameObject);

        if (!_isReleasing)
        {
            _isReleasing = true;
            StartCoroutine(ReleaseCitizen());
        }
    }

    protected IEnumerator ReleaseCitizen()
    {
        while (true)
        {
            if (_citizenOrderQueue.Count == 0)
            {
                _isReleasing = false;
                yield break;
            }

            yield return new WaitForSeconds(_releaseTime);

            var citizen = DequeueCitizen(_citizenOrderQueue);

            if (citizen.Data.JobType != Define.Job.None)
            {
                citizen.Data.MoveType = Define.Move.Up;
                citizen.GetComponent<CitizenController>().enabled = false;
                citizen.GetComponent<UnitManager>().enabled = true;
                citizen.GetComponent<UnitAI>().enabled = true;
            }
            else
            {
                citizen.SetReverseMoveType();
            }

            SetCitizenPosition(citizen);
            citizen.SetNextDestination(transform.position);
        }
    }

    protected override void Init()
    {
        HasUI = false;
    }
}
