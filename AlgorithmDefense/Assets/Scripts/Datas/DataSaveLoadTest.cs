using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaveLoadTest : MonoBehaviour
{
    [ContextMenu("[����]")]
    void SaveJsonData()
    {
        Managers.Data.SaveData();
    }

    [ContextMenu("[�ε�]")]
    void LoadJsonData()
    {
        Managers.Data.LoadData();
    }

    [ContextMenu("[Ŭ����]")]
    void ClearJsonData()
    {
        Managers.Data.Clear();
    }
}
