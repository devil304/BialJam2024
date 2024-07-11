using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HTPScript : MonoBehaviour
{
    private int activePanelIndex = 0;
    private int maxIndex;

    [SerializeField] RectTransform panelsContainer;
    [SerializeField] RectMask2D overflowMask;

    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;

    public void Start() {
        maxIndex = panelsContainer.childCount - 1;
        overflowMask.enabled = true;
        leftButton.interactable = false;
    }

    public void MoveRight() {
        if(activePanelIndex >= maxIndex) {
          return;
        }
        activePanelIndex++;
        ScrollPanels(activePanelIndex);
        leftButton.interactable = true;
    }

    public void MoveLeft() {
        if(activePanelIndex <= 0) {
          return;
        }
        activePanelIndex--;
        ScrollPanels(activePanelIndex);
        rightButton.interactable = true;
    }

    public void RestartPanelsPosition() {
      activePanelIndex = 0;
      ScrollPanels(activePanelIndex);
    }

    public void ScrollPanels(int panelIndex = 0) {
        if(panelIndex <= 0) {
          leftButton.interactable = false;
        }
        if(panelIndex >= maxIndex) {
          rightButton.interactable = false;
        }
        panelsContainer.DOLocalMoveX(-1250 * panelIndex, 0.5f).SetEase(Ease.Linear);
    }
}
