using System;
using DG.Tweening;
using UnityEngine;

public class ProgramingMinigame : MonoBehaviour, IMinigame
{
	public Action MinigameFinished { get; set; }
	public StatsModel GetStatsFromGame() {
		return new StatsModel((5,0,0,0,0));
	}
	[SerializeField]
	private AnimationCurve teamSkillCurve = new AnimationCurve();
	public void CloseGame() {
		int score = wordSpawner.GetScore();
		GameManager.I.ModifyStats(new StatsModel((score/5.0f,0f,0f,0f,0f)));
		wordSpawner.EndGame();
		gameContainer.transform.localScale = new Vector3(0.88f, 0.88f, 0.88f);
		gameContainer.transform.DOScale(0, initializationTime);
		gameContainer.transform.DOJump(gameContainerPosition, 1f, 1, initializationTime);
		DOVirtual.DelayedCall(initializationTime, () => gameContainer.SetActive(false), false);
		gameOverTween?.Kill();
		gameMonitor.transform.DOMoveY(-10f, 0.5f).SetDelay(1f);
		Debug.Log("Close Game");
	}

	public void ShowGame() {
		gameMonitor.transform.DOMoveY(0f, 0.5f);
		gameContainer.SetActive(true);
		gameContainer.transform.localScale = new Vector3(0, 0, 0);
		gameContainer.transform.DOScale(0.88f, initializationTime).SetDelay(0.5f);
		gameContainer.transform.DOJump(gameContainerPosition, 1f, 1, initializationTime).SetDelay(0.5f);
		wordSpawner.SetupGame();
		DOVirtual.DelayedCall(initializationTime + 0.5f, StartGame, false);
		gameOverTween = DOVirtual.DelayedCall(GetTimeFromTeam(), GameOver, false);
		Debug.Log("Show Game");
	}

	public float GetTimeFromTeam() {
		return teamSkillCurve.Evaluate(GameManager.I.StatsTeam.GetStat(StatsTypes.Audio));
	}

	public bool IsDisplayed => gameContainer.activeInHierarchy;

	Tween gameOverTween;
	private float initializationTime = 0.6f;

	public void GameOver()
	{
		MinigameFinished?.Invoke();
		CloseGame();
	}

	public void StartGame() {
		wordSpawner.StartGame(GetTimeFromTeam());
	}

	[SerializeField]
	WordSpawner wordSpawner;

	[SerializeField]
	GameObject gameContainer;
	[SerializeField]
	Vector3 gameContainerPosition = new Vector3(0, 0.7f, 0);
	[SerializeField]
	GameObject gameMonitor;
}
