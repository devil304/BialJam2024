using System;
using DG.Tweening;
using UnityEngine;

public class AudioMiniGame : MonoBehaviour, IMinigame
{
    public Action MinigameFinished { get; set; }
    public StatsModel GetStatsFromGame() {
			return new StatsModel((0,0,0,5,0));
		}
    public void CloseGame() {
			gameObject.SetActive(false);
			Debug.Log("Close Game");
		}

    public void ShowGame() {
			gameObject.SetActive(true);
			spawnArrowsManager.StartGame(30f);
			DOVirtual.DelayedCall(35f, GameOver, false);
			Debug.Log("Show Game");
		}

		public bool IsDisplayed() => gameObject.activeInHierarchy;

		[SerializeField]
		SpawnArrowsManager spawnArrowsManager;

		[SerializeField]
		HitPointsArrowManager hitPointsArrowManager;

		public void GameOver() {
			MinigameFinished?.Invoke();
			CloseGame();
		}
}
