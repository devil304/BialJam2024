using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnomalyData", menuName = "Scriptable Objects/AnomalyData")]
public class AnomalyData : ScriptableObject
{
	public string title;
	public Sprite imageSprite;
	public string description;

	public StatsModelNumbers positiveStatsModel;
	public StatsModelNumbers negativeStatsModel;
	public StatsModelNumbers neutralStatsModel;
	public bool randomPositive;
	public bool randomNegative;
	public bool randomNeutral;

	[Tooltip("Winers are sets on positive draws, and have loser points on negative user choice")]
	public bool sharedWinnerBetweenEffects;
	public RandomsEffects randomPositiveStatsList;
	public RandomsEffects randomNegativeStatsList;
	public RandomsEffects randomNeutralStatsList;

}
[Serializable]
public struct RandomsEffects {
	public int winerPoints;
	public int winerCount;
	public int loserPoints;
	public int loserCount;

}

[Serializable]
public struct StatsModelNumbers {
	public int codeStats;
	public int qaStats;
	public int audioStats;
	public int artStats;
	public int designStats;
}
