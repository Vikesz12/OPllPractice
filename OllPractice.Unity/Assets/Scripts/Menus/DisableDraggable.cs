using UnityEngine;
using UnityEngine.EventSystems;

public class DisableDraggable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private MonoBehaviour _draggableToDisable;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _draggableToDisable.enabled = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _draggableToDisable.enabled = true;
    }
}
