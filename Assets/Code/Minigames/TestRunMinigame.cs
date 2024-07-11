using System.Collections.Generic;
using UnityEngine;

public class TestRunMinigame : MonoBehaviour {
  [SerializeField] GameObject minigameGameObject;
  // IMinigame minigame;

  [SerializeField] List<GameObject> minigamesGameObjects;
  [SerializeField] List<IMinigame> minigames = new();

  private void Start() {
    // minigame = minigameGameObject.GetComponent<IMinigame>();

    foreach (GameObject minigameGO in minigamesGameObjects) {
      IMinigame minigame = minigameGO.GetComponent<IMinigame>();
      if (minigame != null) {
        minigames.Add(minigame);
        minigame.MinigameFinished += CloseGame;
      }
    }
  }

  private void CloseGame() {
    foreach(IMinigame minigame in minigames) {
      if (minigame.IsDisplayed) {
        minigame.CloseGame();
      }
    }
  }

  private void Update() {
    // if (Input.GetKeyDown(KeyCode.Space)) {
    // 	if (minigame.IsDisplayed)
    // 		minigame.CloseGame();
    // 	else
    // 		minigame.ShowGame();
    // }

    int gameIndex = -1;
    if(Input.GetKeyDown(KeyCode.Keypad0)) {
      gameIndex = 0;
    }
    if(Input.GetKeyDown(KeyCode.Keypad1)) {
      gameIndex = 1;
    }
    if(Input.GetKeyDown(KeyCode.Keypad2)) {
      gameIndex = 2;
    }
    if(Input.GetKeyDown(KeyCode.Keypad3)) {
      gameIndex = 3;
    }
    if(Input.GetKeyDown(KeyCode.Keypad4)) {
      gameIndex = 4;
    }

    if (gameIndex >= 0 && gameIndex < minigames.Count) {
      Debug.Log(minigames[gameIndex]);
      minigames[gameIndex].ShowGame();
    }
  }
}