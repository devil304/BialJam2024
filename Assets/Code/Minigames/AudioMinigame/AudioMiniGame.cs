using System;
using DG.Tweening;
using UnityEngine;

public class AudioMiniGame : MonoBehaviour, IMinigame
{
	public Action MinigameFinished { get; set; }
	public StatsModel GetStatsFromGame() {
		return new StatsModel((0,0,0,5,0));
	}
	[SerializeField]
	private AnimationCurve teamSkillCurve = new AnimationCurve();
	public void CloseGame() {
		int score = hitPointsArrowManager.GetScore();
		GameManager.I.ModifyStats(new StatsModel((0f,0f,0f,score/25.0f,0f)));
		transform.localScale = new Vector3(1, 1, 1);
		transform.DOScale(0, initializationTime);
		transform.DOJump(Vector3.zero, 1f, 1, initializationTime);
		spawnArrowsManager.SetGameTime(0f);
		DOVirtual.DelayedCall(initializationTime, () => gameObject.SetActive(false), false);
		gameOverTween?.Kill();
		Debug.Log("Close Game");
	}

	public void ShowGame() {
		gameObject.SetActive(true);
		transform.localScale = new Vector3(0, 0, 0);
		transform.DOScale(1f, initializationTime);
		transform.DOJump(Vector3.zero, 1f, 1, initializationTime);
		float timeFromTeam = GetTimeFromTeam();
		spawnArrowsManager.SetGameTime(timeFromTeam);
		gameOverTween = DOVirtual.DelayedCall(timeFromTeam + 3f, GameOver, false);
		Debug.Log("Show Game");
	}

	public float GetTimeFromTeam() {
		return teamSkillCurve.Evaluate(GameManager.I.StatsTeam.GetStat(StatsTypes.Audio));
	}

	public bool IsDisplayed => gameObject.activeInHierarchy;

	Tween gameOverTween;
	private float initializationTime = 0.6f;

	[SerializeField]
	SpawnArrowsManager spawnArrowsManager;

	[SerializeField]
	HitPointsArrowManager hitPointsArrowManager;

	public void GameOver()
	{
		MinigameFinished?.Invoke();
		CloseGame();
	}
}
