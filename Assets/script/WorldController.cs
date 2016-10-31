using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WorldController : MonoBehaviour {

	public enum GameState { IDLE, IN_COMBAT, ENDED };
	public GameState currentState = GameState.IDLE;

    public static WorldController instance = null;
    public GameObject tiles;
    public GameObject player;
    public GameObject enemies;
    public GameObject allCharacters;

    public Button changeStateButton;

    public int tileWidth, tileHeight;
    private Transform[] tileArray;
    private Node savedNode = null;
    private TileTraverser tileTraverser = null;
    private int lastID;

    private int[] tilesEffectedByPlayerSkill;

    private int currentPlayerTurn = 0;
    private ArrayList charactersInIntiative = new ArrayList();

    void Awake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        StartGame();
    }
		
    private void StartGame() {
        tileArray = new Transform[tileHeight * tileWidth];
       
        int height = 0, width = 0;
        foreach (Transform tileRow in tiles.transform) {
            width = 0;
            foreach(Transform tile in tileRow.transform) {
                tileArray[(height * tileWidth) + width] = tile;
                width++;
            }height++;  
        } tileTraverser = new DijkstraTileTraverser(tileArray, tileWidth, tileHeight, true);

        changeStateButton.onClick.AddListener(changePlayerState);

        charactersInIntiative.Add(player);



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
        //Debug.Log(tileID);
        return tileArray[tileID];
    }

    public int getTileIndexFromID(int tileID) {
        for (int i = 0; i < tileArray.Length; i++) {
            if (tileID == tileArray[i].GetInstanceID()) return i;
        }return -1;
    }

    public void switchTileIsOccupied(int tileID) {
        if(tileArray[getTileIndexFromID(tileID)] != null)
            tileArray[getTileIndexFromID(tileID)].GetComponent<Tile>().tileIsOccupied = !tileArray[getTileIndexFromID(tileID)].GetComponent<Tile>().tileIsOccupied;
    }

    private void addCharactersToIntiative() {
        if (!charactersInIntiative.Contains(player)) charactersInIntiative.Add(player);

        int playerX, playerY, enemyX, enemyY;
        int playerVisibilityRange = player.GetComponent<CharacterController>().visibilityRange;
        int playerOrigin = getTileIndexFromID(player.GetComponent<CharacterPosition>().getCurrentInstanceID());
        playerX = playerOrigin % tileWidth;
        playerY = playerOrigin / tileWidth;

        foreach (Transform enemy in enemies.transform) {
            int enemyPosition = getTileIndexFromID(enemy.GetComponent<CharacterPosition>().getCurrentInstanceID());
            enemyX = enemyPosition % tileWidth;
            enemyY = enemyPosition / tileWidth;

            int distance = (int)Mathf.Sqrt(Mathf.Pow((playerX - enemyX), 2) + Mathf.Pow((playerY - enemyY), 2));
            if (distance <= playerVisibilityRange && distance > 0) {
                if (!charactersInIntiative.Contains(enemy.gameObject)) charactersInIntiative.Add(enemy.gameObject);
            }
        }
    }

    private bool isPlayerWithinRangeOfEnemy() {
        bool playerIsWithinRangeOfEnemy = false;
        int playerX, playerY, enemyX, enemyY;

        int playerVisibilityRange = player.GetComponent<CharacterController>().visibilityRange;
        int playerOrigin = getTileIndexFromID(player.GetComponent<CharacterPosition>().getCurrentInstanceID());
        playerX = playerOrigin % tileWidth;
        playerY = playerOrigin / tileWidth;

        foreach (Transform enemy in enemies.transform) {
            int enemyPosition = getTileIndexFromID(enemy.GetComponent<CharacterPosition>().getCurrentInstanceID());
            enemyX = enemyPosition % tileWidth;
            enemyY = enemyPosition / tileWidth;

            int distance = (int)Mathf.Sqrt(Mathf.Pow((playerX - enemyX), 2) + Mathf.Pow((playerY - enemyY), 2));
            if (distance <= playerVisibilityRange && distance > 0) {
                playerIsWithinRangeOfEnemy = true;
                break;
            }
        }
        return playerIsWithinRangeOfEnemy;
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
        
        GameObject playerWhosTurnItIs = (currentState == GameState.IN_COMBAT) ? (GameObject)charactersInIntiative[currentPlayerTurn] : player;
        if(playerWhosTurnItIs.GetComponent<CharacterMovementController>().isCharacterMoving()) return;

        resetLastPath();

        CharacterController myCharacterController = playerWhosTurnItIs.GetComponent<CharacterController>();
        CharacterMovementController myCharacterMovementController = playerWhosTurnItIs.GetComponent<CharacterMovementController>();

        int index = getTileIndexFromID(tileID);

        switch (myCharacterController.getCurrentCharacterState()) {
            case CharacterController.CharacterState.MOVE:
                Node[] traversalMap = myCharacterMovementController.getTraversalMap();
         
                if (index != -1 && traversalMap != null && traversalMap[index] != null) {
                    Node selectedNode = traversalMap[index];
                    savedNode = traversalMap[index];
                    while (selectedNode != null) {
                        int distanceFromStart = selectedNode.getDistanceFromStart();
                        if (distanceFromStart < myCharacterMovementController.movementSpeed) tileArray[selectedNode.getID()].GetComponent<Tile>().setCurrentState(Tile.TileState.SELECTED_WITHIN_RANGE);
                        else tileArray[selectedNode.getID()].GetComponent<Tile>().setCurrentState(Tile.TileState.SELECTED_OUTSIDE_RANGE);
                        selectedNode = selectedNode.getPreviousNode();
                    }
                }break;

            case CharacterController.CharacterState.ATTACK:
                tilesEffectedByPlayerSkill = myCharacterController.getTilesEffectedByCurrentSkill(index);
                for (int i = 0; i < tilesEffectedByPlayerSkill.Length; i++) {
                    tileArray[tilesEffectedByPlayerSkill[i]].GetComponent<Tile>().setCurrentState(Tile.TileState.SELECTED_WITHIN_RANGE);
                }
                break;
        }
    }

    public void onTileSelect(int tileID) {
        GameObject playerWhosTurnItIs = (currentState == GameState.IN_COMBAT) ? (GameObject)charactersInIntiative[currentPlayerTurn] : player;
        if (playerWhosTurnItIs.GetComponent<CharacterMovementController>().isCharacterMoving()) return;

        CharacterController myCharacterController = playerWhosTurnItIs.GetComponent<CharacterController>();
        CharacterMovementController myCharacterMovementController = playerWhosTurnItIs.GetComponent<CharacterMovementController>();

        switch (myCharacterController.getCurrentCharacterState()) {
            case CharacterController.CharacterState.MOVE:
                if (myCharacterController.getCurrentActionPoints() < 2) return;
                List<Node> path = new List<Node>();
                while (savedNode != null) {
                    if (savedNode.getDistanceFromStart() < myCharacterMovementController.movementSpeed)
                        path.Insert(0, savedNode);
                    else
                        tileArray[savedNode.getID()].GetComponent<Tile>().setCurrentState(Tile.TileState.NOT_SELECTED);
                    savedNode = savedNode.getPreviousNode();
                }

                if (path.Count > 0) {
                    myCharacterMovementController.setPath(path);
                    if (currentState == GameState.IN_COMBAT) myCharacterController.decrementActionPointsByMovement();
                }break;

            case CharacterController.CharacterState.ATTACK:
                if(myCharacterController.getCurrentActionPoints()<3)return;

                CharacterController.CharacterAttribute versus = myCharacterController.getCurrentSkillVersus();
                int skillDamage = myCharacterController.getDamageFromCurrentSkill();
                bool enemyHit = false;

                if (playerWhosTurnItIs == player) {
                    foreach (Transform enemy in enemies.transform) {
                        int enemyPosition = getTileIndexFromID(enemy.GetComponent<CharacterPosition>().getCurrentInstanceID());
                        for (int i = 0; i < tilesEffectedByPlayerSkill.Length; i++) {
                            if (tilesEffectedByPlayerSkill[i] == enemyPosition) {
                                enemyHit = true;
                                int playerRole = myCharacterController.roleD20ForCurrentSkill();
                                if (enemy.GetComponent<CharacterController>().roleD20UsingAttributeAsModifier(versus) < playerRole) {
                                    enemy.GetComponent<CharacterController>().myHealth.decrementCurrentHealthByX(skillDamage);
                                }
                            }
                        }
                    }
                } else {
                    int enemyPosition = getTileIndexFromID(player.GetComponent<CharacterPosition>().getCurrentInstanceID());
                    for (int i = 0; i < tilesEffectedByPlayerSkill.Length; i++) {
                        if (tilesEffectedByPlayerSkill[i] == enemyPosition) {
                            enemyHit = true;
                            int playerRole = myCharacterController.roleD20ForCurrentSkill();
                            if (player.GetComponent<CharacterController>().roleD20UsingAttributeAsModifier(versus) < playerRole) {
                                player.GetComponent<CharacterController>().myHealth.decrementCurrentHealthByX(skillDamage);
                            }
                        }
                    }
                }

                if (currentState == GameState.IN_COMBAT && enemyHit) myCharacterController.decrementActionPointsByAttack();
                break;
        }
    }
  
    public void updateTraversalMap(bool forceUpdate) {
        if (player != null) {
            if (player.GetComponent<CharacterMovementController>().doesTraversalMapNeedToBeUpdated() || forceUpdate) {
                int currentID = player.GetComponent<CharacterPosition>().getCurrentInstanceID();
                player.GetComponent<CharacterMovementController>().setTraversalMap(tileTraverser.getTileTrafersal(getTileIndexFromID(currentID)));
            }
        }

        foreach(Transform enemy in enemies.transform) {
            if (enemy != null) {
                if (enemy.GetComponent<CharacterMovementController>().doesTraversalMapNeedToBeUpdated() || forceUpdate) {
                    int currentID = enemy.GetComponent<CharacterPosition>().getCurrentInstanceID();
                    enemy.GetComponent<CharacterMovementController>().setTraversalMap(tileTraverser.getTileTrafersal(getTileIndexFromID(currentID)));
                }
            }
        }
    }

    void Update() {
        if(currentState == GameState.IDLE) {
            Debug.Log("IDLE GAMESTATE");
            if (isPlayerWithinRangeOfEnemy()) currentState = GameState.IN_COMBAT;
        }else if(currentState == GameState.IN_COMBAT) {
            Debug.Log("COMBAT STATE");
            addCharactersToIntiative();

            GameObject characterWhosTurnItIs = (GameObject)charactersInIntiative[currentPlayerTurn];
            if (characterWhosTurnItIs == null) charactersInIntiative.Remove(characterWhosTurnItIs);
            else {
               
                CharacterController myCharacterController = characterWhosTurnItIs.GetComponent<CharacterController>();
                if (myCharacterController.getCurrentCharacterState() == CharacterController.CharacterState.IDLE) {
                    myCharacterController.startTurn();
                }


                Debug.Log("PLAYER: " + characterWhosTurnItIs.name + " AP: " + myCharacterController.getCurrentActionPoints() + "/" + myCharacterController.totalActionPoints);

                //Check if turn is over
                if (myCharacterController.getCurrentActionPoints() <= 0 && !characterWhosTurnItIs.GetComponent<CharacterMovementController>().isCharacterMoving()) {
                    myCharacterController.endTurn();
                    Debug.Log("PLAYER: " + characterWhosTurnItIs.name);
                    currentPlayerTurn++;
                    if (currentPlayerTurn >= charactersInIntiative.Count) currentPlayerTurn = 0;
                }
            }

            if(charactersInIntiative.Count == 1) {
                if (charactersInIntiative.Contains(player)) {
                    currentState = GameState.IDLE;
                    changePlayerState();
                }
            }

            updateTraversalMap(true);
        }

        updateTraversalMap(false);
    }
}

