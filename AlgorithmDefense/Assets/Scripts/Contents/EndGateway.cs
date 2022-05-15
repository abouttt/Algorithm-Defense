using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGateway : MonoBehaviour
{
    public void EnterCitizen(CitizenController citizen)
    {
        if (citizen != null)
        {
            Managers.Game.Despawn(citizen.gameObject);
        }
    }
}
