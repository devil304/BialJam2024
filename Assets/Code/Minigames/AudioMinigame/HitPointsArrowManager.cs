using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitPointsArrowManager : MonoBehaviour
{
		[Header("References")]
		[SerializeField] Transform LeftArrowHitpointPosition;
		[SerializeField] Transform UpArrowHitpointPosition;
		[SerializeField] Transform RightArrowHitpointPosition;
		[SerializeField] Transform DownArrowHitpointPosition;
		[SerializeField] TextMeshProUGUI scoreLabel;
		[SerializeField] ParticleSystem arrowHitEffect;

		[Header("Configurations")]
		[SerializeField] float hitboxRadius = 0.75f;

		private int score;

		private void OnEnable() {
        GameManager.Instance.MainInput.Main.LeftArrow.started += HandleLeftArrowClick;
        GameManager.Instance.MainInput.Main.UpArrow.started += HandleUpArrowClick;
        GameManager.Instance.MainInput.Main.RightArrow.started += HandleRightArrowClick;
        GameManager.Instance.MainInput.Main.DownArrow.started += HandleDownArrowClick;
				score = 0;
				scoreLabel.text = "0";
		}

		private void OnDisable() {
        GameManager.Instance.MainInput.Main.LeftArrow.started -= HandleLeftArrowClick;
        GameManager.Instance.MainInput.Main.UpArrow.started -= HandleUpArrowClick;
        GameManager.Instance.MainInput.Main.RightArrow.started -= HandleRightArrowClick;
        GameManager.Instance.MainInput.Main.DownArrow.started -= HandleDownArrowClick;
		}

		void HandleLeftArrowClick (InputAction.CallbackContext e) {
				Collider2D hittedElement = Physics2D.OverlapCircle(LeftArrowHitpointPosition.position, hitboxRadius);
				OnArrowHit(hittedElement, LeftArrowHitpointPosition);
		}
		void HandleUpArrowClick (InputAction.CallbackContext e) {
				Collider2D hittedElement = Physics2D.OverlapCircle(UpArrowHitpointPosition.position, hitboxRadius);
				OnArrowHit(hittedElement, UpArrowHitpointPosition);
		}
		void HandleRightArrowClick (InputAction.CallbackContext e) {
				Collider2D hittedElement = Physics2D.OverlapCircle(RightArrowHitpointPosition.position, hitboxRadius);
				OnArrowHit(hittedElement, RightArrowHitpointPosition);
		}
		void HandleDownArrowClick (InputAction.CallbackContext e) {
				Collider2D hittedElement = Physics2D.OverlapCircle(DownArrowHitpointPosition.position, hitboxRadius);
				OnArrowHit(hittedElement, DownArrowHitpointPosition);
		}

		void OnArrowHit(Collider2D hittedArrowCollider, Transform hitPointArrowPosition) {
			if(hittedArrowCollider == null) return;

			float distanseFromCenter = Vector2.Distance(hitPointArrowPosition.position, hittedArrowCollider.transform.position);
			Debug.Log($"Points to add: { GetPointsFromDistance(distanseFromCenter) }");

			Instantiate(arrowHitEffect, hitPointArrowPosition.position, Quaternion.identity);

			score += GetPointsFromDistance(distanseFromCenter);
			scoreLabel.text = $"{score}";

			hittedArrowCollider.transform.DOKill();
			Destroy(hittedArrowCollider.gameObject);
		}

		int GetPointsFromDistance(float distance) {
			if(distance < 0.15) return 5;
			if(distance < 0.35) return 4;
			if(distance < 0.6) return 3;
			if(distance < 0.8) return 2;
			return 1;
		}

}
