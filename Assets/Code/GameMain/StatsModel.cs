using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;

public class StatsModel
{
    public float[] Stats = new float[5];
    // public float Code { get; private set; } //0
    // public float Design { get; private set; } //1
    // public float Art { get; private set; } //2
    // public float Audio { get; private set; } //3
    // public float QA { get; private set; } //4

    public StatsModel((float, float, float, float, float) stats)
    {
        Stats[0] = stats.Item1;
        Stats[1] = stats.Item2;
        Stats[2] = stats.Item3;
        Stats[3] = stats.Item4;
        Stats[4] = stats.Item5;
    }

    public StatsModel()
    {
        Reset();
    }

    public StatsModel(StatsTypes statType, float val)
    {
        Reset();
        Stats[(int)statType] = val;
    }

    public void Reset()
    {
        Array.Fill(Stats, 0);
    }

    public void StatsModify((float, float, float, float, float) statsMod)
    {
        Stats[0] = statsMod.Item1;
        Stats[1] = statsMod.Item2;
        Stats[2] = statsMod.Item3;
        Stats[3] = statsMod.Item4;
        Stats[4] = statsMod.Item5;
    }

    public void StatsModify(StatsModel statsMod)
    {
        Stats = Stats.Select((v, i) => v + statsMod.Stats[i]).ToArray();
    }

    public void StatModify(StatsTypes statType, float val)
    {
        ModifyStat((int)statType, val);
    }

    public float GetStat(StatsTypes statType)
    {
        return Stats[(int)statType];
    }

    public void GenerateRandom()
    {
        float sumStats = StrongRandom.RNG.Next(DataObjectAccess.MinSumStats * 10, DataObjectAccess.MinSumStats * 10) / 10f;
        int mainStat = StrongRandom.RNG.Next(0, 5);

        Reset();
        ModifyStat(mainStat, sumStats / 2f);

        sumStats -= sumStats / 2f;

        List<int> statsList = (new int[5]).ToList().Select((x, i) => i).Where((i) => i != mainStat).ToList();
        statsList.Shuffle();

        for (int i = 0; i < statsList.Count; i++)
        {
            if (i == statsList.Count - 1)
            {
                ModifyStat(i, sumStats);
            }
            else
            {
                var stat = StrongRandom.RNG.Next(0, (int)sumStats * 10) / 10f;
                sumStats -= stat;
                ModifyStat(i, stat);
                if (sumStats <= 0) break;
            }
        }
    }

    public void ModifyStat(int StatIndex, float val)
    {
        Stats[StatIndex] += val;
    }

    public void Normalize()
    {
        var sum = Stats.Sum();
        for (int i = 0; i < Stats.Length; i++)
            Stats[i] = Stats[i] / sum * 100f;
    }
}

public enum StatsTypes { Code, Design, Art, Audio, QA }
