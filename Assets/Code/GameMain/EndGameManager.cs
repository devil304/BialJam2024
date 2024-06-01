using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
	[SerializeField] EndData endArt;
	[SerializeField] EndData endQa;
	[SerializeField] EndData endCode;
	[SerializeField] EndData endDesign;
	[SerializeField] EndData endMusic;
	[SerializeField] EndData endWin;
	[SerializeField] EndData endLose;

	[SerializeField] TextMeshProUGUI summaryLabel;
	[SerializeField] Image summaryImage;
	[SerializeField] CanvasGroup canvasGroup;

	private int minScoreToWin = 80;

	private void Start() {
		canvasGroup.alpha = 0;
		canvasGroup.gameObject.SetActive(false);
	}

	public void EndGame() {
		HandleInformation();
		canvasGroup.gameObject.SetActive(true);
		canvasGroup.DOFade(1, 2f);
	}

	public void HandleInformation() {
		float[] allScores = GameManager.I.StatsAct.Stats;
		if (IsWin(allScores)) {
			SetupInformation(endWin);
			return;
		}


		float biggestScore = minScoreToWin;
		int biggestIndex = -1;
		for (int i = 0; i < allScores.Length; i++) {
			if(allScores[i] >= biggestScore) {
				if (allScores[i] == biggestScore) {
					if (UnityEngine.Random.Range(0, 2) == 1)
						continue;
				}
				biggestScore = allScores[i];
				biggestIndex = i;
			}
		}
		if (biggestIndex < 0) {
			SetupInformation(endLose);
			return;
		}

		switch(biggestIndex) {
			case (int)StatsTypes.Art:
				SetupInformation(endArt);
			break;
			case (int)StatsTypes.Code:
				SetupInformation(endCode);
			break;
			case (int)StatsTypes.Audio:
				SetupInformation(endMusic);
			break;
			case (int)StatsTypes.QA:
				SetupInformation(endQa);
			break;
			case (int)StatsTypes.Design:
				SetupInformation(endDesign);
			break;
		}
	}

	public bool IsWin(float[] allScores) {
		foreach (var score in allScores) {
			if (score < minScoreToWin) return false;
		}
		return true;
	}

	public void SetupInformation(EndData endData) {
		summaryLabel.text = endData.endText;
		Color textColor;
		ColorUtility.TryParseHtmlString($"#{endData.endColor}", out textColor);
		summaryLabel.color = textColor;
		summaryImage.sprite = endData.endSprite;
	}

	public void ReturnToMenu() {
		GameManager.I.LoadMenu();
	}

	public void ExitGame() {
		Application.Quit();
	}
}

[Serializable]
public struct EndData {
	public Sprite endSprite;
	public string endText;
	public string endColor;
}
