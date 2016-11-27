using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;


//http://www.sharkbombs.com/2015/02/10/tooltips-with-the-new-unity-ui-ugui/


public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    

    public GameObject toolTipPrefab;
    public GameObject toolTip;
    public Canvas canvas;

    private bool shouldHoverBeStarted = true;

    public void Start() {
        toolTip = Instantiate(toolTipPrefab) as GameObject;
        toolTip.transform.SetParent(canvas.transform, false);
        toolTip.transform.position = new Vector3(transform.position.x, transform.position.y+110, transform.position.z);
        toolTip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Invoke("StartHover", .75f);
        shouldHoverBeStarted = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        shouldHoverBeStarted = false;
        toolTip.SetActive(false);
    }

    private void StartHover() {
        if (shouldHoverBeStarted) {
            toolTip.SetActive(true);
        }
    }
}