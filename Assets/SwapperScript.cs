using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwapperScript : MonoBehaviour
{
    [SerializeField] SpriteRenderer ren;
    [SerializeField] List<Sprite> swappers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnDisable()
    {
        GameManager.I.MainInput.Main.LMB.started -= LefMouseButtonDown;
    }

    private void OnEnable()
    {
        GameManager.I.MainInput.Main.LMB.started += LefMouseButtonDown;
				ren.sprite = swappers[0];
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LefMouseButtonDown(InputAction.CallbackContext obj)
    {
        StartCoroutine(Swap());
    }

    IEnumerator Swap()
    {
        ren.sprite = swappers[1];
        yield return new WaitForSeconds(0.5f);
        ren.sprite = swappers[0];
    }
}