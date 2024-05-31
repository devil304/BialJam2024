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
	private int activeWordIndex = -1;

	bool tryFocus = false;

	private float gameTime = 0;
	private float maxGameTime = 0;

	private bool shouldSpawn = false;

	private void Start() {
		inputField.onValueChanged.AddListener(OnInputFieldChanged);
	}

	private void Update() {
		if (gameTime >= maxGameTime || !shouldSpawn) return;

		gameTime += Time.deltaTime;

		if (tryFocus && gameObject.activeInHierarchy && shouldSpawn) inputField.Select();
	}

	public void SetupGame() {
		score = 0;
		scoreLabel.text = "0";
		wordToWriteLabel.text = "";
	}

	public void StartGame(float maxGameTime) {
		this.maxGameTime = maxGameTime;
		gameTime = 0;
		shouldSpawn = true;
		inputField.onDeselect.AddListener(StartTryingFocusInput);
		inputField.onSelect.AddListener(StopTryingFocusInput);
		inputField.ActivateInputField();
		inputField.Select();
		SetNewWord();
	}

	private void StartTryingFocusInput(string a) {
		tryFocus = true;
	}

	private void StopTryingFocusInput(string a) {
		tryFocus = false;
	}

	public void EndGame() {
		wordToWriteLabel.text = "END";
		shouldSpawn = false;
		tryFocus = false;
		inputField.onDeselect.RemoveListener(StartTryingFocusInput);
		inputField.onSelect.RemoveListener(StopTryingFocusInput);
		inputField.ReleaseSelection();
		inputField.DeactivateInputField();
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
		PlayKeyboardClip();

		HandleUserInput(userInput);
	}

	private void PlayKeyboardClip() {
		if (audioClips.Count > 0) {
			AudioClip audioClip = audioClips[Random.Range(0, audioClips.Count)];
			Sound.PlaySoundAtPos(Vector3.zero, audioClip, Sound.MixerTypes.BGMMinigames, 1f, true, false, true);
		}
	}

	private string GetNewWord() {
		int newIndex = Random.Range(0, keywords.Count);
		if (activeWordIndex != newIndex) {
			activeWordIndex = newIndex;
			return keywords[newIndex];
		}
		return GetNewWord();
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