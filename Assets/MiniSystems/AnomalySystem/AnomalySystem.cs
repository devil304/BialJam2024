using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnomalySystem : MonoBehaviour
{
	public event Action OnAnomalyStart;
	public event Action OnAnomalyEnd;
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
	private List<AnomalyData> allAnomaliesData;
	private List<AnomalyData> drawdedAnomalies = new List<AnomalyData>();

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		alertInfoText.OnJumpingTextEnd += DisplayAnomaly;
		decisionCanvas.DOFade(0, 0);
		LoadAllAnomalyData();
	}

	private void LoadAllAnomalyData() {
		try {
			drawdedAnomalies = new List<AnomalyData>();
			allAnomaliesData = Resources.LoadAll("Anomalies/", typeof(AnomalyData)).Cast<AnomalyData>().ToList();
		}
		catch (Exception e)
		{
			Debug.Log("Proper Method failed with the following exception: ");
			Debug.Log(e);
		}
	}

	public void StartAnomaly() {
		alertInfoText.gameObject.SetActive(true);
		OnAnomalyStart?.Invoke();
	}

	public AnomalyData DrawAnomaly() {
		if(allAnomaliesData.Count <= 0) LoadAllAnomalyData();
		AnomalyData drawdedAnomalyData = allAnomaliesData[UnityEngine.Random.Range(0, allAnomaliesData.Count)];

		allAnomaliesData.Remove(drawdedAnomalyData);
		drawdedAnomalies.Add(drawdedAnomalyData);
		return drawdedAnomalyData;
	}

	public void CreateAnomaly() {
		activeAnomaly = new Anomaly(DrawAnomaly());
		positiveStatsPanel.SetupStats(activeAnomaly.GetStatsModel(AnomalyReaction.YES));
		negativeStatsPanel.SetupStats(activeAnomaly.GetStatsModel(AnomalyReaction.NO));
		neutralStatsPanel.SetupStats(activeAnomaly.GetStatsModel(AnomalyReaction.IGNORE));
	}

	void DisplayAnomaly()
	{
		CreateAnomaly();
		decisionCanvas.DOFade(1f, 1f).SetDelay(1f);
		activeCard = Instantiate(anomalyCardPrefab, transform.position, Quaternion.identity);
		activeCard.SetupCard(activeAnomaly.GetAnomalyData());
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
		decisionCanvas.DOFade(0f, 1f);
		GameManager.I.MainInput.Main.LeftArrow.started -= HandleLeftArrowClick;
		GameManager.I.MainInput.Main.RightArrow.performed -= HandleRightArrowClick;
		GameManager.I.MainInput.Main.DownArrow.started -= HandleDownArrowClick;
		activeCard.OnDecisionActions -= OnDecisionMake;
		DOVirtual.DelayedCall(1f, () =>
		{
			GameManager.I.ModifyStats(activeAnomaly.GetStatsModel(decision));
			activeCard = null;
			OnAnomalyEnd?.Invoke();
		}, false);
	}
}
