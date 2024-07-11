using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BounceHeadMenu : MonoBehaviour
{
    [SerializeField] Transform head;
    [SerializeField] float bunceSpeed;

    float startPos;
    float startScale;

    void Start()
    {
        startPos = head.localPosition.y;
        startScale = head.localScale.y;
        // head.localPosition += Vector3.up * _minMaxY;
        head
          .DOLocalMoveY(startPos * 0.95f, bunceSpeed)
          .SetLoops(-1, LoopType.Yoyo)
          .SetLink(head.gameObject, LinkBehaviour.KillOnDestroy);
        // head.localScale = Vector3.one - Vector3.one * _minMaxS;
        head
          .DOScale(startScale * 1.05f, bunceSpeed)
          .SetLoops(-1, LoopType.Yoyo)
          .SetLink(head.gameObject, LinkBehaviour.KillOnDestroy);
    }
}
