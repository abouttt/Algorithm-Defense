using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GatewaySaveData
{
    public string Data;
    public string OrderQueue;
    public string DirectionCondition;

    public GatewaySaveData(string data, string orderQueue, string directionCondition)
    {
        Data = data;
        OrderQueue = orderQueue;
        DirectionCondition = directionCondition;
    }
}
