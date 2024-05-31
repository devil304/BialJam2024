using System;
using DG.Tweening;
using UnityEngine;

public class ProgramingMinigame : MonoBehaviour, IMinigame
{
    public Action MinigameFinished { get; set; }
    public StatsModel GetStatsFromGame() {
			return new StatsModel((5,0,0,0,0));
		}
    public void CloseGame() {
			transform.localScale = new Vector3(1, 1, 1);
			transform.DOScale(0, initializationTime);
			transform.DOJump(Vector3.zero, 1f, 1, initializationTime);
			DOVirtual.DelayedCall(initializationTime, () => gameObject.SetActive(false), false);
			gameOverTween?.Kill();
			Debug.Log("Close Game");
		}

    public void ShowGame() {
			gameObject.SetActive(true);
			transform.localScale = new Vector3(0, 0, 0);
			transform.DOScale(1f, initializationTime);
			transform.DOJump(Vector3.zero, 1f, 1, initializationTime);
			DOVirtual.DelayedCall(initializationTime + 0.5f, StartGame, false);
			gameOverTween = DOVirtual.DelayedCall(35f, GameOver, false);
			Debug.Log("Show Game");
		}

	public bool IsDisplayed => gameObject.activeInHierarchy;

	Tween gameOverTween;
	private float initializationTime = 0.6f;

	public void GameOver()
	{
		MinigameFinished?.Invoke();
		CloseGame();
		wordSpawner.EndGame();
	}

	public void StartGame() {
		wordSpawner.StartGame(30f);
	}

	[SerializeField]
	WordSpawner wordSpawner;
}
