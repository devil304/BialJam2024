using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitPointsArrowManager : MonoBehaviour
{
		[SerializeField] Transform LeftArrowHitpointPosition;
		[SerializeField] Transform UpArrowHitpointPosition;
		[SerializeField] Transform RightArrowHitpointPosition;
		[SerializeField] Transform DownArrowHitpointPosition;
		[SerializeField] TextMeshProUGUI scoreLabel;
		[SerializeField] ParticleSystem arrowHitEffect;

		private int score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.MainInput.Main.LeftArrow.started += HandleLeftArrowClick;
        GameManager.Instance.MainInput.Main.UpArrow.performed += HandleUpArrowClick;
        GameManager.Instance.MainInput.Main.RightArrow.performed += HandleRightArrowClick;
        GameManager.Instance.MainInput.Main.DownArrow.started += HandleDownArrowClick;
				score = 0;
				scoreLabel.text = "0";
				// Time.timeScale = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {

    }

		void HandleLeftArrowClick (InputAction.CallbackContext e) {
				Collider2D hittedElement = Physics2D.OverlapCircle(LeftArrowHitpointPosition.position, 0.75f);
				OnArrowHit(hittedElement, LeftArrowHitpointPosition);
				// Debug.Log($"Left Arrow Clicked! {hittedElement?.name}");
		}
		void HandleUpArrowClick (InputAction.CallbackContext e) {
				Collider2D hittedElement = Physics2D.OverlapCircle(UpArrowHitpointPosition.position, 0.75f);
				OnArrowHit(hittedElement, UpArrowHitpointPosition);
				// Debug.Log("Up Arrow Clicked!");
		}
		void HandleRightArrowClick (InputAction.CallbackContext e) {
				Collider2D hittedElement = Physics2D.OverlapCircle(RightArrowHitpointPosition.position, 0.75f);
				OnArrowHit(hittedElement, RightArrowHitpointPosition);
				// Debug.Log("Right Arrow Clicked!");
		}
		void HandleDownArrowClick (InputAction.CallbackContext e) {
				Collider2D hittedElement = Physics2D.OverlapCircle(DownArrowHitpointPosition.position, 0.75f);
				OnArrowHit(hittedElement, DownArrowHitpointPosition);
				// Debug.Log("Down Arrow Clicked!");
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
