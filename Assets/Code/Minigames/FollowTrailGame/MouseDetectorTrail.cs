using UnityEngine;

public class MouseDetectorTrail : MonoBehaviour
{
    [SerializeField] TrailMinigameManager _manager;

    private void OnMouseEnter()
    {
        _manager.MouseEnter();
    }

    private void OnMouseExit()
    {
        _manager.MouseLeft();
    }
}
