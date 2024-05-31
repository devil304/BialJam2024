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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _timer = _gameTime;
        for (int i = 0; i < GameManager.I.Team.Count; i++)
        {
            var charM = GameManager.I.Team[i];
            _teamSprites[i].UpdateSprites(charM.head, charM.hair, charM.accessory, charM.NickName);
        }
    }

    // Update is called once per frame
    void Update()
    {
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
        GameManager.I.ModifyStatsBasedOnTeam(_passivePointsSpeed * Time.deltaTime);
        GameManager.I.StatsAct.Clamp();
        for (int i = 0; i < _statSliders.Length; i++)
        {
            _statSliders[i].value = GameManager.I.StatsAct.GetStat((StatsTypes)i);
        }
    }
}
