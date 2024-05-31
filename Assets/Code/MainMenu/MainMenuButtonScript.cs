using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector2 startPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Im here!");
        transform.DOKill();
        transform.DOMoveX(startPos.x+100,1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Aaaand I'm not!");
        transform.DOKill();
        transform.DOMoveX(startPos.x, 1f);
    }


}
