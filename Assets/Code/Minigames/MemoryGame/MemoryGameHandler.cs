using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;
using Unity.Mathematics;

public class MemoryGameHandler : MonoBehaviour
{

    public List<MemoryGameCardScript> flippedCards;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        transform.DOMove(Vector3.zero, 0.5f);
        flippedCards = new();
        InitCards();

    }


    // Update is called once per frame
    void Update()
    {
        if(flippedCards.Count == 2)
        {
            CheckCards(flippedCards[0], flippedCards[1]);
        }
    }

    void CheckCards(MemoryGameCardScript a, MemoryGameCardScript b)
    {
        if (a.cardNum == b.cardNum)
        {
            a.EraseCard();
            b.EraseCard();
        }
        else
        {
            a.FlipBackCard();
            b.FlipBackCard();
        }
        flippedCards = new();
    }

    void InitCards()
    {
        List<int> idList = new(){ 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8 };

        foreach (Transform child in transform)
        {
            if(child.tag == "Card")
            {
                int random = StrongRandom.RNG.Next(idList.Count);
                child.GetComponent<MemoryGameCardScript>().cardNum = idList[random];
                idList.RemoveAt(random);
            }

        }

    }
}
