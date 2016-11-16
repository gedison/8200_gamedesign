using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldController : MonoBehaviour {

    public static WorldController instance = null;

    public enum GameState { IDLE, IN_COMBAT };
	public GameState currentState = GameState.IDLE;
    
    public GameObject tiles;
    public GameObject player;
    public GameObject enemies;
    public GameObject destructableObjects;
    private ArrayList allCharacters = new ArrayList();

	public ExperienceManager xpManager;
	public QuestManager qManager;

    public int tileWidth, tileHeight;
    private TileController myTileController;
    private InitativeController myInitativeController;

    public bool onUI = false;

    void Awake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        StartGame();
    }
		
    private void StartGame() {
        myTileController = new TileController(tiles, tileWidth, tileHeight);
        myInitativeController = new InitativeController(player, enemies, myTileController);

		qManager = new QuestManager ();

        allCharacters.Add(player);
        foreach (Transform enemy in enemies.transform) allCharacters.Add(enemy.gameObject);

    }

    public Transform getTileFromArrayIndex(int tileID) {
        return myTileController.getTileFromID(tileID);
    }

    public void switchTileIsOccupied(int tileID) {
        if(myTileController!=null)myTileController.switchIsTileOccupied(tileID);
    }

    private void haveCharacterPreformAttackOnTile(GameObject playerWhosTurnItIs, int tileID) {
        CharacterController myCharacterController = playerWhosTurnItIs.GetComponent<CharacterController>();

        if (myCharacterController.getCurrentActionPoints() < 3 && currentState == GameState.IN_COMBAT) return;

        CharacterController.CharacterAttribute versus = myCharacterController.getCurrentSkillVersus();
       
        bool enemyHit = false;

        int[] tilesEffectedByPlayerSkill = myTileController.getLastAttack();

        foreach (GameObject character in allCharacters) {
            if (character != null) {
                int characterPosition = character.GetComponent<CharacterPosition>().getTileID();
                for (int i = 0; i < tilesEffectedByPlayerSkill.Length; i++) {
                    if (tilesEffectedByPlayerSkill[i] == characterPosition) {
                        enemyHit = true;
                        int playerRole = myCharacterController.roleD20ForCurrentSkill();
                        if (character != playerWhosTurnItIs && character.GetComponent<CharacterController>().roleD20UsingAttributeAsModifier(versus) < playerRole) {
                            int skillDamage = myCharacterController.getDamageFromCurrentSkill();
                            character.GetComponent<CharacterController>().myHealth.decrementCurrentHealthByX(skillDamage);

                            Condition conditionToApply = myCharacterController.getConditionFromCurrentSkill();

                            if(conditionToApply!=null && character.GetComponent<Condition>() == null) {
                                switch (conditionToApply.getName()) {
                                    case "Dazed":character.AddComponent<Dazed>();break;
                                }
                            }
                        }
                    }
                }
            }
        }

        foreach (Transform destructableObject in destructableObjects.transform) {
            if (destructableObject != null) {
                int characterPosition = destructableObject.GetComponent<CharacterPosition>().getTileID();
                for (int i = 0; i < tilesEffectedByPlayerSkill.Length; i++) {
                    if (tilesEffectedByPlayerSkill[i] == characterPosition) {
                        int skillDamage = myCharacterController.getDamageFromCurrentSkill();
                        destructableObject.GetComponent<Health>().decrementCurrentHealthByX(skillDamage);
                    }
                }
            }
        }

        if (currentState == GameState.IN_COMBAT && enemyHit) {
            myCharacterController.decrementActionPointsByAttack();
            myCharacterController.incrementSkillUsage();
        }
    }

    private void moveCharacterToTile(GameObject playerWhosTurnItIs, int tileID) {
        CharacterController myCharacterController = playerWhosTurnItIs.GetComponent<CharacterController>();
        CharacterMovementController myCharacterMovementController = playerWhosTurnItIs.GetComponent<CharacterMovementController>();

        if (myCharacterController.getCurrentActionPoints() < 2 && currentState == GameState.IN_COMBAT) return;

        List<Node> path = new List<Node>();
        Node savedNode = myTileController.getLastTraversalPath();
        while (savedNode != null) {
            if (savedNode.getDistanceFromStart() < myCharacterController.getMovementSpeed())
                path.Insert(0, savedNode);
            else
                myTileController.getTileFromID(savedNode.getID()).GetComponent<Tile>().setCurrentState(Tile.TileState.NOT_SELECTED);
            savedNode = savedNode.getPreviousNode();
        }

        if (path.Count >= 0) {
            myCharacterMovementController.setPath(path);
            if (currentState == GameState.IN_COMBAT) myCharacterController.decrementActionPointsByMovement();
        }
    }

    public void onTileHover(int index) {
        GameObject playerWhosTurnItIs = (currentState != GameState.IN_COMBAT) ? player : myInitativeController.getPlayerWhosTurnItIs();

        if (playerWhosTurnItIs == null || playerWhosTurnItIs.GetComponent<CharacterMovementController>().isCharacterMoving()) return;
        if (onUI && playerWhosTurnItIs == player) return;

        switch (playerWhosTurnItIs.GetComponent<CharacterController>().getCurrentCharacterState()) {
            case CharacterController.CharacterState.MOVE:
                myTileController.highlightTraversalPathFromCharacterToTile(playerWhosTurnItIs, index);
                break;

            case CharacterController.CharacterState.ATTACK:
                myTileController.highlightAttackFromCharacter(playerWhosTurnItIs, index);
                break;
        }
    }

    public void onTileSelect(int tileID) {
        GameObject playerWhosTurnItIs = (currentState != GameState.IN_COMBAT) ? player : myInitativeController.getPlayerWhosTurnItIs();

        if (playerWhosTurnItIs == null || playerWhosTurnItIs.GetComponent<CharacterMovementController>().isCharacterMoving()) return;
        if (onUI && playerWhosTurnItIs == player) return;

        switch (playerWhosTurnItIs.GetComponent<CharacterController>().getCurrentCharacterState()) {
            case CharacterController.CharacterState.MOVE:
                moveCharacterToTile(playerWhosTurnItIs, tileID);
                break;

            case CharacterController.CharacterState.ATTACK:
                haveCharacterPreformAttackOnTile(playerWhosTurnItIs, tileID);
                break;
        }
    }
  
    public void updateTraversalMap(bool forceUpdate) {
        foreach(GameObject character in allCharacters) {
            if (character != null && (character.GetComponent<CharacterMovementController>().doesTraversalMapNeedToBeUpdated() || forceUpdate))
                myTileController.updateCharactersTraversalMap(character);
        }
    }

    private void setAllCharacterStatesToX(CharacterController.CharacterState newCharacterState) {
        foreach (GameObject character in allCharacters) {
            if (character != null) character.GetComponent<CharacterController>().setCurrentCharacterState(newCharacterState);
        }
    }

    void Update() {
        if(currentState == GameState.IDLE) {
            Debug.Log("IDLE GAMESTATE");

            if (player != null) player.GetComponent<CharacterController>().resetActionPoints();
            
            if (myInitativeController.isPlayerWithinRangeOfEnemy()) {
                currentState = GameState.IN_COMBAT;
                setAllCharacterStatesToX(CharacterController.CharacterState.IDLE);
            }
        }else if(currentState == GameState.IN_COMBAT) {
            Debug.Log("COMBAT STATE");
            myInitativeController.addCharactersToIntiative();

            GameObject characterWhosTurnItIs = myInitativeController.getPlayerWhosTurnItIs();
            if (characterWhosTurnItIs == null) myInitativeController.endTurn(); 
            else {
                CharacterController myCharacterController = characterWhosTurnItIs.GetComponent<CharacterController>();
                if (myCharacterController.getCurrentCharacterState() == CharacterController.CharacterState.IDLE) myCharacterController.startTurn();

                //Check if turn is over
                if (myCharacterController.getCurrentCharacterState() == CharacterController.CharacterState.TURN_OVER && !characterWhosTurnItIs.GetComponent<CharacterMovementController>().isCharacterMoving()) {
                    updateTraversalMap(true);
                    myCharacterController.setCurrentCharacterState(CharacterController.CharacterState.IDLE);
                    myInitativeController.endTurn();
                }   
            }

            //Player Won Encounter
            if (myInitativeController.getNumberOfPlayersInIntiative() == 1) {
                if (player != null) {
                    currentState = GameState.IDLE;
                    CharacterController myCharacterController = player.GetComponent<CharacterController>();
                    myCharacterController.resetSkillsPerEncounter();
                    myCharacterController.startTurn();
                }
            }

            //Player Lost Encounter
            if(player== null) {
                currentState = GameState.IDLE;
                setAllCharacterStatesToX(CharacterController.CharacterState.IDLE);
            }

           
        }

        updateTraversalMap(false);
    }
}

