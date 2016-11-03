﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldController : MonoBehaviour {

	public enum GameState { IDLE, IN_COMBAT };
	public GameState currentState = GameState.IDLE;

    public static WorldController instance = null;
    public GameObject tiles;
    public GameObject player;
    public GameObject enemies;
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

        allCharacters.Add(player);
        foreach (Transform enemy in enemies.transform) allCharacters.Add(enemy.gameObject);
    }

    public Transform getTileFromArrayIndex(int tileID) {
        return myTileController.getTileFromID(tileID);
    }

    public void switchTileIsOccupied(int tileID) {
        if(myTileController!=null)myTileController.switchIsTileOccupied(tileID);
    }

    public void onTileHover(int index) {
        GameObject playerWhosTurnItIs = (currentState != GameState.IN_COMBAT) ? player : myInitativeController.getPlayerWhosTurnItIs();
        if (playerWhosTurnItIs == null) return;
        if (playerWhosTurnItIs.GetComponent<CharacterMovementController>().isCharacterMoving()) return;
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
        if (playerWhosTurnItIs == null) return;
        if (playerWhosTurnItIs.GetComponent<CharacterMovementController>().isCharacterMoving()) return;
        if (onUI && playerWhosTurnItIs == player) return;

        CharacterController myCharacterController = playerWhosTurnItIs.GetComponent<CharacterController>();
        CharacterMovementController myCharacterMovementController = playerWhosTurnItIs.GetComponent<CharacterMovementController>();

        switch (myCharacterController.getCurrentCharacterState()) {
            case CharacterController.CharacterState.MOVE:
                if (myCharacterController.getCurrentActionPoints() < 2 && currentState == GameState.IN_COMBAT) return;
                List<Node> path = new List<Node>();
                Node savedNode = myTileController.getLastTraversalPath();
                while (savedNode != null) {
                    if (savedNode.getDistanceFromStart() < myCharacterMovementController.movementSpeed)
                        path.Insert(0, savedNode);
                    else
                        myTileController.getTileFromID(savedNode.getID()).GetComponent<Tile>().setCurrentState(Tile.TileState.NOT_SELECTED);
                    savedNode = savedNode.getPreviousNode();
                }

                if (path.Count > 0) {
                    myCharacterMovementController.setPath(path);
                    if (currentState == GameState.IN_COMBAT) myCharacterController.decrementActionPointsByMovement();
                }
                break;

            case CharacterController.CharacterState.ATTACK:
                if(myCharacterController.getCurrentActionPoints()<3 && currentState == GameState.IN_COMBAT)return;

                CharacterController.CharacterAttribute versus = myCharacterController.getCurrentSkillVersus();
                int skillDamage = myCharacterController.getDamageFromCurrentSkill();
                bool enemyHit = false;

                int[] tilesEffectedByPlayerSkill = myTileController.getLastAttack();

                foreach(GameObject character in allCharacters) {
                    if (character != null) { 
                        int characterPosition = character.GetComponent<CharacterPosition>().getTileID();
                        for (int i = 0; i < tilesEffectedByPlayerSkill.Length; i++) {
                            if (tilesEffectedByPlayerSkill[i] == characterPosition) {
                                enemyHit = true;
                                int playerRole = myCharacterController.roleD20ForCurrentSkill();
                                if (character != playerWhosTurnItIs && character.GetComponent<CharacterController>().roleD20UsingAttributeAsModifier(versus) < playerRole) {
                                    character.GetComponent<CharacterController>().myHealth.decrementCurrentHealthByX(skillDamage);
                                }
                            }
                        }
                    }
                }

                if (currentState == GameState.IN_COMBAT && enemyHit) myCharacterController.decrementActionPointsByAttack();
                break;
        }
    }
  
    public void updateTraversalMap(bool forceUpdate) {
        foreach(GameObject character in allCharacters) {
            if (character != null && (character.GetComponent<CharacterMovementController>().doesTraversalMapNeedToBeUpdated() || forceUpdate))
                myTileController.updateCharactersTraversalMap(character);
        }
    }

    void Update() {
        if(currentState == GameState.IDLE) {
            Debug.Log("IDLE GAMESTATE");
            if (myInitativeController.isPlayerWithinRangeOfEnemy()) {
                currentState = GameState.IN_COMBAT;
                player.GetComponent<CharacterController>().setCurrentCharacterState(CharacterController.CharacterState.IDLE);
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
                    myCharacterController.setCurrentCharacterState(CharacterController.CharacterState.IDLE);
                    myInitativeController.endTurn();
                }   
            }

            //Player Won Encounter
            if(myInitativeController.getNumberOfPlayersInIntiative() == 1) {
                if (player != null) {
                    currentState = GameState.IDLE;
                    CharacterController myCharacterController = player.GetComponent<CharacterController>();
                    myCharacterController.startTurn();
                }
            }

            //Player Lost Encounter
            if(player== null) {
                currentState = GameState.IDLE;
                foreach (Transform enemy in enemies.transform) {
                    if (enemy != null) enemy.GetComponent<CharacterController>().setCurrentCharacterState(CharacterController.CharacterState.IDLE);
                }
            }

            updateTraversalMap(true);
        }

        updateTraversalMap(false);
    }
}

