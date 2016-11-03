using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public Text playerHealth;
    public Text playerAP;

    public Button switchToAttack;
    public Button switchToMovement;
    public Button endTurn;

    public GameObject player;

    private CharacterController myCharacterController;
    private Health myCharacterHealth;

	// Use this for initialization
	void Start () {
        myCharacterController = player.GetComponent<CharacterController>();

        switchToAttack.onClick.AddListener(setPlayerToAttack);
        switchToMovement.onClick.AddListener(setPlayerToMove);
        endTurn.onClick.AddListener(endPlayerTurn);
	}

    private void setPlayerToAttack() {
        if (player != null) myCharacterController.setCurrentCharacterState(CharacterController.CharacterState.ATTACK);
    }

    private void setPlayerToMove() {
        if (player != null) myCharacterController.setCurrentCharacterState(CharacterController.CharacterState.MOVE);
    }

    private void endPlayerTurn() {
        if (player != null) myCharacterController.endTurn();
    }
	
	void Update () {
        if (myCharacterHealth == null && player !=null) {
            myCharacterHealth = player.GetComponent<Health>(); 
        }
        if (player != null) { 
            if(myCharacterHealth!=null)playerHealth.text = "Health "+myCharacterHealth.getCurrentHealth() + "/" + myCharacterHealth.totalHealth;
            playerAP.text = "AP "+myCharacterController.getCurrentActionPoints() + "/" + myCharacterController.totalActionPoints;
        }
    }
}
