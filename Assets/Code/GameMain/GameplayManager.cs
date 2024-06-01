using System;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] AnomalySystem _anomalySystem;
    [SerializeField] EndGameManager _endGameManager;
    [SerializeField] AudioClip _clip;
    float _timer;
    float _mGameTimer;
    float _anomalyTimer;
    IMinigame _actMG = null;
    bool _anomalyOn = false;
    List<int> _chances = new List<int>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _timer = _gameTime;
        for (int i = 0; i < GameManager.I.Team.Count; i++)
        {
            var charM = GameManager.I.Team[i];
            _teamSprites[i].UpdateSprites(charM.head, charM.hair, charM.accessory, charM.NickName);
        }
        _mGameTimer = StrongRandom.RNG.Next(6, 9);
        _anomalyTimer = StrongRandom.RNG.Next(15, 18);
        _anomalySystem.OnAnomalyStart += AnomalyStart;
        _anomalySystem.OnAnomalyEnd += AnomalyEnd;
        for (int i = 0; i < 5; i++)
        {
            int[] arr = new int[Mathf.RoundToInt(GameManager.I.StatsTeam.Stats[i])];
            Array.Fill(arr, i);
            _chances.AddRange(arr);
        }
        _chances.Shuffle();
        Debug.Log($"Team stats: Code {GameManager.I.StatsTeam.GetStat(StatsTypes.Code)}, Design {GameManager.I.StatsTeam.GetStat(StatsTypes.Design)}, Art {GameManager.I.StatsTeam.GetStat(StatsTypes.Art)}, Audio {GameManager.I.StatsTeam.GetStat(StatsTypes.Audio)}, QA {GameManager.I.StatsTeam.GetStat(StatsTypes.QA)}");
        var ThatASS = Sound.PlaySoundAtPos(Vector3.zero, _clip, Sound.MixerTypes.BGMMain, sound2D: true, initialFadeDur: 1f);
        ThatASS.loop = true;
    }

    private void AnomalyEnd()
    {
        _anomalyOn = false;
        _anomalyTimer = StrongRandom.RNG.Next(10, 15);
    }

    private void AnomalyStart()
    {
        _anomalyOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_actMG != null || _anomalyOn) return;
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(_timer);
        _timeUI.text = $"Time left to make game {time.Seconds:D2}:{time.Milliseconds / 10:D2}";
        if (_timer <= 0)
        {
            //GameManager.I.LoadMenu();
            enabled = false;
						HandleEndGame();
            return;
        }
        if (_mGameTimer > 0)
        {
            _mGameTimer -= Time.deltaTime;
            if (_mGameTimer <= 0)
            {
                // var filtered = _chances.Where(i => GameManager.I.StatsAct.GetStat((StatsTypes)i) < 90f).ToList();
                // _actMG = GameManager.I._minigames[filtered[StrongRandom.RNG.Next(0, filtered.Count)]];
								_actMG = GetRandomHelpfullGame();
                _actMG.ShowGame();
                _actMG.MinigameFinished += MGFinished;

                return;
            }
        }
        if (_anomalyTimer > 0)
        {
            _anomalyTimer -= Time.deltaTime;
            if (_anomalyTimer <= 0)
            {
                _anomalySystem.StartAnomaly();
                return;
            }
        }
        GameManager.I.ModifyStatsBasedOnTeam(_passivePointsSpeed * Time.deltaTime);
        GameManager.I.StatsAct.Clamp();
        for (int i = 0; i < _statSliders.Length; i++)
        {
            _statSliders[i].value = GameManager.I.StatsAct.GetStat((StatsTypes)i);
        }
    }

		private IMinigame GetRandomHelpfullGame() {
			var filtered = _chances.Where(i => GameManager.I.StatsAct.GetStat((StatsTypes)i) < _endGameManager.GetMinScoreToWin()).ToList();

			if(filtered.Count > 0) {
				_chances.ForEach(i => {
					if (GameManager.I.StatsAct.GetStat((StatsTypes)i) < _endGameManager.GetMinScoreToWin()/2) {
						filtered.Add(i);
					}
				});
				return GameManager.I._minigames[filtered[StrongRandom.RNG.Next(0, filtered.Count)]];
			}

			return GameManager.I._minigames[StrongRandom.RNG.Next(0, _chances.Count)];
		}

    private void MGFinished()
    {
        _actMG.MinigameFinished -= MGFinished;
        GameManager.I.ModifyStats(_actMG.GetStatsFromGame());
        _actMG.CloseGame();
        _actMG = null;
        _mGameTimer = StrongRandom.RNG.Next(4, 8);
    }

		private void HandleEndGame() {
			_endGameManager.EndGame();
		}
}
