using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextSelect : MonoBehaviour {

    public GameObject gameObjectWithHealth;
    private Text selectedText;

    // Use this for initialization
    void Start () {
        selectedText = GetComponent<Text>();
	}

    void setGameObject(GameObject gm) {
        gameObjectWithHealth = gm;
    }
	
	// Update is called once per frame
	void Update () {
        if (gameObjectWithHealth != null) {
            Health health = gameObjectWithHealth.GetComponent<Health>();
            selectedText.text = gameObjectWithHealth.name + "\n" + "Health: " + health.getCurrentHealth() + "/" + health.totalHealth;
        }else {
            selectedText.text = "";
        }
    }
}
