using UnityEngine;
using System.Collections;
    using UnityEngine.UI;
using UnityEngine.EventSystems;

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
