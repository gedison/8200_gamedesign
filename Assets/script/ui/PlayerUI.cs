using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

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
}
