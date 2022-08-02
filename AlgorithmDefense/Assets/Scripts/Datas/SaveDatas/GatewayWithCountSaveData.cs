using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GatewayWithCountSaveData
{
    public string Data;
    public string OrderQueue;
    public string DirectionCondition;

    public GatewayWithCountSaveData(string data, string orderQueue, string directionCondition)
    {
        Data = data;
        OrderQueue = orderQueue;
        DirectionCondition = directionCondition;
    }
}
