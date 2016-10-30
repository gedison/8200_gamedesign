using UnityEngine;
using UnityEngine.UI;

public class TextSelect : MonoBehaviour {

    public GameObject gameObjectWithHealth;
    private Text selectedText;

    void Start () {
        selectedText = GetComponent<Text>();
	}

    void setGameObject(GameObject gm) {
        gameObjectWithHealth = gm;
    }
	
	void Update () {
        if (gameObjectWithHealth != null) {
            Health health = gameObjectWithHealth.GetComponent<Health>();
            selectedText.text = gameObjectWithHealth.name + "\n" + "Health: " + health.getCurrentHealth() + "/" + health.totalHealth;
        }else {
            selectedText.text = "";
        }
    }
}
