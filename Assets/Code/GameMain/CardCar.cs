using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;

public class CardCar : MonoBehaviour
{
    [SerializeField] GameObject[] _cards;
    [SerializeField] AnimationCurve cardDistance;
    private List<CharacterModel> characters;
    int offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    private void OnEnable()
    {
        offset = 0;
        CreateNewCardPull();
        MoveRight();
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void MoveRight()
    {
        offset += 1;
        for (int i = 0; i < _cards.Length; i++)
        {
            var localIndex = (i + offset) % _cards.Length;
            _cards[i].transform.DOLocalMoveX(cardDistance.Evaluate(Math.Abs(localIndex - 2)) * (localIndex - 2), 0.5f);
            _cards[i].transform.DOScale(Vector3.one * (1f / (Math.Abs(localIndex - 2) + 1)), 0.5f);
        }
        UpdateDeck(offset);
    }

    public void MoveLeft()
    {
        offset -= 1;
        for (int i = 0; i < _cards.Length; i++)
        {
            var localIndex = (i + offset) % _cards.Length;
            _cards[i].transform.DOLocalMoveX(cardDistance.Evaluate(Math.Abs(localIndex - 2)) * (localIndex - 2), 0.5f);
            _cards[i].transform.DOScale(Vector3.one * (1f / (Math.Abs(localIndex - 2) + 1)), 0.5f);
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
    }

}
