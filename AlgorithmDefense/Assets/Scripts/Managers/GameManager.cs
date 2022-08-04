using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public int[] MagicCounts { get; private set; }

    public void Init()
    {
        MagicCounts = new int[Enum.GetValues(typeof(Define.Magic)).Length - 1];
    }

    public void Clear()
    {

    }
}
