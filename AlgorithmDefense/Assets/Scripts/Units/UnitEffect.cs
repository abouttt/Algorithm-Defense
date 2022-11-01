using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEffect : MonoBehaviour
{
    private float _speed = 5f;

    private void OnEnable()
    {
        Invoke("DestroyArrow", 0.2f);
    }

    private void Update()
    {
        transform.Translate(Vector2.right * _speed * Time.deltaTime);
    }

    private void DestroyArrow()
    {
        Managers.Resource.Destroy(gameObject);
    }
}
