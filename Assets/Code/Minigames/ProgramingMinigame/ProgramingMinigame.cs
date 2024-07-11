using System;
using DG.Tweening;
using UnityEngine;

public class ProgramingMinigame : MonoBehaviour, IMinigame
{
	public Action MinigameFinished { get; set; }
	private StatsModel gameScore;
	public StatsModel GetStatsFromGame() => gameScore;
	[SerializeField]
	private AnimationCurve teamSkillCurve = new AnimationCurve();

	public void CloseGame() {
    wordSpawner.OnGameEnd -= GameOver;
		gameContainer.transform.localScale = new Vector3(0.88f, 0.88f, 0.88f);
		gameContainer.transform.DOScale(0, initializationTime);
		gameContainer.transform.DOJump(gameContainerPosition, 1f, 1, initializationTime);
		DOVirtual.DelayedCall(initializationTime, () => gameContainer.SetActive(false), false);
		gameOverTween?.Kill();
    DOVirtual.DelayedCall(1f, () => {
		  gameMonitor.transform.DOMoveY(-10f, 0.5f);
		  PlayAudioClip(monitorSlideDownClip);
    }, false);
		PlayAudioClip(programSlideDownClip);
		// Debug.Log("Close Game");
	}

	public void ShowGame() {
		PlayAudioClip(monitorSlideUpClip);
		gameMonitor.transform.DOMoveY(0f, 0.5f);
		gameObject.SetActive(true);
		gameContainer.SetActive(true);
		gameContainer.transform.localScale = new Vector3(0, 0, 0);
		gameContainer.transform.DOScale(0.88f, initializationTime).SetDelay(0.5f);
		gameContainer.transform.DOJump(gameContainerPosition, 1f, 1, initializationTime).SetDelay(0.5f);
		wordSpawner.SetupGame();
		DOVirtual.DelayedCall(initializationTime + 0.5f, StartGame, false);
		// gameOverTween = DOVirtual.DelayedCall(GetTimeFromTeam(), GameOver, false);
    wordSpawner.OnGameEnd += GameOver;
		// Debug.Log("Show Game");
	}

	public float GetTimeFromTeam() {
		return teamSkillCurve.Evaluate(GameManager.I.StatsTeam.GetStat(StatsTypes.Audio));
	}

	public bool IsDisplayed => gameContainer.activeInHierarchy;

	Tween gameOverTween;
	private float initializationTime = 0.6f;

	public void GameOver()
	{
		int score = wordSpawner.GetScore();
		gameScore = new StatsModel(StatsTypes.Code, score / 2.5f);
		MinigameFinished?.Invoke();
		// CloseGame();
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
}
