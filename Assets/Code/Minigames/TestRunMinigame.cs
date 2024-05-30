using UnityEngine;

public class TestRunMinigame : MonoBehaviour {
	[SerializeField] GameObject minigameGameObject;
	IMinigame minigame;

	private void Start() {
		minigame = minigameGameObject.GetComponent<IMinigame>();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (minigame.IsDisplayed())
				minigame.CloseGame();
			else
				minigame.ShowGame();
		}
	}
}