using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwapperScript : MonoBehaviour
{
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] List<Sprite> swappers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.I.MainInput.Main.LMB.started += LefMouseButtonDown;
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
        renderer.sprite = swappers[1];
        yield return new WaitForSeconds(0.5f);
        renderer.sprite = swappers[0];
    }
}