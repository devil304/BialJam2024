using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class RecruitmentHandler : MonoBehaviour
{
    [SerializeField] 
    List<Transform> checkpoints;

    [SerializeField]
    List<Transform> cards;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(MoveLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MoveLoop()
    {


        int i = 0;
        while (true)
        {
            yield return new WaitForSeconds(1);
            int y = 0;
            foreach (Transform t in cards)
            {
                t.transform.DOMove(checkpoints[(i+y)%5].position, 1f);
                t.transform.DOScale(checkpoints[(i+y) % 5].localScale, 1f);
                y++;
            }
            i++;
        }

    }

    void PreprareDeck()
    {

    }

    void InitFirstCards()
    {
        
    }


}
