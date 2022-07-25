using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaveLoadTest : MonoBehaviour
{
    public bool Save;
    public bool Load;
    public bool Clear;

    private void Update()
    {
        if (Save)
        {
            Save = false;
            Managers.Data.SaveTilemaps();
        }

        if (Load)
        {
            Load = false;
            Managers.Data.LoadTilemaps();
        }

        if (Clear)
        {
            Clear = false;
            Managers.Data.Clear();
        }
    }
}
