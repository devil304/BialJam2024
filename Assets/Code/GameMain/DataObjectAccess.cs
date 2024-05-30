using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DataObjectAccess
{
    static DataContainer _dataContainer;
    public static Queue<string> NickNames;
    public static int MinSumStats => _dataContainer.CharMinStatsSum;
    public static int MaxSumStats => _dataContainer.CharMaxStatsSum;

    static DataObjectAccess()
    {
        _dataContainer = Resources.Load<DataContainer>("DataContainer");
    }

    public static void ClearNicks()
    {
        NickNames.Clear();
    }

    public static string GetNick()
    {
        if (NickNames.Count == 0)
            ShuffleNames();
        return NickNames.Dequeue();
    }

    static void ShuffleNames()
    {
        List<string> nicks = _dataContainer.NickNames.ToList();
        while (nicks.Count > 0)
        {
            var index = StrongRandom.RNG.Next(0, nicks.Count);
            NickNames.Enqueue(nicks[index]);
            nicks.RemoveAt(index);
        }
    }
}
