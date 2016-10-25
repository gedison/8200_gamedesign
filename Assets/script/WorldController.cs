using UnityEngine;
using System.Collections.Generic;

public class WorldController : MonoBehaviour {

    public static WorldController instance = null;
    public GameObject tiles;
    public GameObject player;
    public GameObject enemies;

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
        int index = getTileIndexFromID(tileID);

        resetLastPath();

      
        //Player Skill Usage
        CharacterController myCharacterController = player.GetComponent<CharacterController>();
        tilesEffectedByPlayerSkill = myCharacterController.getTilesEffectedByCurrentSkill(index);
        for (int i = 0; i < tilesEffectedByPlayerSkill.Length; i++) {
            tileArray[tilesEffectedByPlayerSkill[i]].GetComponent<Tile>().setCurrentState(Tile.TileState.SELECTED_WITHIN_RANGE);
        }
        



 

        /*
         * //Player Movement
        int index = getTileIndexFromID(tileID);
        if (index != -1) {
            if (traversalMap!= null && traversalMap[index] != null) { 
                Node selectedNode = traversalMap[index];
                savedNode = traversalMap[index];
                while (selectedNode != null) {
                    int distanceFromStart = selectedNode.getDistanceFromStart();
                    if (distanceFromStart < player.GetComponent<CharacterMovementController>().movementSpeed) tileArray[selectedNode.getID()].GetComponent<Tile>().setCurrentState(Tile.TileState.SELECTED_WITHIN_RANGE);
                    else tileArray[selectedNode.getID()].GetComponent<Tile>().setCurrentState(Tile.TileState.SELECTED_OUTSIDE_RANGE);
                    selectedNode = selectedNode.getPreviousNode();
                }
            }
        }*/
    }

    public void onTileSelect(int tileID) {
        if (player.GetComponent<CharacterMovementController>().isCharacterMoving()) return;

        //Player Skill Usage
        CharacterController myCharacterController = player.GetComponent<CharacterController>();
        CharacterController.CharacterAttribute versus = myCharacterController.getCurrentSkillVersus();
        int skillDamage = myCharacterController.getDamageFromCurrentSkill();
        foreach (Transform enemy in enemies.transform) {
            int enemyPosition = getTileIndexFromID(enemy.GetComponent<CharacterPosition>().getCurrentInstanceID());
            Debug.Log(enemyPosition);
            for(int i=0; i<tilesEffectedByPlayerSkill.Length; i++) {
                if(tilesEffectedByPlayerSkill[i] == enemyPosition) {
                    int playerRole = myCharacterController.roleD20ForCurrentSkill();
                    Debug.Log(playerRole + " " + enemy.GetComponent<CharacterController>().roleD20UsingAttributeAsModifier(versus));
                    if (enemy.GetComponent<CharacterController>().roleD20UsingAttributeAsModifier(versus) < playerRole) {
                        enemy.GetComponent<CharacterController>().myHealth.decrementCurrentHealthByX(skillDamage);
                        Debug.Log("HIT: " + skillDamage+" ENEMY HEALTH: "+enemy.GetComponent<CharacterController>().myHealth.getCurrentHealth());
                    }       
                }
            }     
         }


        /*
        //Player Movement
        List<Node> path = new List<Node>();
        while (savedNode != null) {
            if (savedNode.getDistanceFromStart() < player.GetComponent<CharacterMovementController>().movementSpeed) path.Insert(0, savedNode);
            else tileArray[savedNode.getID()].GetComponent<Tile>().setCurrentState(Tile.TileState.NOT_SELECTED);
            savedNode = savedNode.getPreviousNode();
        }

        if(path.Count>0)player.GetComponent<CharacterMovementController>().setPath(path);
        */
    }

    void Update() {
        int currentID = player.GetComponent<CharacterPosition>().getCurrentInstanceID();
        if (currentID != lastID) {
            traversalMap = tileTraverser.getTileTrafersal(getTileIndexFromID(currentID));
            lastID = currentID;
        }
    }
}

