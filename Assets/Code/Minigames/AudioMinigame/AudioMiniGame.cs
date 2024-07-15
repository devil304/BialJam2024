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
		// spawnArrowsManager.SetGameTime(0f);
    spawnArrowsManager.OnEndGame -= GameOver;
		DOVirtual.DelayedCall(initializationTime, () => gameContainer.SetActive(false), false);
		gameOverTween?.Kill();
    DOVirtual.DelayedCall(1f, () => {
		  gameMonitor.transform.DOMoveY(-15f, 0.5f);
		  PlayAudioClip(monitorSlideDownClip);
    }, false);
		PlayAudioClip(programSlideDownClip);
		// Debug.Log("Close Game");
	}

	public void ShowGame() {
		PlayAudioClip(monitorSlideUpClip);
		gameMonitor.transform.DOMoveY(-0.15f, 0.5f);
		gameObject.SetActive(true);
		gameContainer.SetActive(true);
		gameContainer.transform.localScale = new Vector3(0, 0, 0);
		gameContainer.transform.DOScale(0.78f, initializationTime).SetDelay(0.5f);
		gameContainer.transform.DOJump(gameContainerPosition, 1f, 1, initializationTime).SetDelay(0.5f);
		float timeFromTeam = GetTimeFromTeam();
    spawnArrowsManager.OnEndGame += GameOver;
		spawnArrowsManager.SetGameTime(timeFromTeam);
		// gameOverTween = DOVirtual.DelayedCall(timeFromTeam + 3f, GameOver, false);
		// Debug.Log("Show Game");
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
	[SerializeField]
	private AudioClip programSlideDownClip;
	private void PlayAudioClip(AudioClip audioClip) {
		if(audioClip != null) {
			Sound.PlaySoundAtPos(Vector3.zero, audioClip, Sound.MixerTypes.SFX, 1f, true, false, true);
		}
	}

	public void GameOver()
	{
		int score = hitPointsArrowManager.GetScore();
		gameScore = new StatsModel(StatsTypes.Audio, score / 5f);
		MinigameFinished?.Invoke();
		// CloseGame();
	}

}
