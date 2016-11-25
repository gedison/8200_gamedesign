using UnityEngine;
using UnityEngine.UI;

public class GameStateText : MonoBehaviour {

    public Text myText;

    public string startCombat = "Enemy Spotted: Begin Combat";
    public string winString = "All Enemies Defeated";
    public string loseString = "Game Over";
    public float textPopupTime = 4.0f;

    private bool firstTime = true;
    private bool firstTime2 = true;

    void Start () {
        myText.text = "";
	}

    public void setStartCombatString() {
        if (firstTime2) firstTime2 = false;
        else myText.text = startCombat;
        Invoke("DisableText", textPopupTime);
    }

    public void setWinString() {
        Debug.Log("WIN STRING CALLED");
        if (firstTime) firstTime = false;
        else myText.text = winString;
        Invoke("DisableText", textPopupTime);
    }

    public void setLoseString() {
        Debug.Log("LOSE CALLED");
      
        myText.text = loseString;
    }

    void DisableText() {
        myText.text = "";
    }

    void Update() {

    }
}
