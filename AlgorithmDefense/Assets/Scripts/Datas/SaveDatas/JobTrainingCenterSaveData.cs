using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct JobTrainingCenterSaveData
{
    public string Data;
    public string OrderQueue;

    public JobTrainingCenterSaveData(string data, string orderQueue)
    {
        Data = data;
        OrderQueue = orderQueue;
    }
}
