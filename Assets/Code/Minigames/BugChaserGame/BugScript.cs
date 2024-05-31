using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using System.Collections;

public class BugScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public List<AudioClip> hits;
    public List<AudioClip> misses;
    public AudioClip walking;


    Vector3 coordinates;
    public BugChaserHandler handler;
    public Transform swapper;
    bool canHit;

    Coroutine loopCorotuine;
    AudioSource source;

    void Start()
    {
        source = Sound.PlaySoundAtTarget(transform, walking, Sound.MixerTypes.SFX, 1, sound2D: true, destroyAfter: true, initialFadeDur: 0f);
        source.loop = true;

        loopCorotuine = StartCoroutine(Loop());
        GameManager.I.MainInput.Main.LMB.started += LefMouseButtonDown;
    }

    private void OnDestroy()
    {
        GameManager.I.MainInput.Main.LMB.started -= LefMouseButtonDown;
    }

    private void LefMouseButtonDown(InputAction.CallbackContext obj)
    {
        if (canHit)
        {
            Sound.PlaySoundAtTarget(transform, hits[StrongRandom.RNG.Next(hits.Count - 1)], Sound.MixerTypes.SFX, 1, sound2D: true, destroyAfter: true);
            StartCoroutine(WaitAfterDead());
        }
        else
        {
            Sound.PlaySoundAtTarget(transform, misses[StrongRandom.RNG.Next(misses.Count - 1)], Sound.MixerTypes.SFX, 1, sound2D: true, destroyAfter: true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    Vector3 RandomCoordnidantes()
    {
        coordinates = new Vector3(StrongRandom.RNG.Next(-900, 900) / 2000f, StrongRandom.RNG.Next(-900, 900) / 2000f, 0);
        return coordinates;
    }

    IEnumerator Loop()
    {
        RandomCoordnidantes();
        var dur = (coordinates - transform.localPosition).magnitude / 1f;
        transform.DOLocalMove(coordinates, dur).SetEase(Ease.Linear);

        while (true)
        {
            yield return new WaitForSeconds(dur);
            RandomCoordnidantes();
            dur = (coordinates - transform.localPosition).magnitude / 1f;
            transform.DOLocalMove(coordinates, dur).SetEase(Ease.Linear);
        }
    }

    IEnumerator WaitAfterDead()
    {
        source.DOFade(0, 0.5f);
        Debug.Log("ded");
        StopCoroutine(loopCorotuine);
        transform.DOKill();

        yield return new WaitForSeconds(2);
        {
            handler.score += 1;
            Destroy(this.gameObject);
        }

    }

    private void OnMouseEnter()
    {
        canHit = true;
    }

    private void OnMouseExit()
    {
        canHit = false;
    }
}