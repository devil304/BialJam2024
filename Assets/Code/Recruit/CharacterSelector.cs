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
	private int focusedCharacterIndex = 0;

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
    focusedCharacterIndex = 0;
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

	public void UpdatePoolPosition(bool scaleLeft) {
    List<int> visibleIndexes = new();
		UpdateSubscribeButton();
    int hidingCardIndex = -1;
		if(scaleLeft) {
			if (farLeftCard != null) {
        hidingCardIndex = charactersCards.IndexOf(farLeftCard);
        farLeftCard.transform.DOScale(0f, 0.5f);
      }
		} else {
			if (farRightCard != null) {
        hidingCardIndex = charactersCards.IndexOf(farRightCard);
        farRightCard.transform.DOScale(0f, 0.5f);
      }
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

    int rightIndex = (focusedCharacterIndex + 1)%characterPoolCount;
    int farRightIndex = (focusedCharacterIndex + 2)%characterPoolCount;
    visibleIndexes.Add(hidingCardIndex);
    visibleIndexes.Add(farLeftIndex);
    visibleIndexes.Add(leftIndex);
    visibleIndexes.Add(focusedCharacterIndex);
    visibleIndexes.Add(rightIndex);
    visibleIndexes.Add(farRightIndex);

		farLeftCard = charactersCards[farLeftIndex];
		leftCard = charactersCards[leftIndex];
		centerCard = charactersCards[focusedCharacterIndex];
		rightCard = charactersCards[rightIndex];
		farRightCard = charactersCards[farRightIndex];

    MoveHidenCards(visibleIndexes, scaleLeft);
		UpdateCardPosition(farLeftCard, 1);
		UpdateCardPosition(leftCard, 2);
		UpdateCardPosition(centerCard, 3);
		UpdateCardPosition(rightCard, 4);
		UpdateCardPosition(farRightCard, 5);
	}

  private void MoveHidenCards(List<int> visibleIndexes, bool moveToRight) {
		Vector3 moveVector = Vector3.zero;
		if(moveToRight) {
				moveVector.x = 6.3f;
		} else {
				moveVector.x = -6.3f;
		}

    for(var i = 0; i < charactersCards.Count; i++) {
      if(visibleIndexes.Contains(i)) continue;

      if(visibleIndexes[0] == -1 && i <= charactersCards.Count / 2) { //For first display set elements on left and right to prevent long card jump
        charactersCards[i].transform.localPosition = moveVector * -1;
      } else {
        charactersCards[i].transform.localPosition = moveVector;
      }

      charactersCards[i].transform.localScale = Vector3.zero;
    }
    
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
