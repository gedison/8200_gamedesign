using UnityEngine;
using UnityEngine.UI;

public class ToolTipView : MonoBehaviour {

    public Text tooltipText;

    private static ToolTipView instance;
    public static ToolTipView Instance {
        get {
            if (instance == null)
                instance = GameObject.FindObjectOfType<ToolTipView>();
            return instance;
        }
    }

    void Awake() {
        instance = this;
        HideToolTip();
    }

    public bool IsActive {
        get {
            return gameObject.activeSelf;
        }
    }

    public void ShowToolTip(string text, Vector3 pos) {
        if (tooltipText.text != text)
            tooltipText.text = text;

        transform.position = pos;

        gameObject.SetActive(true);
    }

    public void HideToolTip() {
        gameObject.SetActive(false);
    }
}
