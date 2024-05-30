using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrailMinigameManager : MonoBehaviour, IMinigame
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] EdgeCollider2D _collider;
    Texture2D _texture;
    int _index = 0;
    List<Vector2> _points;
    bool _mouseOver;

    public Action MinigameFinished { get; set; }

    public bool IsDisplayed => throw new NotImplementedException();

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
        _spriteRenderer.sprite = Sprite.Create(_texture, new Rect(0.0f, 0.0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f));
        var pointInWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        var pointInWorldSpriteBounds = _spriteRenderer.bounds.size / 2f;
        float scaleFactor = pointInWorld.x / pointInWorldSpriteBounds.x;
        _spriteRenderer.transform.localScale *= scaleFactor;
        _points = _collider.points.Select(p => (Vector2)_collider.transform.TransformPoint(p)).ToList();
        _index = 0;
    }

    void Update()
    {
        if (_index == _points.Count) return;
        var mousPos = GameManager.Instance.MainInput.Main.MousePos.ReadValue<Vector2>();
        var worldPos = (Vector2)Camera.main.ScreenToWorldPoint(mousPos);
        if (_index <= 0)
        {
            if ((worldPos - _points[0]).sqrMagnitude < 0.11f * 0.11f && _mouseOver)
            {
                _index = 1;

            }
            else
                return;
        }
        CheckAllPoints(worldPos);
        _texture.SetPixel((int)(mousPos.x / 4), (int)(mousPos.y / 4), Color.red);
        _texture.Apply();
    }

    void CheckAllPoints(Vector2 worldPos)
    {
        for (int i = _index; i < _points.Count; i++)
        {
            if (_mouseOver && (worldPos - _points[i]).sqrMagnitude < 0.11f * 0.11f)
            {
                if (i - _index > 0)
                    Debug.Log($"Missed points: {i - _index}");
                else
                    Debug.Log("Next point captured!");
                _index = i + 1;
            }
        }
    }

    public void MouseEnter()
    {
        _mouseOver = true;
    }

    public void MouseLeft()
    {
        _mouseOver = false;
        if (_index > 0 && _index < _points.Count)
            Debug.Log("Left line");
    }

    public StatsModel GetStatsFromGame()
    {
        //TODO
        return new();
    }

    public void CloseGame()
    {
        //TODO
    }

    public void ShowGame()
    {
        //TODO
    }
}
