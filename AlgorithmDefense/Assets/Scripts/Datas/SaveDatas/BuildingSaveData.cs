using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BuildingSaveData
{
    public string Data;
    public string OrderQueue;
    public string DirectionCondition;

    public BuildingSaveData(string data, string orderQueue)
    {
        Data = data;
        OrderQueue = orderQueue;
        DirectionCondition = null;
    }

    public BuildingSaveData(string data, string orderQueue, string directionCondition)
    {
        Data = data;
        OrderQueue = orderQueue;
        DirectionCondition = directionCondition;
    }
}
