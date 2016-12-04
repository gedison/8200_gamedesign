using UnityEngine;
using System.Collections;

/* This class is responsible for the game's turn system
 */
public class InitativeController {

    private GameObject player;
    private GameObject enemies;
    private TileController myTileController;

    private int currentPlayerTurn = 0;
    private ArrayList charactersInIntiative = new ArrayList();

    public InitativeController(GameObject player, GameObject enemies, TileController myTileController) {
        this.player = player;
        this.enemies = enemies;
        this.myTileController = myTileController;
        charactersInIntiative.Add(player);
    }

    //Checks if the player is within eucledian distance of any enemy
    public bool isPlayerWithinRangeOfEnemy() {
        if (player != null) {
            int playerOrigin = player.GetComponent<CharacterPosition>().getTileID();
            int playerVisibilityRange = 4;// player.GetComponent<CharacterController>().visibilityRange;

            foreach (Transform enemy in enemies.transform) {
                if (enemy != null) { 
                    int enemyOrigin = enemy.GetComponent<CharacterPosition>().getTileID();
                    if (myTileController.getDistanceBetweenTwoTiles(playerOrigin, enemyOrigin) <= playerVisibilityRange) return true;
                }
            }
        }
        
        return false;
    }

    //Adds all characters within distance of player to intiative
    public void addCharactersToIntiative() {
        if (player != null) {
            int playerOrigin = player.GetComponent<CharacterPosition>().getTileID();
            int playerVisibilityRange = 4;// player.GetComponent<CharacterController>().visibilityRange;

            foreach (Transform enemy in enemies.transform) {
                int enemyOrigin = enemy.GetComponent<CharacterPosition>().getTileID();
                if (myTileController.getDistanceBetweenTwoTiles(playerOrigin, enemyOrigin) <= playerVisibilityRange) {
                    if (!charactersInIntiative.Contains(enemy.gameObject))
                        charactersInIntiative.Add(enemy.gameObject);
                }
            }
        }     
    }

    private void removeAllNullPlayers() {
        for(int i = charactersInIntiative.Count-1; i>=0; i--) {
            if (((GameObject)charactersInIntiative[i]) == null) {
                charactersInIntiative.RemoveAt(i);
            }
        }
    }

    public GameObject getPlayerWhosTurnItIs() {
        removeAllNullPlayers();
        if (currentPlayerTurn < 0 || currentPlayerTurn >= charactersInIntiative.Count) currentPlayerTurn = 0;
        return (GameObject) charactersInIntiative[currentPlayerTurn];
    }

    public void endTurn() {
        removeAllNullPlayers();
        currentPlayerTurn = ((currentPlayerTurn + 1) < charactersInIntiative.Count) ? (currentPlayerTurn + 1) : 0;
    }

    public int getNumberOfPlayersInIntiative() {
        return charactersInIntiative.Count;
    }
}
