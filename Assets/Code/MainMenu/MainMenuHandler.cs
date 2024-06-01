using DG.Tweening;
using NUnit.Framework;
using System;
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

    [SerializeField] List<GameObject> developers;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        source = Sound.PlaySoundAtPos(transform.position, menuMusic, Sound.MixerTypes.BGMMain, 1, sound2D: true, destroyAfter: true, initialFadeDur: 1f);
        source.loop = true;
        DontDestroyOnLoad(source.gameObject); 
        source.gameObject.AddComponent<DestroyWhenSceneChangeTo>().SetSceneIndex(2);
        StartCoroutine(Test());

    }

    // Update is called once per frame
    void Update()
    {

    }


    void Fade(float end, float dur, CanvasGroup newCanvas, Action<CanvasGroup> onEnd)
    {
        fadeTween?.Kill(true);

        fadeTween = newCanvas.DOFade(end, dur);
        fadeTween.OnComplete(()=> {
            onEnd?.Invoke(newCanvas);
        });
    }

    void FadeIn(float dur, CanvasGroup newCanvas)
    {

        Fade(1f, dur, newCanvas, (cg) =>
        {
            cg.interactable = true;
            cg.blocksRaycasts = true;
            ChangeAlpha(1);
        });

    }

    void FadeOut(float dur, CanvasGroup newCanvas)
    {


        Fade(0f, dur, newCanvas, (cg) =>
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
            ChangeAlpha(0);
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
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void StartCredits (CanvasGroup to)
    {
        StartCoroutine(CreditsPresentation(menuCanvas,to));
    }

    private IEnumerator CreditsPresentation(CanvasGroup from, CanvasGroup to)
    {

        StartCoroutine(MoveFromOneToAnotherPanel(menuCanvas, to));

        foreach (GameObject dev in developers)
        {
            yield return new WaitForSeconds(1f);
            FadeIn(0.5f, dev.GetComponent<CanvasGroup>());
            yield return new WaitForSeconds(5f);
            FadeOut(0.5f, dev.GetComponent<CanvasGroup>());
            yield return new WaitForSeconds(1f);
        }

        

    }

}
