using DG.Tweening;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
		[SerializeField] ParticleSystem missEffect;
		Tween tween;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
				var initialScale = transform.localScale;
				transform.localScale = Vector3.zero;
				transform.DOScale(initialScale, 0.5f);
        transform.DOMoveY(-6f, 2f).SetEase(Ease.Linear);
				tween = DOVirtual.DelayedCall(2.5f, DeleteArrow, false);
    }

		void DeleteArrow() {
			Instantiate(missEffect, transform.position,  Quaternion.Euler(-90, 0, 0));
			transform.DOKill();
			Destroy(gameObject);
		}

		private void OnDestroy() {
			tween.Kill();
		}
}
