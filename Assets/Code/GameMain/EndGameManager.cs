using System;
using System.Collections.Generic;
using System.Linq;
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
	[SerializeField] CanvasGroup canvasGroup;

  [SerializeField] Sprite successSprite;
  [SerializeField] Sprite failSprite;

  [SerializeField] Image programingStatus;
  [SerializeField] Image artStatus;
  [SerializeField] Image audioStatus;
  [SerializeField] Image qaStatus;
  [SerializeField] Image designStatus;

	private int minScoreToWin = 75;

	private void Start() {
		canvasGroup.alpha = 0;
		canvasGroup.gameObject.SetActive(false);
	}

	public void EndGame() {
    var allStats = GameManager.I.StatsAct;
    Debug.Log("Write Results:");
    Debug.Log(allStats.Stats.ToArray());
		for (int i = 0; i < GameManager.I.StatsAct.Stats.Length; i++) {
      Debug.Log(allStats.Stats[i]);
      Debug.Log((StatsTypes)i);
		}
		HandleInformation();
		canvasGroup.gameObject.SetActive(true);
		canvasGroup.DOFade(1, 2f);
	}

	public void HandleInformation() {
		float[] allScores = GameManager.I.StatsAct.Stats;
		if (IsWin(allScores)) {
			SetupInformation(endWin);
      artStatus.sprite = successSprite;
      programingStatus.sprite = successSprite;
      audioStatus.sprite = successSprite;
      qaStatus.sprite = successSprite;
      designStatus.sprite = successSprite;
			return;
		}


    var allStats = GameManager.I.StatsAct;
    List<StatsTypes> winingTypes = new();
    int winningIndex = -1;
		for (int i = 0; i < allStats.Stats.Length; i++) {
      if (allStats.Stats[i] >= minScoreToWin) {
        winingTypes.Add((StatsTypes)i);
        winningIndex = i;
      }
		}

		if (winingTypes.Count <= 0) {
			SetupInformation(endLose);
			return;
		}

    if (winingTypes.Count == 1) {
      switch(winningIndex) {
        case (int)StatsTypes.Art:
          SetupInformation(endArt);
          artStatus.sprite = successSprite;
        break;
        case (int)StatsTypes.Code:
          SetupInformation(endCode);
          programingStatus.sprite = successSprite;
        break;
        case (int)StatsTypes.Audio:
          SetupInformation(endMusic);
          audioStatus.sprite = successSprite;
        break;
        case (int)StatsTypes.QA:
          SetupInformation(endQa);
          qaStatus.sprite = successSprite;
        break;
        case (int)StatsTypes.Design:
          SetupInformation(endDesign);
          designStatus.sprite = successSprite;
        break;
      }
      return;
    }

    CreateInformation(winingTypes);
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
	}

// public enum StatsTypes { Code, Design, Art, Audio, QA }
  public void CreateInformation(List<StatsTypes> statsTypes) {
    string summaryText = "Your team created ";
    EndResult? bestResult = null;
    int winCount = 0;

    if(statsTypes.Contains(StatsTypes.Code)) {
      summaryText += $"<color=#{endCode.endColor}>{endCode.adjective}</color>, ";
      programingStatus.sprite = successSprite;
      winCount++;
      bestResult = GetBestResult(endCode.endResult, bestResult);
    }

    if(statsTypes.Contains(StatsTypes.Design)) {
      summaryText += $"<color=#{endDesign.endColor}>{endDesign.adjective}</color>, ";
      designStatus.sprite = successSprite;
      winCount++;
      bestResult = GetBestResult(endDesign.endResult, bestResult);
    }
    
    if(statsTypes.Contains(StatsTypes.Art)) {
      summaryText += $"<color=#{endArt.endColor}>{endArt.adjective}</color>, ";
      artStatus.sprite = successSprite;
      winCount++;
      bestResult = GetBestResult(endArt.endResult, bestResult);
    }
    
    if(statsTypes.Contains(StatsTypes.Audio)) {
      summaryText += $"<color=#{endMusic.endColor}>{endMusic.adjective}</color>, ";
      audioStatus.sprite = successSprite;
      winCount++;
      bestResult = GetBestResult(endMusic.endResult, bestResult);
    }
    
    if(statsTypes.Contains(StatsTypes.QA)) {
      summaryText += $"<color=#{endQa.endColor}>{endQa.adjective}</color>, ";
      qaStatus.sprite = successSprite;
      winCount++;
      bestResult = GetBestResult(endQa.endResult, bestResult);
    }

    int lastComma = summaryText.LastIndexOf(",");
    summaryText = summaryText.Remove(lastComma);

    Debug.Log(bestResult);
    Debug.Log(bestResult?.noun);
    if(winCount < 3 && bestResult != null) {
      summaryText += $" {bestResult?.noun}.";
    } else {
      summaryText += " game.";
    }

		summaryLabel.text = $"{summaryText}\nTry again.";
  }

  private EndResult? GetBestResult(EndResult? endResult, EndResult? bestResult) {
    if (bestResult == null && endResult != null)
      return endResult;

    if(endResult == null && bestResult != null) {
      return bestResult;
    }

    if(endResult != null && bestResult != null && bestResult?.power < endResult?.power) {
      return endResult;
    }

    return bestResult;
  }

	public void ReturnToMenu() {
		GameManager.I.LoadMenu();
	}

	public void ExitGame() {
		Application.Quit();
	}

	public int GetMinScoreToWin() => minScoreToWin;
}

[Serializable]
public struct EndData {
  public StatsTypes statsType;
	public string endText;
	public string endColor;
  public string adjective;
  public EndResult endResult;
}

[Serializable]
public struct EndResult {
  public string noun;
  public int power;
}