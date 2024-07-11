using DG.Tweening;
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


    public void OnPointerEnter(PointerEventData eventData)
    {
      // Debug.Log("On Hover");
      transform.DOKill();
      transform.DOLocalMoveX(100f,1f).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
      // transform.DOMove(new Vector3(100, 0, 0),1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      // Debug.Log("On Exit hover");
      transform.DOKill();
      // transform.DOMoveX(startPos.x, 2f);
      transform.DOLocalMoveX(0f, 1f).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }
}
