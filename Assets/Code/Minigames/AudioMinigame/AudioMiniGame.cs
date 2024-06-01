using System;
using DG.Tweening;
using UnityEngine;

public class AudioMiniGame : MonoBehaviour, IMinigame
{
	public Action MinigameFinished { get; set; }
	private StatsModel gameScore;
	public StatsModel GetStatsFromGame() => gameScore;

	[SerializeField]
	private AnimationCurve teamSkillCurve = new AnimationCurve();
	public void CloseGame() {
		gameContainer.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
		gameContainer.transform.DOScale(0, initializationTime);
		gameContainer.transform.DOJump(gameContainerPosition, 1f, 1, initializationTime);
		spawnArrowsManager.SetGameTime(0f);
		DOVirtual.DelayedCall(initializationTime, () => gameContainer.SetActive(false), false);
		gameOverTween?.Kill();
		gameMonitor.transform.DOMoveY(-10f, 0.5f).SetDelay(1f);
		PlayCardClip(monitorSlideDownClip);
		Debug.Log("Close Game");
	}

	public void ShowGame() {
		PlayCardClip(monitorSlideUpClip);
		gameMonitor.transform.DOMoveY(0f, 0.5f);
		gameContainer.SetActive(true);
		gameContainer.transform.localScale = new Vector3(0, 0, 0);
		gameContainer.transform.DOScale(0.7f, initializationTime).SetDelay(0.5f);
		gameContainer.transform.DOJump(gameContainerPosition, 1f, 1, initializationTime).SetDelay(0.5f);
		float timeFromTeam = GetTimeFromTeam();
		spawnArrowsManager.SetGameTime(timeFromTeam);
		gameOverTween = DOVirtual.DelayedCall(timeFromTeam + 3f, GameOver, false);
		Debug.Log("Show Game");
	}

	public float GetTimeFromTeam() {
		return teamSkillCurve.Evaluate(GameManager.I.StatsTeam.GetStat(StatsTypes.Audio));
	}

	public bool IsDisplayed => gameContainer.activeInHierarchy;

	Tween gameOverTween;
	private float initializationTime = 0.6f;

	[SerializeField]
	SpawnArrowsManager spawnArrowsManager;

	[SerializeField]
	HitPointsArrowManager hitPointsArrowManager;
	[SerializeField]
	GameObject gameContainer;
	[SerializeField]
	Vector3 gameContainerPosition = new Vector3(0, 0.7f, 0);
	[SerializeField]
	GameObject gameMonitor;
	[SerializeField]
	private AudioClip monitorSlideUpClip;
	[SerializeField]
	private AudioClip monitorSlideDownClip;
	private void PlayCardClip(AudioClip audioClip) {
		if(audioClip != null) {
			Sound.PlaySoundAtPos(Vector3.zero, audioClip, Sound.MixerTypes.SFX, 1f, true, false, true);
		}
	}

	public void GameOver()
	{
		int score = hitPointsArrowManager.GetScore();
		gameScore = new StatsModel(StatsTypes.Audio, score / 5f);
		MinigameFinished?.Invoke();
		CloseGame();
	}

}
