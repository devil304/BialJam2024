using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
	[SerializeField] private GameObject characterCardPrefab;
	private List<CharacterModel> characters;

	[SerializeField] List<Sprite> body;
	[SerializeField] List<Sprite> head;
	[SerializeField] List<Sprite> hair;
	[SerializeField] List<Sprite> accessory;
	[SerializeField] TMP_Text currentTeamSizeLabel;
	[SerializeField] TMP_Text subscribeButtonText;
	[SerializeField] int characterPoolCount = 10;
	[SerializeField] Button startGameButton;


	private List<CharacterModel> selectedCharacters = new();
	private int focusedCharacterIndex;

	private List<GameObject> charactersCards = new();


	private GameObject farLeftCard;
	private GameObject leftCard;
	private GameObject centerCard;
	private GameObject rightCard;
	private GameObject farRightCard;
	private void Awake() {
		CreateNewCardPool();
		UpdatePoolPosition(false);
		startGameButton.interactable = false;
	}

	void CreateNewCardPool()
	{
		characters = new List<CharacterModel>();
		for (int i = 0; i < characterPoolCount; i++)
		{
				CharacterModel character = new CharacterModel();
				character.GenerateRandom();
				InitCharacterSprites(character);
				characters.Add(character);

				GameObject characterCard = Instantiate(characterCardPrefab, transform);
				Debug.Log(characterCard);
				characterCard.transform.localScale = Vector3.zero;
				characterCard.GetComponent<CharacterCard>().UpdateCharacterSprite(character);
				charactersCards.Add(characterCard);
		}
	}

	public void HandleSubscribeButton() {
		var selectedCharacter = characters[focusedCharacterIndex];
		CharacterCard characterCard = charactersCards[focusedCharacterIndex].GetComponent<CharacterCard>();

		if (selectedCharacters.Contains(selectedCharacter)) {
			selectedCharacters.Remove(selectedCharacter);
			characterCard.ToggleSelected(false);
		} else if (selectedCharacters.Count < 5) {
			selectedCharacters.Add(selectedCharacter);
			characterCard.ToggleSelected(true);
			MoveRight();
		}

		currentTeamSizeLabel.text = $"Current Team Size: {selectedCharacters.Count}/5";

		startGameButton.interactable = selectedCharacters.Count == 5;

		UpdateSubscribeButton();
	}

	public void UpdateSubscribeButton() {
		var focusedCharacter = characters[focusedCharacterIndex];
		if(selectedCharacters.Contains(focusedCharacter)) {
			subscribeButtonText.text = "Unsubscribe";
		} else {
			subscribeButtonText.text = "Recruit";
		}
	}

	public void MoveRight() {
		focusedCharacterIndex = (focusedCharacterIndex + 1) % characterPoolCount;
		UpdatePoolPosition(true);
	}

	public void MoveLeft() {
		focusedCharacterIndex--;
		if (focusedCharacterIndex < 0) focusedCharacterIndex = characterPoolCount - 1;
		UpdatePoolPosition(false);
	}

	public void UpdatePoolPosition(bool scaleRight) {
		UpdateSubscribeButton();
		if(scaleRight) {
			if (farLeftCard != null) farLeftCard.transform.DOScale(0f, 0.5f);
		} else {
			if (farRightCard != null) farRightCard.transform.DOScale(0f, 0.5f);
		}
		int farLeftIndex;
		int leftIndex;
		if (focusedCharacterIndex == 1) {
			farLeftIndex = characterPoolCount - 1;
			leftIndex = focusedCharacterIndex - 1;
		} else if (focusedCharacterIndex == 0) {
			farLeftIndex = characterPoolCount - 2;
			leftIndex = characterPoolCount - 1;
		} else {
			farLeftIndex = focusedCharacterIndex - 2;
			leftIndex = focusedCharacterIndex - 1;
		}
		farLeftCard = charactersCards[farLeftIndex];
		leftCard = charactersCards[leftIndex];
		centerCard = charactersCards[focusedCharacterIndex];
		rightCard = charactersCards[(focusedCharacterIndex + 1)%characterPoolCount];
		farRightCard = charactersCards[(focusedCharacterIndex + 2)%characterPoolCount];

		UpdateCardPosition(farLeftCard, 1);
		UpdateCardPosition(leftCard, 2);
		UpdateCardPosition(centerCard, 3);
		UpdateCardPosition(rightCard, 4);
		UpdateCardPosition(farRightCard, 5);
	}

	public void UpdateCardPosition(GameObject cardObj, int cardIndex) {
		Vector3 moveVector = Vector3.zero;
		float scaleValue = 0.7f;
		switch(cardIndex) {
			case 1:
				moveVector.x = -6.3f;
				scaleValue = 0.2f;
			break;
			case 2:
				moveVector.x = -4;
				scaleValue = 0.4f;
			break;
			case 3:
				moveVector = Vector3.zero;
				scaleValue = 0.7f;
			break;
			case 4:
				moveVector.x = 4;
				scaleValue = 0.4f;
			break;
			case 5:
				moveVector.x = 6.3f;
				scaleValue = 0.2f;
			break;
		}
		cardObj.transform.DOMove(moveVector, 0.5f);
		cardObj.transform.DOScale(scaleValue, 0.5f);
	}

	void InitCharacterSprites(CharacterModel character) {
		character.body = body[StrongRandom.RNG.Next(body.Count)];
		character.hair = hair[StrongRandom.RNG.Next(hair.Count)];
		character.head = head[StrongRandom.RNG.Next(head.Count)];

		switch (character.MainStat) {
			case 0: {
					character.accessory = accessory[0];
					break;
				}
			case 1: {
					break;
				}
			case 2: {
					character.accessory = accessory[2];
					break;
				}
			case 3: {
					character.accessory = accessory[3];
					break;
				}
			case 4: {
					character.accessory = accessory[4];
					character.head = head[1];
					break;
				}
		}
	}

	public void StartGame() {
		foreach (var character in selectedCharacters) {
			GameManager.I.AddTeamMember(character);
		}

		GameManager.I.FinishSelectingTeam();
	}
}
