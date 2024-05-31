public enum AnomalyReaction {
	YES,
	NO,
	IGNORE
}

public class Anomaly {
	private AnomalySystem anomalySystem;
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

}