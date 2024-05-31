using System.Collections.Generic;
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
	private GameObject anomalyCardPrefab;

	private GameObject activeCard;

	private List<Anomaly> activeAnomalies;

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
	}

	// Update is called once per frame
	void Update()
	{

	}

	void DisplayAnomaly()
	{
		Debug.Log("ANOMALY CAN BE DISPLAYED!");
		decisionCanvas.DOFade(1f, 1f).SetDelay(1f);
		activeCard = Instantiate(anomalyCardPrefab, transform.position, Quaternion.identity);
		activeCard.transform.parent = transform;

		GameManager.I.MainInput.Main.LeftArrow.started += HandleLeftArrowClick;
		GameManager.I.MainInput.Main.RightArrow.performed += HandleRightArrowClick;
		GameManager.I.MainInput.Main.DownArrow.started += HandleDownArrowClick;
	}

	public void HandleLeftArrowClick(InputAction.CallbackContext e)
	{

		activeCard.transform.DOMoveX(-20f, 1f);
		activeCard.transform.DORotate(new Vector3(2, 0, 0), 1f);
		OnDecisionMake();
	}

	public void HandleRightArrowClick(InputAction.CallbackContext e)
	{
		activeCard.transform.DOMoveX(20f, 1f);
		activeCard.transform.DORotate(new Vector3(-2, 0, 0), 1f);
		OnDecisionMake();
	}

	public void HandleDownArrowClick(InputAction.CallbackContext e)
	{
		activeCard.transform.DOMoveY(-20f, 1f);
		OnDecisionMake();
	}

	public void OnDecisionMake()
	{
		decisionCanvas.DOFade(0f, 1f);
		GameManager.I.MainInput.Main.LeftArrow.started -= HandleLeftArrowClick;
		GameManager.I.MainInput.Main.RightArrow.performed -= HandleRightArrowClick;
		GameManager.I.MainInput.Main.DownArrow.started -= HandleDownArrowClick;
		DOVirtual.DelayedCall(1f, () =>
		{
			Destroy(activeCard);
			activeCard = null;
			alertInfoText.gameObject.SetActive(true);
		}, false);
	}
}
