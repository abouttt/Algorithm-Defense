using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MagicFactorySaveData
{
    public string Data;
    public string OrderQueue;

    public MagicFactorySaveData(string data, string orderQueue)
    {
        Data = data;
        OrderQueue = orderQueue;
    }
}
