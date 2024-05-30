using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StatsModel
{
    public float Code { get; private set; }
    public float Design { get; private set; }
    public float Art { get; private set; }
    public float Audio { get; private set; }
    public float QA { get; private set; }

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
        int sumStats = StrongRandom.RNG.Next(DataObjectAccess.MinSumStats, DataObjectAccess.MinSumStats);
        int mainStat = StrongRandom.RNG.Next(0, 5);
        switch (mainStat)
        {
            case 0:
                Code = sumStats / 2;
                break;
            case 1:
                Design = sumStats / 2;
                break;
            case 2:
                Art = sumStats / 2;
                break;
            case 3:
                Audio = sumStats / 2;
                break;
            case 4:
                QA = sumStats / 2;
                break;
        }

        sumStats -= sumStats / 2;

        List<int> statsList = (new int[5]).ToList().Select((x, i) => i).Where((i) => i != mainStat).ToList();
        statsList.Shuffle();

    }
}
