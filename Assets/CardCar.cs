using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CardCar : MonoBehaviour
{
    [SerializeField] GameObject[] _cards;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SwapCards());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SwapCards()
    {
        int offset = 0;
        while (true)
        {
            for (int i = 0; i < _cards.Length; i++)
            {
                var localIndex = (i + offset) % _cards.Length;
                _cards[i].transform.DOLocalMoveX((localIndex - 2) * 0.75f, 1f);
                _cards[i].transform.DOScale(Vector3.one * (1f / (Math.Abs(localIndex - 2) + 1)), 1f);
            }
            offset = (offset + 1) % _cards.Length;
            yield return new WaitForSeconds(2);
        }
    }
}
