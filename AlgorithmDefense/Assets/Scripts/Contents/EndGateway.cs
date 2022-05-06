using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGateway : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var citizen = collision.GetComponent<CitizenController>();
        if (citizen != null)
        {
            Managers.Game.Despawn(citizen.gameObject);
        }
    }
}
