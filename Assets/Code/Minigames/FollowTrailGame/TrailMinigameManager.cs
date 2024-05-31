using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TrailMinigameManager : MonoBehaviour, IMinigame
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] List<EdgeCollider2D> _trails = new List<EdgeCollider2D>();
    [SerializeField] TextMeshProUGUI _scoreTxt;
    [SerializeField] TextMeshProUGUI _timerUI;
    [SerializeField] AnimationCurve _timeCurve;
    [SerializeField] float _perfectScore = 25f;
    [SerializeField] Color _drawColor = Color.red;
    [SerializeField] AudioClip _clip;
    int _currentTrail = 0;
    Texture2D _texture;
    int _index = 0;
    List<Vector2> _points;
    bool _mouseOver;
    float _score = 0;
    float _timer;
    AudioSource _loop;

    public Action MinigameFinished { get; set; }

    public bool IsDisplayed => gameObject.activeInHierarchy;

    private void Awake()
    {
        _trails.Shuffle();
        transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        _texture = new Texture2D(Screen.width / 4, Screen.height / 4, TextureFormat.ARGB32, false)
        {
            anisoLevel = 0,
            filterMode = FilterMode.Point
        };
        for (int x = 0; x < Screen.width / 4; x++)
        {
            for (int y = 0; y < Screen.height / 4; y++)
            {
                _texture.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }
        _texture.Apply();
        _spriteRenderer.enabled = false;
        _currentTrail++;
        if (_currentTrail >= _trails.Count)
        {
            _currentTrail = 0;
            _trails.Shuffle();
        }
        _trails[_currentTrail].gameObject.SetActive(true);
        _index = 0;
        _scoreTxt.text = $"Score: {0}";
        _timer = 0;
        _timerUI.text = $"Move cursor to start to begin minigame!";
        _points = null;
    }

    void Update()
    {
        if (_points == null) return;
        var mousPos = GameManager.I.MainInput.Main.MousePos.ReadValue<Vector2>();
        var worldPos = (Vector2)Camera.main.ScreenToWorldPoint(mousPos);
        if (_index <= 0)
        {
            if ((worldPos - _points[0]).sqrMagnitude < 0.12f * 0.12f && _mouseOver)
            {

                _index = 1;
                _timer = _timeCurve.Evaluate(GameManager.I.StatsTeam.GetStat(StatsTypes.Art));
                _loop = Sound.PlaySoundAtPos(Vector3.zero, _clip, Sound.MixerTypes.SFX, sound2D: true);
                _loop.loop = true;
            }
            else
                return;
        }
        if (_timer <= 0) return;
        if (_index >= _points.Count) return;
        _timer -= Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(_timer);
        _timerUI.text = $"Time left {timeSpan.Seconds:D2}:{timeSpan.Milliseconds / 10:D2}";
        if (_timer <= 0)
        {
            if (_index < _points.Count)
            {
                _score -= (_points.Count - _index) / (float)_points.Count * _perfectScore * 0.1f;
                DisplayScore();
                _loop.Pause();
                MinigameFinished?.Invoke();
            }
            return;
        }
        CheckAllPoints(worldPos);
        _texture.SetPixel((int)(mousPos.x / 4), (int)(mousPos.y / 4), _drawColor);
        _texture.Apply();
        if (_index >= _points.Count)
        {
            _loop.Pause();
            MinigameFinished?.Invoke();
        }
    }

    void CheckAllPoints(Vector2 worldPos)
    {
        if (_timer <= 0) return;
        for (int i = _index; i < _points.Count; i++)
        {
            if (_mouseOver && (worldPos - _points[i]).sqrMagnitude < 0.11f * 0.11f)
            {
                if (i - _index > 0)
                {
                    _score -= (i - _index) / (float)_points.Count * _perfectScore * 0.8f;
                }
                else
                    _score += 1f / _points.Count * _perfectScore;
                _index = i + 1;
            }
        }
        DisplayScore();
    }

    void DisplayScore()
    {
        _score = Math.Clamp(_score, 0, _perfectScore);
        _scoreTxt.text = $"Score: {_score:n2}";
    }

    public void MouseEnter()
    {
        _mouseOver = true;
        if (_timer <= 0) return;
        _loop.UnPause();
    }

    public void MouseLeft()
    {
        _mouseOver = false;
        _loop.Pause();
        if (_timer <= 0) return;
        if (_index > 0 && _index < _points.Count)
            _score -= _perfectScore * 0.05f;
        DisplayScore();
    }

    private void OnDisable()
    {
        _trails.ForEach(trail => trail.gameObject.SetActive(false));
    }

    public StatsModel GetStatsFromGame()
    {
        return new StatsModel(StatsTypes.Art, _score);
    }

    public void CloseGame()
    {
        transform.DOKill(true);
        transform.DOScale(Vector3.zero, 1f).OnComplete(() => gameObject.SetActive(false)).SetEase(Ease.InSine);
        if (_loop != null)
        {
            _loop.Stop();
            Destroy(_loop.gameObject);
        }
    }

    public void ShowGame()
    {
        transform.DOKill(true);
        gameObject.SetActive(true);
        transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            _spriteRenderer.sprite = Sprite.Create(_texture, new Rect(0.0f, 0.0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f));
            var pointInWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            var pointInWorldSpriteBounds = _spriteRenderer.bounds.size / 2f;
            float scaleFactor = pointInWorld.x / pointInWorldSpriteBounds.x;
            _spriteRenderer.transform.localScale *= scaleFactor;
            _spriteRenderer.enabled = true;
            _spriteRenderer.transform.localPosition = _spriteRenderer.transform.parent.InverseTransformPoint(Vector2.zero);
            _points = _trails[_currentTrail].points.Select(p => (Vector2)_trails[_currentTrail].transform.TransformPoint(p)).ToList();
        });
    }
}
