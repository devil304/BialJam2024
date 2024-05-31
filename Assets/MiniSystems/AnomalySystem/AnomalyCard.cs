using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AnomalyCard : MonoBehaviour
{
	[SerializeField]
	TextMeshProUGUI descriptionLabel;
	[SerializeField]
	Image cardImage;

	bool followMouse = false;
	bool canFollowMouse = false;
	Vector2 mouseStartPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(0, -100f, 0);
				transform.DOMove(Vector3.zero, 1f, false);
				transform.DOShakePosition(0.5f, 5, 20, 80, false, true).SetDelay(1f);
				transform.DOShakeRotation(0.5f, 5, 20, 80, true).SetDelay(1f);
				DOVirtual.DelayedCall(1f, EnableInputs, false);
    }

		private void EnableInputs() {
			GameManager.I.MainInput.Main.LMB.started += OnMouseDown;
			GameManager.I.MainInput.Main.LMB.canceled += OnMouseUp;
			GameManager.I.MainInput.Main.MousePos.performed += OnMouseMove;
		}

    // Update is called once per frame
    void Update()
    {
        
    }

		private void OnMouseMove(InputAction.CallbackContext c) {
			if (!followMouse) return;
			Vector2 mouseMovement = mouseStartPosition - c.action.ReadValue<Vector2>();
			Debug.Log($"ON MOUSE MOVE {mouseMovement.normalized}");
		}
		private void OnMouseEnter() {
			canFollowMouse = true;
		}

		private void OnMouseExit() {
			canFollowMouse = false;
		}

		public void OnMouseDown(InputAction.CallbackContext c) {
			if (!canFollowMouse) return;
			mouseStartPosition = GameManager.I.MainInput.Main.MousePos.ReadValue<Vector2>();
			followMouse = true;
		}

		public void OnMouseUp(InputAction.CallbackContext c) {
			followMouse = false;
		}

		private void OnDestroy() {
			GameManager.I.MainInput.Main.LMB.started -= OnMouseDown;
			GameManager.I.MainInput.Main.LMB.canceled -= OnMouseUp;
			GameManager.I.MainInput.Main.MousePos.performed -= OnMouseMove;
		}

		public void SetupCard(string description, Sprite imageSprite) {
			descriptionLabel.text = description;
			cardImage.sprite = imageSprite;
		}
}
