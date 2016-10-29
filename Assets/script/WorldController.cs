using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WorldController : MonoBehaviour {

	public enum GameState { MOVE, ATTACK, IDLE, ENDED };
	public GameState currentState = GameState.MOVE;

    public static WorldController instance = null;
    public GameObject tiles;
    public GameObject player;
    public GameObject enemies;

    public Button changeStateButton;

    public int tileWidth, tileHeight;
    private Transform[] tileArray;
    private Node[] traversalMap;
    private Node savedNode = null;
    private TileTraverser tileTraverser = null;
    private int lastID;

    private int[] tilesEffectedByPlayerSkill;

    void Awake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        StartGame();
    }
		
    private void StartGame() {
        int[] weights = new int[tileHeight * tileWidth];
        tileArray = new Transform[tileHeight * tileWidth];
       
        int height = 0, width = 0;
        foreach (Transform tileRow in tiles.transform) {
            width = 0;
            foreach(Transform tile in tileRow.transform) {
                tileArray[(height * tileWidth) + width] = tile;
                weights[(height * tileWidth) + width] = tile.GetComponent<Tile>().getMovementModifier();
                width++;
            }height++;  
        } tileTraverser = new DijkstraTileTraverser(weights, tileWidth, tileHeight, true);

        changeStateButton.onClick.AddListener(changePlayerState);
    }

    private void changePlayerState() {
        CharacterController myCharacterController = player.GetComponent<CharacterController>();
        if (myCharacterController.getCurrentCharacterState() == CharacterController.CharacterState.ATTACK) {
            myCharacterController.setCurrentCharacterState(CharacterController.CharacterState.MOVE);
            changeStateButton.GetComponentInChildren<Text>().text = "Switch to Attack";
        } else {
            myCharacterController.setCurrentCharacterState(CharacterController.CharacterState.ATTACK);
            changeStateButton.GetComponentInChildren<Text>().text = "Switch to Movement";
        }
    }

    public Transform getTileFromArrayIndex(int tileID) {
        Debug.Log(tileID);
        return tileArray[tileID];
    }

    public int getTileIndexFromID(int tileID) {
        for (int i = 0; i < tileArray.Length; i++) {
            if (tileID == tileArray[i].GetInstanceID()) return i;
        }return -1;
    }

    public void switchTileIsOccupied(int tileID) {
        tileArray[getTileIndexFromID(tileID)].GetComponent<Tile>().tileIsOccupied = !tileArray[getTileIndexFromID(tileID)].GetComponent<Tile>().tileIsOccupied;
    }

    private void resetLastPath() {
        while (savedNode != null) {
            tileArray[savedNode.getID()].GetComponent<Tile>().setCurrentState(Tile.TileState.NOT_SELECTED);
            savedNode = savedNode.getPreviousNode();
        }

        if (tilesEffectedByPlayerSkill != null) {
            for (int i = 0; i < tilesEffectedByPlayerSkill.Length; i++) {
                tileArray[tilesEffectedByPlayerSkill[i]].GetComponent<Tile>().setCurrentState(Tile.TileState.NOT_SELECTED);
            }
        }
    }

    public void onTileHover(int tileID) {
        if (player.GetComponent<CharacterMovementController>().isCharacterMoving()) return;
        resetLastPath();

        CharacterController myCharacterController = player.GetComponent<CharacterController>();
        int index = getTileIndexFromID(tileID);
     
        switch (myCharacterController.getCurrentCharacterState()) {
            case CharacterController.CharacterState.MOVE:
                if (index != -1) {
                    if (traversalMap != null && traversalMap[index] != null) {
                        Node selectedNode = traversalMap[index];
                        savedNode = traversalMap[index];
                        while (selectedNode != null) {
                            int distanceFromStart = selectedNode.getDistanceFromStart();
                            if (distanceFromStart < player.GetComponent<CharacterMovementController>().movementSpeed) tileArray[selectedNode.getID()].GetComponent<Tile>().setCurrentState(Tile.TileState.SELECTED_WITHIN_RANGE);
                            else tileArray[selectedNode.getID()].GetComponent<Tile>().setCurrentState(Tile.TileState.SELECTED_OUTSIDE_RANGE);
                            selectedNode = selectedNode.getPreviousNode();
                        }
                    }
                }

                break;
            case CharacterController.CharacterState.ATTACK:
                tilesEffectedByPlayerSkill = myCharacterController.getTilesEffectedByCurrentSkill(index);
                for (int i = 0; i < tilesEffectedByPlayerSkill.Length; i++) {
                    tileArray[tilesEffectedByPlayerSkill[i]].GetComponent<Tile>().setCurrentState(Tile.TileState.SELECTED_WITHIN_RANGE);
                }

                break;
        }
    }

    public void onTileSelect(int tileID) {
        if (player.GetComponent<CharacterMovementController>().isCharacterMoving()) return;
        CharacterController myCharacterController = player.GetComponent<CharacterController>();

        switch (myCharacterController.getCurrentCharacterState()) {
		case CharacterController.CharacterState.MOVE:
			List<Node> path = new List<Node> ();
			while (savedNode != null) {
				if (savedNode.getDistanceFromStart () < player.GetComponent<CharacterMovementController> ().movementSpeed)
					path.Insert (0, savedNode);
				else
					tileArray [savedNode.getID ()].GetComponent<Tile> ().setCurrentState (Tile.TileState.NOT_SELECTED);
				savedNode = savedNode.getPreviousNode ();
			}if (path.Count > 0) player.GetComponent<CharacterMovementController>().setPath(path);
            break;
         case CharacterController.CharacterState.ATTACK:
            CharacterController.CharacterAttribute versus = myCharacterController.getCurrentSkillVersus();
            int skillDamage = myCharacterController.getDamageFromCurrentSkill();
            foreach (Transform enemy in enemies.transform) {
            int enemyPosition = getTileIndexFromID(enemy.GetComponent<CharacterPosition>().getCurrentInstanceID());
                for (int i = 0; i < tilesEffectedByPlayerSkill.Length; i++) {
					if (tilesEffectedByPlayerSkill[i] == enemyPosition) { 
                        int playerRole = myCharacterController.roleD20ForCurrentSkill();
                        if (enemy.GetComponent<CharacterController>().roleD20UsingAttributeAsModifier(versus) < playerRole) {
                            enemy.GetComponent<CharacterController>().myHealth.decrementCurrentHealthByX(skillDamage);
                            Debug.Log("HIT: " + skillDamage + " ENEMY HEALTH: " + enemy.GetComponent<CharacterController>().myHealth.getCurrentHealth());
                        }
                    }
                }
            }
            break;
        }
    }

    void Update() {
		/* Check player */
		switch (currentState) {
		case GameState.MOVE: // ALlow the user to move
			break;
		case GameState.ATTACK: // Attack/Skill state
			// Allow them to use skills
			// Allow them to click button to switch states
			// ESC switches back to move
			// Check action points
			break;
		case GameState.IDLE:
			// Allow them to end their turn, otherwise do nothing
			// Once they end turn, switch to ended.
			break;
		case GameState.ENDED:
			foreach (Transform enemy in enemies.transform) {
				//enemy.GetComponent<AI>.Run();
			}
			currentState = GameState.MOVE;
			break;
		}

		/* Go through all of the enemies */

        int currentID = player.GetComponent<CharacterPosition>().getCurrentInstanceID();
        if (currentID != lastID) {
            traversalMap = tileTraverser.getTileTrafersal(getTileIndexFromID(currentID));
            lastID = currentID;
        }

    }
}

