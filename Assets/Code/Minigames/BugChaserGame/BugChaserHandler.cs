using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;
using System;
using DG.Tweening;

public class BugChaserHandler : MonoBehaviour, IMinigame
{
    public int score;
    public GameObject prefab;
    public Transform swapper;
    public TMP_Text scoreText;
    public TMP_Text timerText;
    StatsModel gameStats;
    private float currTime;
    public float countdownTime;

    [SerializeField]
    AnimationCurve timer;

    public Action MinigameFinished { get; set; }

    public bool IsDisplayed => gameObject.activeInHierarchy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private void OnEnable()
    {
        transform.DOMove(Vector3.zero, 0.5f);

        countdownTime = timer.Evaluate(GameManager.I.StatsTeam.GetStat(StatsTypes.QA));
        score = 0;
        StartCoroutine(NewBug());
        Cursor.visible = false;

        currTime = countdownTime;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = GameManager.I.MainInput.Main.MousePos.ReadValue<Vector2>();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = -1;
        swapper.position = mousePos;
            
        scoreText.text = $"Score: {score}";

        currTime -= Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(currTime);
        timerText.text = $"{time.Seconds} : {time.Milliseconds}";


        if (currTime <= 0)
        {
            timerText.text = "00:000";
            StopCoroutine(NewBug());
            foreach (Transform child in transform)
            {
                if(child.tag == "Bug")
                {
                    Destroy(child.gameObject);
                }
            }
            GameOver(score);
        }

    }

    void GameOver(float score)
    {
        Cursor.visible = true;
        gameStats = new((0, 0, 0, 0, score));
        MinigameFinished?.Invoke();
    }

    IEnumerator NewBug()
    {
        CreateNewBug(prefab);

        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            CreateNewBug(prefab);
        }
    }

    void CreateNewBug(GameObject bugPrefab)
    {
        GameObject newBug = Instantiate(bugPrefab,this.transform,false);
        newBug.transform.localPosition = new Vector3(StrongRandom.RNG.Next(-900, 900) / 2000f, StrongRandom.RNG.Next(-900, 900) / 2000f, 0);
        newBug.GetComponent<BugScript>().handler = this;
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
        transform.DOMove(Vector3.zero, 0.5f);
    }


}
