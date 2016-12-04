using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
        Invoke("StartHover", .5f);
        shouldHoverBeStarted = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        shouldHoverBeStarted = false;
        toolTip.SetActive(false);
    }

    public void setMove(int playerMovement) {
        Text[] textInToolTip = toolTip.GetComponentsInChildren<Text>();
        for (int i = 0; i < textInToolTip.Length; i++) {
            switch (textInToolTip[i].name) {
                case "Title":
                    textInToolTip[i].text = "Movement";
                    break;
                case "Cost":
                    textInToolTip[i].text = "2 AP";
                    break;
                case "Description":
                    textInToolTip[i].text = "Moving with a lithe grace, you frolic from tile to tile up to "+playerMovement+" squares. You can move twice per turn, or once per turn with an attack.";
                    break;
                case "Damage":
                    textInToolTip[i].text = "No Damage";
                    break;
            }
        }
    }

    public void setTipToValues(string[] values) {
        Text[] textInToolTip = toolTip.GetComponentsInChildren<Text>();
        for (int i = 0; i < textInToolTip.Length; i++) {
            switch (textInToolTip[i].name) {
                case "Title":
                    textInToolTip[i].text = values[i];
                    break;
                case "Cost":
                    textInToolTip[i].text = values[i];
                    break;
                case "Description":
                    textInToolTip[i].text = values[i];
                    break;
                case "Damage":
                    textInToolTip[i].text = values[i];
                    break;
            }
        }
    }

    public void setSkill(Skill mySkill) {
        Text[] textInToolTip = toolTip.GetComponentsInChildren<Text>();
        for(int i=0; i<textInToolTip.Length; i++) {
            switch (textInToolTip[i].name) {
                case "Title":
                    textInToolTip[i].text = mySkill.getSkillName();
                    break;
                case "Cost":
                    textInToolTip[i].text = "3 AP";
                    break;
                case "Description":
                    textInToolTip[i].text = mySkill.getSkillDescription();
                    break;
                case "Damage":
                    textInToolTip[i].text = mySkill.toString();
                    break;
            }
        }
    }

    private void StartHover() {
        if (shouldHoverBeStarted) {
            toolTip.transform.position = new Vector3(transform.position.x-50, transform.position.y + 110, transform.position.z);
            toolTip.SetActive(true);
        }
    }
}