using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScreenSpaceDialog : MonoBehaviour {

    public Canvas canvas;
    public GameObject dialogBoxPrefab;

    private string dialogString;
    public float offset = 1.0f;

    private bool isPermanent = true;
    private float timeTilDestruction = 3.0f;

    private GameObject myDialogBox;
    private Text myDialogText;
    
    public void Start() {
        myDialogBox = Instantiate(dialogBoxPrefab) as GameObject;
        myDialogBox.transform.SetParent(canvas.transform, false);
        myDialogText = myDialogBox.GetComponentInChildren<Text>();

        if (!isPermanent) {
            Invoke("DestoryObject", timeTilDestruction);
        }
    }

    void LateUpdate() {
        Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        myDialogBox.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);
    }

    void DestroyObject() {
        Destroy(myDialogBox);
    }

    
    

    
}
