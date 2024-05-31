using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroup menuCanvas;

    public List<Image> buttonsBackground;

    public List<Button> buttons;

    [SerializeField] AudioClip menuMusic;

    AudioSource source;

    Tween fadeTween;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        source = Sound.PlaySoundAtTarget(transform, menuMusic, Sound.MixerTypes.SFX, 1, sound2D: true, destroyAfter: true, initialFadeDur: 1f);
        source.loop = true;
        StartCoroutine(Test());


    }

    // Update is called once per frame
    void Update()
    {

    }


    void Fade(float end, float dur, CanvasGroup newCanvas, TweenCallback onEnd)
    {
        fadeTween?.Kill(true);

        fadeTween = newCanvas.DOFade(end, dur);
        fadeTween.onComplete += onEnd;
    }

    void FadeIn(float dur, CanvasGroup newCanvas)
    {

        Fade(1f, dur, newCanvas, () =>
        {
            ChangeAlpha(1);
            newCanvas.interactable = true;
            newCanvas.blocksRaycasts = true;
        });

    }

    void FadeOut(float dur, CanvasGroup newCanvas)
    {


        Fade(0f, dur, newCanvas, () =>
        {
            ChangeAlpha(0);
            newCanvas.interactable = true;
            newCanvas.blocksRaycasts = false;
        });

    }

    private IEnumerator Test()
    {
        yield return new WaitForSeconds(seconds: 0.5f);
        FadeIn(0.5f, menuCanvas);

    }

    public void ShowAnotherPanel(CanvasGroup to)
    {

        StartCoroutine(MoveFromOneToAnotherPanel(menuCanvas, to));

    }

    public void ShowMenu(CanvasGroup from)
    {
        StartCoroutine(MoveFromOneToAnotherPanel(from, menuCanvas));
    }

    private IEnumerator MoveFromOneToAnotherPanel(CanvasGroup from, CanvasGroup to)
    {
        FadeOut(0.5f, from);
        yield return new WaitForSeconds(0.1f);
        FadeIn(0.5f, to);

    }

    private void ChangeAlpha(int a)
    {
        buttonsBackground.ForEach(img => img.color = new Color(img.color.r, img.color.g, img.color.b, a));
    }

    public void ExitGame()
    {
        Debug.Log("Bye bye");
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

}
