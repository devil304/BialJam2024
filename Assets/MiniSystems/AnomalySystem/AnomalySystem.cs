using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnomalySystem : MonoBehaviour
{
	[SerializeField]
	JumpingText alertInfoText;

	[SerializeField]
	CanvasGroup decisionCanvas;

	[SerializeField]
	private AnomalyCard anomalyCardPrefab;

	private AnomalyCard activeCard;

	private Anomaly activeAnomaly;

	[SerializeField]
	private StatsPanel positiveStatsPanel;
	[SerializeField]
	private StatsPanel negativeStatsPanel;
	[SerializeField]
	private StatsPanel neutralStatsPanel;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		alertInfoText.OnJumpingTextEnd += DisplayAnomaly;
		decisionCanvas.DOFade(0, 0);
		LoadAllAnomalyData();
	}

	private void LoadAllAnomalyData() {
		Debug.Log("LoadAddAnomalyData");
		try {
			var loadedObjects = Resources.LoadAll("MiniSystems/AnomalySystem/Anomalies", typeof(AnomalyData)).Cast<AnomalyData>();
			Debug.Log("LoadAddAnomalyData");
			Debug.Log(loadedObjects.Count());
			foreach(var go in loadedObjects)
			{
					Debug.Log(go.title);
			}
		}
		catch (Exception e)
		{
			Debug.Log("Proper Method failed with the following exception: ");
			Debug.Log(e);
		}
	}

	void DisplayAnomaly()
	{
		Debug.Log("ANOMALY CAN BE DISPLAYED!");
		decisionCanvas.DOFade(1f, 1f).SetDelay(1f);
		activeCard = Instantiate(anomalyCardPrefab, transform.position, Quaternion.identity);
		activeCard.transform.parent = transform;
		activeCard.OnDecisionActions += OnDecisionMake;

		GameManager.I.MainInput.Main.LeftArrow.started += HandleLeftArrowClick;
		GameManager.I.MainInput.Main.RightArrow.performed += HandleRightArrowClick;
		GameManager.I.MainInput.Main.DownArrow.started += HandleDownArrowClick;
	}

	public void HandleLeftArrowClick(InputAction.CallbackContext e)
	{
		activeCard.HandleNegative();
	}

	public void HandleRightArrowClick(InputAction.CallbackContext e)
	{
		activeCard.HandlePositive();
	}

	public void HandleDownArrowClick(InputAction.CallbackContext e)
	{
		activeCard.HandlePositive();
	}

	public void OnDecisionMake(AnomalyReaction decision)
	{
		Debug.Log($"ON DECISIOn MAKED {decision}");
		decisionCanvas.DOFade(0f, 1f);
		GameManager.I.MainInput.Main.LeftArrow.started -= HandleLeftArrowClick;
		GameManager.I.MainInput.Main.RightArrow.performed -= HandleRightArrowClick;
		GameManager.I.MainInput.Main.DownArrow.started -= HandleDownArrowClick;
		activeCard.OnDecisionActions -= OnDecisionMake;
		DOVirtual.DelayedCall(1f, () =>
		{
			activeCard = null;
			alertInfoText.gameObject.SetActive(true);
		}, false);
	}
}
