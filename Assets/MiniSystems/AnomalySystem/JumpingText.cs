using System;
using DG.Tweening;
using UnityEngine;

public class JumpingText : MonoBehaviour {
	public event Action OnJumpingTextEnd;
	private void OnEnable() {
		transform.localScale = new Vector3(1f, 1f, 1f);
		transform.DOShakeScale(1f, 4f, 7, 50, true);
		transform.DOScale(Vector2.zero, 0.5f).SetDelay(1.5f);
		DOVirtual.DelayedCall(2f, HideText, false);
	}

	private void HideText() {
		OnJumpingTextEnd?.Invoke();
		gameObject.SetActive(false);
	}
}