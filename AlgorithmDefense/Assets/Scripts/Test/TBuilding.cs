using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBuilding : MonoBehaviour
{
    public float ReleaseTime = 1.0f;
    public TBuilding LinkBuilding;

    private Queue<TMove> _moveQueue = new Queue<TMove>();
    private bool _isReleasing = false;

    private void Update()
    {
        if (LinkBuilding != null)
        {
            if (!_isReleasing)
            {
                _isReleasing = true;
                StartCoroutine(ReleaseCitizen());
            }
        }
    }

    public void EnterTheBuilding(TMove citizen)
    {
        _moveQueue.Enqueue(citizen);
        citizen.gameObject.SetActive(false);

        if (!_isReleasing)
        {
            _isReleasing = true;
            StartCoroutine(ReleaseCitizen());
        }
    }

    private IEnumerator ReleaseCitizen()
    {
        while (true)
        {
            if (_moveQueue.Count == 0)
            {
                _isReleasing = false;
                yield break;
            }

            if (!LinkBuilding)
            {
                _isReleasing = false;
                yield break;
            }

            var citizen = _moveQueue.Dequeue();
            citizen.gameObject.SetActive(true);
            citizen.Dest = LinkBuilding.gameObject.transform.position;

            yield return new WaitForSeconds(ReleaseTime);
        }
    }
}
