using DG.Tweening;
using TMPro;
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
			// GameManager.I.MainInput.Main.LMB.started += OnMouseDown;
			// GameManager.I.MainInput.Main.LMB.canceled += OnMouseUp;
			GameManager.I.MainInput.Main.MousePos.performed += OnMouseMove;
		}

    // Update is called once per frame
    void Update()
    {
        
    }

		private void OnMouseMove(InputAction.CallbackContext c) {
			if (!followMouse) return;
			Vector2 mouseActivePosition = c.action.ReadValue<Vector2>();
			Vector2 mouseMovement = mouseStartPosition - mouseActivePosition;
			float mouseDistance = Vector2.Distance(mouseStartPosition, mouseActivePosition);
			if(Mathf.Abs(mouseMovement.normalized.x) > 0.7f && mouseDistance > 150) {
				transform.DORotateQuaternion(Quaternion.Euler(0, 0, 45 * mouseMovement.normalized.x), 1f);
				transform.DOMove(Vector3.zero, 1f);
			} else if (mouseMovement.normalized.y > 0.7f && mouseDistance > 150) {
				transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 1f);
				transform.DOMoveY(-5, 1f);
			} else {
				transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 1f);
				transform.DOMove(Vector3.zero, 1f);
			}
			Debug.Log($"ON MOUSE MOVE {mouseMovement.normalized}");
			Debug.Log($"ON MOUSE Distance {mouseDistance}");
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
		}

		private void OnDestroy() {
			// GameManager.I.MainInput.Main.LMB.started -= OnMouseDown;
			// GameManager.I.MainInput.Main.LMB.canceled -= OnMouseUp;
			GameManager.I.MainInput.Main.MousePos.performed -= OnMouseMove;
		}

		public void SetupCard(string description, Sprite imageSprite) {
			descriptionLabel.text = description;
			cardImage.sprite = imageSprite;
		}
}
