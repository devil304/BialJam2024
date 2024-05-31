using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    [SerializeField] public CharacterModel character;
    [SerializeField] public TMP_Text text;
    [SerializeField] BounceHead characterModel;
    [SerializeField] List<StarLabelScript> scriptList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //text.text = character.NickName;
        //Debug.Log(character.NickName);

        characterModel.UpdateSprites(character.head, character.hair, character.accessory, character.NickName);

        InitStartLabels();

    }

    public void InitStartLabels()
    {
        for(int i = 0; i < scriptList.Count; i++)
        {
            scriptList[i].InitStats(character.CharStats.GetStat((StatsTypes)i),i);
        }
    }

}
