using DG.Tweening;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
		[SerializeField] ParticleSystem missEffect;
		Tween tween;

		[SerializeField] float arrowDisplacementTime = 2f;
		[SerializeField] AudioClip missAudioClip;
    void Start()
    {
				var initialScale = transform.localScale;
				transform.localScale = Vector3.zero;
				transform.DOScale(initialScale, 0.5f);
        transform.DOMoveY(-6f, arrowDisplacementTime).SetEase(Ease.Linear);
				transform.DOScale(Vector3.zero, 0.5f).SetDelay(arrowDisplacementTime);
				tween = DOVirtual.DelayedCall(arrowDisplacementTime + 0.5f, () => DeleteArrow(true), false);
    }

		void DeleteArrow(bool withEffect) {
			if (withEffect) {
				Instantiate(missEffect, transform.position,  Quaternion.Euler(-90, 0, 0));
				PlayMissClip();
			}
			transform.DOKill();
			Destroy(gameObject);
		}

		private void OnDestroy() {
			tween.Kill();
		}

		private void OnDisable() {
			DeleteArrow(false);
		}

		private void PlayMissClip() {
			if(missAudioClip != null) {
				Sound.PlaySoundAtPos(Vector3.zero, missAudioClip, Sound.MixerTypes.BGMMinigames, 1f, true, false, true);
			}
		}
}
