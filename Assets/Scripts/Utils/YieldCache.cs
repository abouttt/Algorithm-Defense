using System.Collections.Generic;
using UnityEngine;

public static class YieldCache
{
    private static readonly Dictionary<float, WaitForSeconds> s_wfs = new();

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        if (!s_wfs.TryGetValue(seconds, out var wfs))
        {
            wfs = new(seconds);
            s_wfs.Add(seconds, wfs);
        }

        return wfs;
    }
}
