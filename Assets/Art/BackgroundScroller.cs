using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroller : MonoBehaviour
{
	[SerializeField] private RawImage rawImage;
	[SerializeField] private Vector2 scrollDirection = new(1, 1);
	[SerializeField] private float scrollSpeed = 1;

	// Update is called once per frame
	void Update()
	{
		float positionX = rawImage.uvRect.position.x + scrollDirection.x * Time.fixedDeltaTime * scrollSpeed;
		float positionY = rawImage.uvRect.position.y + scrollDirection.y * Time.fixedDeltaTime * scrollSpeed;
		if (positionX >= 10 || positionX <= -10) positionX = 0;
		if (positionY >= 10 || positionY <= -10) positionY = 0;
		Vector2 newPosition = new Vector2(positionX, positionY);
		rawImage.uvRect = new Rect(newPosition, rawImage.uvRect.size);
	}
}
