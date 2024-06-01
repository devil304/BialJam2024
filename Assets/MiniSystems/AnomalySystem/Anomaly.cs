using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AnomalyReaction {
	YES,
	NO,
	IGNORE
}

// public float Code { get; private set; } //0
// public float Design { get; private set; } //1
// public float Art { get; private set; } //2
// public float Audio { get; private set; } //3
// public float QA { get; private set; } //4
public class Anomaly {
	private AnomalyData anomalyData;
	private StatsModel positiveStats;
	private StatsModel negativeStats;
	private StatsModel neutralStats;

	public Anomaly(AnomalyData anomalyData) {
		this.anomalyData = anomalyData;
		SetupAnomaly();
	}

	public void OnReject() {
		GameManager.I.ModifyStats(negativeStats);
	}

	public void OnAccept() {
		GameManager.I.ModifyStats(positiveStats);
	}

	public void OnIgnore() {
		GameManager.I.ModifyStats(neutralStats);
	}

	public void SetupAnomaly() {
		Debug.Log(anomalyData.title);

		if(!anomalyData.randomPositive) {
			var psm = anomalyData.positiveStatsModel;
			positiveStats = new StatsModel((psm.codeStats, psm.designStats, psm.artStats, psm.audioStats, psm.qaStats));
		}

		if(!anomalyData.randomNegative) {
			var nsm = anomalyData.negativeStatsModel;
			negativeStats = new StatsModel((nsm.codeStats, nsm.designStats, nsm.artStats, nsm.audioStats, nsm.qaStats));
		}

		if(!anomalyData.randomNeutral) {
			var nsm = anomalyData.neutralStatsModel;
			neutralStats = new StatsModel((nsm.codeStats, nsm.designStats, nsm.artStats, nsm.audioStats, nsm.qaStats));
		}

		List<StatsTypes> winerTypes = new List<StatsTypes>();
		List<StatsTypes> loserTypes = new List<StatsTypes>();
		if (anomalyData.randomPositive) {
			DrawRandomTypes(winerTypes, loserTypes, anomalyData.randomPositiveStatsList.winerCount, anomalyData.randomPositiveStatsList.loserCount);

			positiveStats = new StatsModel();

			foreach (var winerType in winerTypes) {
				positiveStats.ModifyStat((int)winerType, anomalyData.randomPositiveStatsList.winerPoints);
			}

			foreach (var loserType in loserTypes) {
				positiveStats.ModifyStat((int)loserType, anomalyData.randomPositiveStatsList.loserPoints);
			}

		}

		if (anomalyData.randomNegative) {
			if(!anomalyData.sharedWinnerBetweenEffects) {
				winerTypes = new List<StatsTypes>();
				loserTypes = new List<StatsTypes>();
				DrawRandomTypes(winerTypes, loserTypes, anomalyData.randomNegativeStatsList.winerCount, anomalyData.randomNegativeStatsList.loserCount);
			}

			negativeStats = new StatsModel();
			foreach (var winerType in winerTypes) {
				negativeStats.ModifyStat((int)winerType, anomalyData.randomNegativeStatsList.winerPoints);
			}

			foreach (var loserType in loserTypes) {
				negativeStats.ModifyStat((int)loserType, anomalyData.randomNegativeStatsList.loserPoints);
			}
		}

		if (anomalyData.randomNeutral) {
			if(!anomalyData.sharedWinnerBetweenEffects) {
				winerTypes = new List<StatsTypes>();
				loserTypes = new List<StatsTypes>();
				DrawRandomTypes(winerTypes, loserTypes, anomalyData.randomNeutralStatsList.winerCount, anomalyData.randomNeutralStatsList.loserCount);
			}

			neutralStats = new StatsModel();
			foreach (var winerType in winerTypes) {
				neutralStats.ModifyStat((int)winerType, anomalyData.randomNeutralStatsList.winerPoints);
			}

			foreach (var loserType in loserTypes) {
				neutralStats.ModifyStat((int)loserType, anomalyData.randomNeutralStatsList.loserPoints);
			}
		}
	}

	public void DrawRandomTypes(List<StatsTypes> list1, List<StatsTypes> list2, int list1Count, int list2Count) {
		List<StatsTypes> allTypes = Enum.GetValues(typeof(StatsTypes)).Cast<StatsTypes>().ToList();
		for (int i = 0; i < list1Count; i++)
		{
			int randomIndex = UnityEngine.Random.Range(0, allTypes.Count);
			list1.Add(allTypes[randomIndex]);
			allTypes.RemoveAt(randomIndex);
		}

		for (int i = 0; i < list2Count; i++)
		{
			int randomIndex = UnityEngine.Random.Range(0, allTypes.Count);
			list2.Add(allTypes[randomIndex]);
			allTypes.RemoveAt(randomIndex);
		}
	}

	public StatsModel GetStatsModel(AnomalyReaction anomalyReaction) {
		if(anomalyReaction == AnomalyReaction.YES) return positiveStats;
		if(anomalyReaction == AnomalyReaction.NO) return negativeStats;
		if(anomalyReaction == AnomalyReaction.IGNORE) return neutralStats;
		return null;
	}

	public AnomalyData GetAnomalyData() => anomalyData;

}