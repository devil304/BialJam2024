using DG.Tweening;
using UnityEngine;

public class BounceHead : MonoBehaviour
{
    [SerializeField] float _minY = 0.05f, _maxY = 0.1f;
    [SerializeField] float _minS = 0f, _maxS = 0.015f;
    [SerializeField] Transform _head;
    float _minMaxY;
    float _minMaxS;
    [SerializeField] float _bunceSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _minMaxY = StrongRandom.RNG.Next((int)(_minY * 100), (int)(_maxY * 100)) / 100f;
        _minMaxS = _minMaxY.Remap(_minY, _maxY, _minS, _maxS);
        _head.localPosition += Vector3.up * _minMaxY;
        _head.DOLocalMoveY(-_minMaxY, _bunceSpeed).SetLoops(-1, LoopType.Yoyo);
        _head.localScale = Vector3.one - Vector3.one * _minMaxS;
        _head.DOScale(Vector3.one + Vector3.one * _minMaxS, _bunceSpeed).SetLoops(-1, LoopType.Yoyo);
    }
}
