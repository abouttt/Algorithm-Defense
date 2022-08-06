using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CampSaveData
{
    public string Data;
    public string OrderQueue;

    public CampSaveData(string data, string orderQueue)
    {
        Data = data;
        OrderQueue = orderQueue;
    }
}
