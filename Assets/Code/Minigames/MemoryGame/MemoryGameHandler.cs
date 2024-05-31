using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using TMPro;

public class MemoryGameHandler : MonoBehaviour, IMinigame
{

    public List<MemoryGameCardScript> flippedCards;
    public TMP_Text countdownTimerText;
    private float currTime;
    public float countdownTime;
    StatsModel gameStats;

    [SerializeField]
    AnimationCurve timer;

    public Action MinigameFinished { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {
        transform.DOMove(Vector3.zero, 0.5f);

        countdownTime = timer.Evaluate(GameManager.I.StatsTeam.GetStat(StatsTypes.Design));


        flippedCards = new();

        InitCards(countdownTime);
    }

    void Start()
    {

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

    void InitCards(float time)
    {
        currTime = time;
        List<int> idList = new() { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8 };

        foreach (Transform child in transform)
        {
            if (child.tag == "Card")
            {
                int random = StrongRandom.RNG.Next(idList.Count);
                child.GetComponent<MemoryGameCardScript>().cardNum = idList[random];
                idList.RemoveAt(random);
            }
        }

    }

    void Update()
    {
        if (flippedCards.Count == 2)
        {
            CheckCards(flippedCards[0], flippedCards[1]);
        }

        if (transform.childCount - 2 != 0)
        {
            currTime -= Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(currTime);
            countdownTimerText.text = time.Seconds.ToString() + ":" + time.Milliseconds.ToString();

            if (currTime <= 0)
            {
                GameOver(transform.childCount - 2);
                countdownTimerText.text = "00:000";
            }
        }
        else
        {
            GameOver(Mathf.CeilToInt(countdownTime - currTime) + 16);
        }

    }


    void GameOver(float score)
    {

        foreach (Transform child in transform)
        {
            if (child.tag == "Card")
            {
                child.GetComponent<MemoryGameCardScript>().isGameOver = true;
            }
        }

        gameStats = new((0, score, 0, 0, 0));
        MinigameFinished?.Invoke();
    }

    public StatsModel GetStatsFromGame()
    {
        return gameStats;
    }

    public void CloseGame()
    {
        transform.DOMove(new Vector3(0, 20, 0), 0.5f).OnComplete(() => this.gameObject.SetActive(false));
    }

    public void ShowGame()
    {
        this.gameObject.SetActive(true);
    }

    public bool IsDisplayed => gameObject.activeInHierarchy;
}
