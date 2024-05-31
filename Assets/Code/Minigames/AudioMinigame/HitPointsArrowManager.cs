using System.Collections.Generic;
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
	[SerializeField] List<AudioClip> arrowHitAudioClips = new List<AudioClip>();

	private int score;

	private void OnEnable()
	{
		GameManager.I.MainInput.Main.LeftArrow.started += HandleLeftArrowClick;
		GameManager.I.MainInput.Main.UpArrow.started += HandleUpArrowClick;
		GameManager.I.MainInput.Main.RightArrow.started += HandleRightArrowClick;
		GameManager.I.MainInput.Main.DownArrow.started += HandleDownArrowClick;
		score = 0;
		scoreLabel.text = "0";
	}

	private void OnDisable()
	{
		GameManager.I.MainInput.Main.LeftArrow.started -= HandleLeftArrowClick;
		GameManager.I.MainInput.Main.UpArrow.started -= HandleUpArrowClick;
		GameManager.I.MainInput.Main.RightArrow.started -= HandleRightArrowClick;
		GameManager.I.MainInput.Main.DownArrow.started -= HandleDownArrowClick;
	}

	void HandleLeftArrowClick(InputAction.CallbackContext e)
	{
		Collider2D hittedElement = Physics2D.OverlapCircle(LeftArrowHitpointPosition.position, hitboxRadius);
		OnArrowHit(hittedElement, LeftArrowHitpointPosition);
	}
	void HandleUpArrowClick(InputAction.CallbackContext e)
	{
		Collider2D hittedElement = Physics2D.OverlapCircle(UpArrowHitpointPosition.position, hitboxRadius);
		OnArrowHit(hittedElement, UpArrowHitpointPosition);
	}
	void HandleRightArrowClick(InputAction.CallbackContext e)
	{
		Collider2D hittedElement = Physics2D.OverlapCircle(RightArrowHitpointPosition.position, hitboxRadius);
		OnArrowHit(hittedElement, RightArrowHitpointPosition);
	}
	void HandleDownArrowClick(InputAction.CallbackContext e)
	{
		Collider2D hittedElement = Physics2D.OverlapCircle(DownArrowHitpointPosition.position, hitboxRadius);
		OnArrowHit(hittedElement, DownArrowHitpointPosition);
	}

	void OnArrowHit(Collider2D hittedArrowCollider, Transform hitPointArrowPosition)
	{
		if (hittedArrowCollider == null) return;

		float distanseFromCenter = Vector2.Distance(hitPointArrowPosition.position, hittedArrowCollider.transform.position);
		Debug.Log($"Points to add: {GetPointsFromDistance(distanseFromCenter)}");

		Instantiate(arrowHitEffect, hitPointArrowPosition.position, Quaternion.identity);

		score += GetPointsFromDistance(distanseFromCenter);
		scoreLabel.text = $"{score}";

		hittedArrowCollider.transform.DOKill();
		Destroy(hittedArrowCollider.gameObject);
		PlayHitArrowClip();
	}

	private void PlayHitArrowClip()
	{
		if (arrowHitAudioClips.Count > 0)
		{
			AudioClip audioClip = arrowHitAudioClips[Random.Range(0, arrowHitAudioClips.Count)];
			Sound.PlaySoundAtPos(Vector3.zero, audioClip, Sound.MixerTypes.BGMMinigames, 1f, true, false, true);
		}
	}

	int GetPointsFromDistance(float distance)
	{
		if (distance < 0.15) return 5;
		if (distance < 0.35) return 4;
		if (distance < 0.6) return 3;
		if (distance < 0.8) return 2;
		return 1;
	}

	public int GetScore() => score;

}
