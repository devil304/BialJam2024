using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DataObjectAccess
{
    static DataContainer _dataContainer;
    public static Queue<string> NickNames = new();
    public static int MinSumStats => _dataContainer.CharMinStatsSum;
    public static int MaxSumStats => _dataContainer.CharMaxStatsSum;

    static DataObjectAccess()
    {
        _dataContainer = Resources.Load<DataContainer>("DataContainer");
    }

    public static void ClearNicks()
    {
        NickNames = null;
    }

    public static string GetNick()
    {
        if (NickNames == null)
            ShuffleNames();
        return NickNames.Dequeue();
    }

    static void ShuffleNames()
    {
        List<string> nicks = _dataContainer.NickNames.ToList();
        nicks.Shuffle();
        NickNames = new(nicks);
    }
}
