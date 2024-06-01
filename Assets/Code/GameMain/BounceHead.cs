using DG.Tweening;
using TMPro;
using UnityEngine;

public class BounceHead : MonoBehaviour
{
    [SerializeField] float _minY = 0.05f, _maxY = 0.1f;
    [SerializeField] float _minS = 0f, _maxS = 0.015f;
    [SerializeField] Transform _head;
    [SerializeField] TextMeshProUGUI _name;
    float _minMaxY;
    float _minMaxS;
    [SerializeField] float _bunceSpeed;
    [SerializeField] SpriteRenderer _headSR, _hairSR, _accSR, _bodySR, _deskSR, _shadowSR;
    [SerializeField] int _baseOrderHead, _baseOrderHair, _baseOrderAcc, _baseOrderBody, _baseOrderDesk, _baseOrderShadow;

    private void OnValidate()
    {
        if (_baseOrderHead <= 0)
        {
            Init();
        }
    }

    private void Init()
    {
        if (_headSR)
            _baseOrderHead = _headSR.sortingOrder;
        if (_hairSR)
            _baseOrderHair = _hairSR.sortingOrder;
        if (_accSR)
            _baseOrderAcc = _accSR.sortingOrder;
        if (_bodySR)
            _baseOrderBody = _bodySR.sortingOrder;
        if (_deskSR)
            _baseOrderDesk = _deskSR.sortingOrder;
        if (_shadowSR)
            _baseOrderShadow = _shadowSR.sortingOrder;
    }

    public void ShiftOrder(int offset)
    {
        if (_headSR)
            _headSR.sortingOrder = _baseOrderHead + offset;
        if (_hairSR)
            _hairSR.sortingOrder = _baseOrderHair + offset;
        if (_accSR)
            _accSR.sortingOrder = _baseOrderAcc + offset;
        if (_bodySR)
            _bodySR.sortingOrder = _baseOrderBody + offset;
        if (_deskSR)
            _deskSR.sortingOrder = _baseOrderDesk + offset;
        if (_shadowSR)
            _shadowSR.sortingOrder = _baseOrderShadow + offset;
    }

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

    public void UpdateSprites(Sprite headSR, Sprite hairSR, Sprite accSR, string myName)
    {
        _headSR.sprite = headSR;
        if (!hairSR || hairSR == null)
            _hairSR.enabled = false;
        else
            _hairSR.enabled = true;
        _hairSR.sprite = hairSR;
        if (!accSR || accSR == null)
            _accSR.enabled = false;
        else
            _accSR.enabled = true;
        _accSR.sprite = accSR;
        _name.text = myName;
    }
}
