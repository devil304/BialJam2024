using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnomalyCard : MonoBehaviour
{
	[SerializeField]
	TextMeshProUGUI descriptionLabel;
	[SerializeField]
	Image cardImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(0, -100f, 0);
				transform.DOMove(Vector3.zero, 1f, false);
				transform.DOShakePosition(0.5f, 5, 20, 80, false, true).SetDelay(1f);
				transform.DOShakeRotation(0.5f, 5, 20, 80, true).SetDelay(1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

		public void SetupCard(string description, Sprite imageSprite) {
			descriptionLabel.text = description;
			cardImage.sprite = imageSprite;
		}
}
