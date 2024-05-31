using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AnomalyReaction {
	YES,
	NO,
	IGNORE
}

public class Anomaly {
	private AnomalyData anomalyData;
	private StatsModel positiveStats;
	private StatsModel negativeStats;
	private StatsModel neutralStats;

	public void OnReject() {
		GameManager.I.ModifyStats(negativeStats);
	}

	public void OnAccept() {
		GameManager.I.ModifyStats(positiveStats);
	}

	public void OnIgnore() {
		GameManager.I.ModifyStats(neutralStats);
	}

	public void SetupAnomaly(AnomalyData anomalyData) {
		this.anomalyData = anomalyData;
		int typesCount = Enum.GetNames(typeof(StatsTypes)).Length;

		if (anomalyData.randomPositive) {
			List<StatsTypes> winerTypes = new List<StatsTypes>();
			for (int i = 0; i < anomalyData.randomPositiveStatsList.winerCount; i++)
			{
				winerTypes.Add((StatsTypes)Enum.GetValues(typeof(StatsTypes)).GetValue(UnityEngine.Random.Range(0, typesCount)));
			}
		}
	}

}