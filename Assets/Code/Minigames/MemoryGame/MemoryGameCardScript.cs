using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using System;
using Sequence = DG.Tweening.Sequence;
using NUnit.Framework.Constraints;

public class MemoryGameCardScript : MonoBehaviour
{
    private Transform CardTransform;
    MemoryGameHandler mh;
    Vector3 rotationZ = new Vector3(0, 0, 10);
    Vector3 rotationX = new Vector3(0, 180, 0);
    public bool isFlipped;
    Sequence _anim;
    public int cardNum;

    public GameObject awers;
    public GameObject rewers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isFlipped = false;
        CardTransform = this.gameObject.GetComponent<Transform>();
        mh = transform.parent.GetComponent<MemoryGameHandler>();
        ///test
        //cardNum = 1;
    }

    void StartAnim()
    {
        CardTransform.DOScale(0.19f, 0.15f);

        _anim = DOTween.Sequence();
        _anim.Append(CardTransform.DORotate(rotationZ, 0.5f, RotateMode.Fast)
            .SetEase(Ease.OutSine));
        _anim.Append(CardTransform.DORotate(-rotationZ, 1f, RotateMode.Fast)
            .SetEase(Ease.InOutSine));
        _anim.Append(CardTransform.DORotate(Vector3.zero, 0.5f, RotateMode.Fast)
            .SetEase(Ease.InSine));
        _anim.SetLoops(-1);
    }

    private void OnMouseEnter()
    {
        if(!isFlipped)
        {
            StartAnim();
        }
    }

    private void OnMouseExit()
    {
        if (!isFlipped)
        {
            StopAnim();
            ResetScale();
        }
    }

    private void StopAnim(Action callback = null)
    {
        _anim.Kill(true);
        CardTransform.DORotate(Vector3.zero, 1f, RotateMode.Fast)
            .SetEase(Ease.InOutSine).OnComplete(()=>callback?.Invoke());
    }

    private void ResetScale()
    {
        CardTransform.DOScale(0.15f, 0.15f);
    }

    private void OnMouseDown()
    {
        if(!isFlipped)
        {
            StopAnim();
            isFlipped = true;
            CardTransform.DOKill(true);
            CardTransform.DORotate(rotationX, 0.5f).OnComplete(() => SendCardToHandler());
        }
    }

    void SendCardToHandler()
    {
        mh.flippedCards.Add(this);
    }

    public void EraseCard()
    {
        _anim.Kill(true);
        CardTransform.DOScale(0f,0.25f)
        .OnComplete(() => Destroy(this.gameObject));
    }

    public void FlipBackCard()
    {
        ResetScale();
        StopAnim(()=>isFlipped = false);
    }
}
