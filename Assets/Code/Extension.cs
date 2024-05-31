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

    public static float Remap(this float value,
        float start1, float stop1,
        float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}
