using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvas;
    
    public List<Button> buttons;

    Tween fadeTween;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Test());  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Fade(float end, float dur, TweenCallback onEnd)
    {
        fadeTween?.Kill();

        fadeTween = canvas.DOFade(end, dur);
        fadeTween.onComplete += onEnd;
    }

    void FadeIn(float dur)
    {
        Fade(1f, dur, () =>
        {
            canvas.interactable = true;
            canvas.blocksRaycasts = false;
        });
    }

    void FadeOut(float dur)
    {
        Fade(0f, dur, () =>
        {
            canvas.interactable = true;
            canvas.blocksRaycasts = false;
        });
    }

    private IEnumerator Test()
    {
        yield return new WaitForSeconds(2f);
        FadeIn(1f);
        
    }

    public void OpenPanel(CanvasGroup group)
    {

    }
}
