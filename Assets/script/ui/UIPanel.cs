using UnityEngine;
using UnityEngine.EventSystems;

/* Prevents the player from clicking on the grid when their mouse is on the UI bar
 */

public class UIPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public bool onUi = false;

    public void OnPointerEnter(PointerEventData eventData) {
        onUi = true;
        WorldController.instance.onUI = true;
    }
    public void OnPointerExit(PointerEventData eventData) {
        onUi = false;
        WorldController.instance.onUI = false;
    }
}
