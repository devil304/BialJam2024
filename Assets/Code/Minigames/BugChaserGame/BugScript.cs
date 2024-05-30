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

    Vector3 coordinates;
    public BugChaserHandler handler;
    public Transform swapper;

    void Start()
    {
        StartCoroutine(Loop());
    }

    // Update is called once per frame
    void Update()
    {

    }

    Vector3 RandomCoordnidantes()
    {
        coordinates = new Vector3(StrongRandom.RNG.Next(-900, 900) /2000f, StrongRandom.RNG.Next(-900, 900) / 2000f,0);
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

    private void OnMouseDown()
    {
        handler.score += 1;
        Destroy(this.gameObject);
    }
}