using UnityEngine;
using UnityEngine.UI;

public class GameStateText : MonoBehaviour {

    public Text myText;

    private string tutorialString1 = "Click a tile to move";
    private string tutorialString2 = "Right Mouse button to pan\nCenter Button to rotate";
    private string tutorialString3 = "Click a skill and an enemy to attack";
    private string tutorialString4 = "Different skills have different effects";

    public string startCombat = "Enemy Spotted: Begin Combat";
    public string winString = "All Enemies Defeated";
    public string loseString = "Game Over";
    public string winGameString = "You Defeated All The Enemies!\nYou Win!";
    public float textPopupTime = 3.0f;

    void Start () {
        myText.text = "";
	}


    private bool setCombatHasBeenCalled = false;
    private bool combatTutorialHasBeenCalled = false;
    public void setStartCombatString() {
        if (!setCombatHasBeenCalled) {
            setCombatHasBeenCalled = true;
            myText.text = tutorialString1;
            Invoke("DisableTextTutorial", textPopupTime);
        } else {
            myText.text = startCombat;
            if (!combatTutorialHasBeenCalled) {
                combatTutorialHasBeenCalled = true;
                Invoke("DisableTextTutorial2", textPopupTime);
            } else Invoke("DisableText", textPopupTime);
        } 
    }

    private bool setWinHasBeenCalled = false;
    public void setWinString() {
        if (!setWinHasBeenCalled) {
            setWinHasBeenCalled = true;
        }else {
            myText.text = winString;
            Invoke("DisableText", textPopupTime);
        }
    }

    public void setLoseString() {
        myText.text = loseString;
    }

    public void setGameWinString() {
        myText.text = winGameString;
    }

    void DisableText() {
        myText.text = "";
    }

    void DisableTextTutorial() {
        myText.text = tutorialString2;
        Invoke("DisableText", textPopupTime);
    }
    
    void DisableTextTutorial2() {
        myText.text = tutorialString3;
        Invoke("DisableTextTutorial3", textPopupTime);
    }

    void DisableTextTutorial3() {
        myText.text = tutorialString4;
        Invoke("DisableText", textPopupTime);
    }

}
