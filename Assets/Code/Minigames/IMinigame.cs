using System;
using UnityEngine;

public interface IMinigame
{
    public Action MinigameFinished { get; set; }
    public StatsModel GetStatsFromGame();
    public void CloseGame();
    public void ShowGame();
    public bool IsDisplayed();
}
