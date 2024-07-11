using System.Collections.Generic;
using UnityEngine;

public class CharacterCard : MonoBehaviour
{
	[SerializeField] BounceHead characterSprite;
	[SerializeField] StatsModel statsModel;

	[SerializeField] List<StarLabelScript> scriptList = new();

	[SerializeField] GameObject selectedIndicator;

	

	public void UpdateCharacterSprite(CharacterModel character) {
		characterSprite.UpdateSprites(character.head, character.hair, character.accessory, character.NickName);

		for(int i = 0; i < scriptList.Count; i++)
		{
				scriptList[i].InitStats(character.CharStats.GetStat((StatsTypes)i),i);
		}
	}

	public void ToggleSelected(bool state) {
		selectedIndicator.SetActive(state);
	}
}
