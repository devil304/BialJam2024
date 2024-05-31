using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] Slider[] _statSliders;
    [SerializeField] float _passivePointsSpeed = 1f;
    [SerializeField] float _gameTime;
    [SerializeField] TextMeshProUGUI _timeUI;
    [SerializeField] BounceHead[] _teamSprites;
    float _timer;
    float _mGameTimer;
    float _anomalyTimer;
    IMinigame _actMG = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _timer = _gameTime;
        for (int i = 0; i < GameManager.I.Team.Count; i++)
        {
            var charM = GameManager.I.Team[i];
            _teamSprites[i].UpdateSprites(charM.head, charM.hair, charM.accessory, charM.NickName);
        }
        _mGameTimer = StrongRandom.RNG.Next(6, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (_actMG != null) return;
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(_timer);
        _timeUI.text = $"Time left to make game {time.Seconds:D2}:{time.Milliseconds / 10:D2}";
        if (_timer <= 0)
        {
            GameManager.I.LoadMenu();
            enabled = false;
            return;
        }
        if (_mGameTimer > 0)
        {
            _mGameTimer -= Time.deltaTime;
            if (_mGameTimer <= 0)
            {
                _actMG = GameManager.I._minigames[StrongRandom.RNG.Next(0, 5)];
                _actMG.ShowGame();
                _actMG.MinigameFinished += MGFinished;
            }
        }
        GameManager.I.ModifyStatsBasedOnTeam(_passivePointsSpeed * Time.deltaTime);
        GameManager.I.StatsAct.Clamp();
        for (int i = 0; i < _statSliders.Length; i++)
        {
            _statSliders[i].value = GameManager.I.StatsAct.GetStat((StatsTypes)i);
        }
    }

    private void MGFinished()
    {
        _actMG.MinigameFinished -= MGFinished;
        GameManager.I.ModifyStats(_actMG.GetStatsFromGame());
        _actMG.CloseGame();
        _actMG = null;
        _mGameTimer = StrongRandom.RNG.Next(6, 10);
    }
}
