using UnityEngine;
using UnityEngine.UI;



public class EndTurn : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void Start () {
        this.GetComponent<Button>().onClick.AddListener(endPlayerTurn);
    }

    private void endPlayerTurn() {
        Debug.Log("TesT");
        CharacterController myCharacterController = player.GetComponent<CharacterController>();
        myCharacterController.setActionPointsToZero();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
