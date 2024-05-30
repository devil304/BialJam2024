using System.Collections.Generic;
using UnityEngine;
using System;

public static class Extension
{
    public static void Shuffle<T>(this IList<T> list)
    {
        if (list.Count <= 1)
        {
            return;
        }

        for (int i = 0; i < list.Count; i++)
        {
            int newIndex = StrongRandom.RNG.Next(0, list.Count);
            T x = list[i];
            list[i] = list[newIndex];
            list[newIndex] = x;
        }
    }
}
