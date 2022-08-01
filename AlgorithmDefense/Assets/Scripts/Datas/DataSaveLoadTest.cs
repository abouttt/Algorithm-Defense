using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaveLoadTest : MonoBehaviour
{
    [ContextMenu("[저장]")]
    void SaveJsonData()
    {
        Managers.Data.SaveData();
    }

    [ContextMenu("[로드]")]
    void LoadJsonData()
    {
        Managers.Data.LoadData();
    }

    [ContextMenu("[클리어]")]
    void ClearJsonData()
    {
        Managers.Data.Clear();
    }
}
