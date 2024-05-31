using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AnomalyCard : MonoBehaviour
{
	public event Action<AnomalyReaction> OnDecisionActions;
	[SerializeField]
	TextMeshProUGUI descriptionLabel;
	[SerializeField]
	TextMeshProUGUI titleLabel;
	[SerializeField]
	Image cardImage;

	bool followMouse = false;
	bool canFollowMouse = false;
	Vector2 mouseStartPosition;
	AnomalyReaction? anomalySelectedReaction;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
			transform.position = new Vector3(0, -100f, 0);
			transform.DOMove(new Vector3(0, 0.7f, 0), 1f, false);
			// transform.DOShakePosition(0.5f, 5, 20, 80, false, true).SetDelay(1f);
			transform.DOShakeRotation(0.25f, 5, 20, 80, true).SetDelay(1f);
			DOVirtual.DelayedCall(1f, EnableInputs, false);
	}

	private void EnableInputs() {
		GameManager.I.MainInput.Main.MousePos.performed += OnMouseMove;
	}

	private void OnMouseMove(InputAction.CallbackContext c) {
		if (!followMouse) return;
		Vector2 mouseActivePosition = c.action.ReadValue<Vector2>();
		Vector2 mouseMovement = mouseStartPosition - mouseActivePosition;
		float mouseDistance = Vector2.Distance(mouseStartPosition, mouseActivePosition);
		if(Mathf.Abs(mouseMovement.normalized.x) > 0.7f && mouseDistance > 150) {
			transform.DORotateQuaternion(Quaternion.Euler(0, 0, 45 * mouseMovement.normalized.x), 1f);
			transform.DOMove(Vector3.zero, 1f);
			anomalySelectedReaction = mouseMovement.normalized.x < 0 ? AnomalyReaction.YES : AnomalyReaction.NO;
		} else if (mouseMovement.normalized.y > 0.7f && mouseDistance > 150) {
			anomalySelectedReaction = AnomalyReaction.IGNORE;
			transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 1f);
			transform.DOMoveY(-5, 1f);
		} else {
			anomalySelectedReaction = null;
			transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 1f);
			transform.DOMove(Vector3.zero, 1f);
		}
	}
	private void OnMouseEnter() {
		canFollowMouse = true;
	}

	private void OnMouseExit() {
		canFollowMouse = false;
	}

	public void OnMouseDown() {
		if (!canFollowMouse) return;
		mouseStartPosition = GameManager.I.MainInput.Main.MousePos.ReadValue<Vector2>();
		followMouse = true;
	}

	public void OnMouseUp() {
		followMouse = false;
		if(anomalySelectedReaction != null) {
			Debug.Log($"Decision has been maked {anomalySelectedReaction}");
			switch(anomalySelectedReaction) {
				case AnomalyReaction.YES:
					HandlePositive();
				break;
				case AnomalyReaction.NO:
					HandleNegative();
				break;
				case AnomalyReaction.IGNORE:
					HandleNeutral();
				break;
			}
		}
	}

	private void OnDestroy() {
		GameManager.I.MainInput.Main.MousePos.performed -= OnMouseMove;
	}

	public void SetupCard(string description, Sprite imageSprite) {
		descriptionLabel.text = description;
		cardImage.sprite = imageSprite;
	}

	public void HandleNegative()
	{

		transform.DOMoveX(-20f, 1f);
		transform.DORotate(new Vector3(2, 0, 0), 1f);
		OnDecisionMake(AnomalyReaction.NO);
	}

	public void HandlePositive()
	{
		transform.DOMoveX(20f, 1f);
		transform.DORotate(new Vector3(-2, 0, 0), 1f);
		OnDecisionMake(AnomalyReaction.YES);
	}

	public void HandleNeutral()
	{
		transform.DOMoveY(-20f, 1f);
		OnDecisionMake(AnomalyReaction.IGNORE);
	}

	public void OnDecisionMake(AnomalyReaction decision)
	{
		OnDecisionActions?.Invoke(decision);
		DOVirtual.DelayedCall(1f, DestroyCard, false);
	}

	public void DestroyCard() {
		Destroy(gameObject);
	}

}
