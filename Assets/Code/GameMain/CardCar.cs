using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CardCar : MonoBehaviour
{
    [SerializeField] GameObject[] _cards;
    [SerializeField] AnimationCurve cardDistance;
    private List<CharacterModel> characters;
    int offset;

    [SerializeField] List<Sprite> body;
    [SerializeField] List<Sprite> head;
    [SerializeField] List<Sprite> hair;
    [SerializeField] List<Sprite> accessory;

    public CardScript currentSelectedCharacter;

    [SerializeField] Button StartButton;
    [SerializeField] Button RecruitButton;  
    [SerializeField] TMP_Text text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void OnEnable()
    {
        offset = 0;
        CreateNewCardPull();
        MoveRight();
        HideStartButton();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.I.Team.Count == 5)
        {
            ShowStartButton();
        }
        else
        {
            text.text = $"Current Team Size: {GameManager.I.Team.Count}/5";
        }
        
    }

    void ShowStartButton()
    {
        CanvasGroup cg;
        cg = StartButton.GetComponent<CanvasGroup>();
        cg.DOFade(1, 0.5f);
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    void HideStartButton()
    {
        CanvasGroup cg;
        cg = StartButton.GetComponent<CanvasGroup>();
        cg.DOFade(0, 0.5f);
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    public void MoveRight()
    {
        offset += 1;
        for (int i = 0; i < _cards.Length; i++)
        {
            var localIndex = (i + offset) % _cards.Length;
            _cards[i].transform.DOLocalMoveX(cardDistance.Evaluate(Math.Abs(localIndex - 2)) * (localIndex - 2), 0.5f);
            _cards[i].transform.DOScale(Vector3.one * (1f / (Math.Abs(localIndex - 2) + 1)), 0.5f);
            _cards[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = -Math.Abs(localIndex - 2);
            if (localIndex == 0)
            {
                _cards[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = -3;
            }
        }
        UpdateDeck(offset);
    }

    public void MoveLeft()
    {
        offset -= 1;
        if(offset < 0)
        {
            offset = characters.Count - offset;
        }
        for (int i = 0; i < _cards.Length; i++)
        {
            var localIndex = (i + offset) % _cards.Length;
            _cards[i].transform.DOLocalMoveX(cardDistance.Evaluate(Math.Abs(localIndex - 2)) * (localIndex - 2), 0.5f);
            _cards[i].transform.DOScale(Vector3.one * (1f / (Math.Abs(localIndex - 2) + 1)), 0.5f);
            _cards[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = -Math.Abs(localIndex - 2);
            if (localIndex == 4)
            {
                _cards[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = -3;
            }
        }
        UpdateDeck(offset);
    }

    void CreateNewCardPull()
    {
        characters = new List<CharacterModel>();
        for (int i= 0; i< 10;i++) 
        {
            CharacterModel character = new CharacterModel();
            character.GenerateRandom();
            characters.Add(character);
        }
        UpdateDeck(offset);

    }

    public void UpdateDeck(int offset)
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            _cards[i].GetComponent<CardScript>().character = characters[(i + offset) % characters.Count];
        }
        currentSelectedCharacter = _cards[3].GetComponent<CardScript>();
    }

    public void Recruit()
    {
        GameManager.I.AddTeamMember(currentSelectedCharacter.character);
        characters.Remove(currentSelectedCharacter.character);
        MoveRight();
    }

    public void StartGame()
    {
        GameManager.I.FinishSelectingTeam();
    }

}
