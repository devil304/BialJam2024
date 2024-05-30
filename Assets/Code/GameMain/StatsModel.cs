using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StatsModel
{
    public float Code { get; private set; } //0
    public float Design { get; private set; } //1
    public float Art { get; private set; } //2
    public float Audio { get; private set; } //3
    public float QA { get; private set; } //4

    public StatsModel((float, float, float, float, float) stats)
    {
        Code = stats.Item1;
        Design = stats.Item2;
        Art = stats.Item3;
        Audio = stats.Item4;
        QA = stats.Item5;
    }

    public StatsModel()
    {
        Reset();
    }

    public void Reset()
    {
        Code = 0;
        Design = 0;
        Art = 0;
        Audio = 0;
        QA = 0;
    }

    public void StatsModify((float, float, float, float, float) statsMod)
    {
        Code += statsMod.Item1;
        Design += statsMod.Item2;
        Art += statsMod.Item3;
        Audio += statsMod.Item4;
        QA += statsMod.Item5;
    }

    public void StatsModify(StatsModel statsMod)
    {
        Code += statsMod.Code;
        Design += statsMod.Design;
        Art += statsMod.Art;
        Audio += statsMod.Audio;
        QA += statsMod.QA;
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
        switch (StatIndex)
        {
            case 0:
                Code += val;
                break;
            case 1:
                Design += val;
                break;
            case 2:
                Art += val;
                break;
            case 3:
                Audio += val;
                break;
            case 4:
                QA += val;
                break;
        }
    }
}
