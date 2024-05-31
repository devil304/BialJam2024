using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordSpawner : MonoBehaviour {
	[SerializeField]
	private List<string> keywords = new List<string>();


	[SerializeField] TextMeshProUGUI wordToWriteLabel;
	[SerializeField] TextMeshProUGUI scoreLabel;
	[SerializeField] TMP_InputField inputField;

	[SerializeField] List<AudioClip> audioClips;

	private int score = 0;

	private string wordToWrite;
	int currentCharIndex;

	bool tryFocus = false;

	private float gameTime = 0;
	private float maxGameTime = 0;

	private bool shouldSpawn = false;

	private void Start() {
		inputField.onValueChanged.AddListener(OnInputFieldChanged);
		inputField.onDeselect.AddListener((string a) => {
			tryFocus = true;
		});
		inputField.onSelect.AddListener((string a) => {
			tryFocus = false;
		});
	}

	private void Update() {
		if (gameTime >= maxGameTime || !shouldSpawn) return;

		gameTime += Time.deltaTime;

		if (tryFocus && gameObject.activeInHierarchy && shouldSpawn) inputField.Select();
	}

	public void StartGame(float maxGameTime) {
		this.maxGameTime = maxGameTime;
		gameTime = 0;
		score = 0;
		wordToWriteLabel.text = "";
		shouldSpawn = true;
		inputField.Select();
		SetNewWord();
	}

	public void EndGame() {
		shouldSpawn = false;
		tryFocus = false;
	}

	private void SetNewWord() {
		wordToWrite = GetNewWord();
		wordToWriteLabel.text = wordToWrite.Replace(" ", "_");
		currentCharIndex = 0;
		scoreLabel.text = score.ToString();
	}

	private void OnInputFieldChanged(string newValue) {
		if (newValue == "") return;

		char userInput = newValue[0];
		inputField.text = "";
		AudioClip audioClip = audioClips[Random.Range(0, audioClips.Count)];
		Sound.PlaySoundAtPos(Vector3.zero, audioClip, Sound.MixerTypes.BGMMinigames, 1f, true, false, true);

		HandleUserInput(userInput);
	}

	private string GetNewWord() {
		return keywords[Random.Range(0, keywords.Count)];
	}

	void HandleUserInput(char userInput) {
		if (CheckChar(wordToWrite[currentCharIndex], userInput)) {
			currentCharIndex++;
			wordToWriteLabel.text = GetCorrectLetter(currentCharIndex);

			if (currentCharIndex == wordToWrite.Length) {
				score += currentCharIndex;
				SetNewWord();
			}
		} else {
			Debug.Log("INCORRECT");
		}
	}

	bool CheckChar(char letterFromWord, char userInput) {
		if (letterFromWord == ' ') return userInput == ' ' || userInput == '_';
		return userInput == letterFromWord;
	}

	string GetCorrectLetter(int currentPosition) {
		string replacedWord = wordToWrite.Replace(" ", "_");
		return $"<color=\"green\">{replacedWord.Substring(0, currentPosition)}</color>{replacedWord.Substring(currentPosition, replacedWord.Length - currentPosition)}";
	}
}